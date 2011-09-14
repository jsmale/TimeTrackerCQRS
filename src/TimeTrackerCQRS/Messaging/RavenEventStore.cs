using System;
using System.Collections.Generic;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Serialization;
using Raven.Client;
using Raven.Client.Embedded;
using TimeTrackerCQRS.Events;
using System.Linq;

namespace TimeTrackerCQRS.Messaging
{
    public class RavenEventStore : IEventStore
    {
        static IStoreEvents store;

        public RavenEventStore(IEventPublisher publisher)
        {
            InitStore(publisher);
        }

        public static IStoreEvents Store
        {
            get { return store; }
        }
        public static void InitStore(IEventPublisher publisher)
        {
            if (store != null) return;
            store = Wireup.Init()
                .UsingMyRavenPersistence()
                .UsingAsynchronousDispatcher(new DelegateMessagePublisher(x => publisher.Publish(x.Events.Select(y => (Event)y.Body))))
                .Build();
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            using (var stream = store.OpenStream(aggregateId, 0, int.MaxValue))
            {
                var existingEvents = stream.CommittedEvents.Select(x => (Event)x.Body).ToArray();
                if (existingEvents.Length > 0 && existingEvents.Last().Version != expectedVersion && expectedVersion != -1)
                {
                    throw new ConcurrencyException();
                }
                var i = expectedVersion;
                foreach (var @event in events)
                {
                    i++;
                    @event.Version = i;
                    stream.Add(new EventMessage { Body = @event });
                }
                stream.CommitChanges(Guid.NewGuid());
            }
        }

        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            using (var stream = store.OpenStream(aggregateId, 0, int.MaxValue))
            {
                return stream.CommittedEvents.Select(x => (Event)x.Body).ToList();
            }
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return store.GetFrom(new DateTime(2000, 1, 1))
                .OrderBy(x => x.CommitStamp)
                .SelectMany(x => x.Events)
                .Select(x => (Event)x.Body);
        }
    }
}
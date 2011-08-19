using System;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.Domain
{
    public class Repository : IRepository
    {
        private readonly IEventStore storage;

        public Repository(IEventStore storage)
        {
            this.storage = storage;
        }

        public void Save(AggregateRoot aggregate, int expectedVersion)
        {
            storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        }

        public T GetById<T>(Guid id) where T : AggregateRoot, new()
        {
            var aggregate = new T();
            var events = storage.GetEventsForAggregate(id);
            aggregate.LoadsFromHistory(events);
            return aggregate;
        }
    }
}
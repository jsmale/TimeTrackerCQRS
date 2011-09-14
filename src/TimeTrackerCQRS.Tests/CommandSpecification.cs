using System;
using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Domain;
using TimeTrackerCQRS.Events;
using TimeTrackerCQRS.Messaging;
using System.Linq;

namespace TimeTrackerCQRS.Tests
{
    [TestClass]
    public abstract class CommandSpecification<THandler, TCommand> 
        where TCommand : ICommand 
        where THandler : IHandles<TCommand>
    {
        private TestEventStore eventStore;
        private THandler handler;
        protected Exception exception;
        private static readonly CompareObjects comparer = new CompareObjects();
        protected TCommand command;

        [TestInitialize]
        public void Init()
        {
            eventStore = new TestEventStore(Given());
            handler = CreateHandler(new Repository(eventStore));
            exception = null;
            try
            {
                command = When();
                handler.Handle(command);
            }
            catch(Exception ex)
            {
                exception = ex;
            }
        }

        protected abstract THandler CreateHandler(IRepository repository);

        protected void AssertEventOccured(Event @event)
        {
            Assert.IsTrue(eventStore.SavedEvents.Any(x => comparer.Compare(x, @event)), "Event not found");
        }

        protected abstract IEnumerable<Event> Given();
        protected abstract TCommand When();

        class TestEventStore : IEventStore
        {
            private readonly List<Event> eventHistory;
            private IList<Event> savedEvents;
            
            public TestEventStore(IEnumerable<Event> eventHistory)
            {
                this.eventHistory = new List<Event>(eventHistory);
            }

            public IEnumerable<Event> SavedEvents
            {
                get { return savedEvents.AsEnumerable(); }
            }

            public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
            {
                savedEvents = new List<Event>(events);
            }

            public List<Event> GetEventsForAggregate(Guid aggregateId)
            {
                return eventHistory;
            }

            public IEnumerable<Event> GetAllEvents()
            {
                return eventHistory;
            }
        }
    }
}
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

        [TestInitialize]
        public void Init()
        {
            eventStore = new TestEventStore(Given());
            handler = CreateHandler<THandler>(new Repository(eventStore));
            exception = null;
            try
            {
                handler.Handle(When());
            }
            catch(Exception ex)
            {
                exception = ex;
            }
        }

        private T CreateHandler<T>(IRepository repository)
        {
            return (T)typeof(THandler).GetConstructors()[0].Invoke(new object[] {repository});
        }

        protected void AssertEventOccured(IEvent @event)
        {
            Assert.IsTrue(eventStore.SavedEvents.Any(x => comparer.Compare(x, @event)), "Event not found");
        }

        protected abstract IEnumerable<IEvent> Given();
        protected abstract TCommand When();

        class TestEventStore : IEventStore
        {
            private readonly List<IEvent> eventHistory;
            private IList<IEvent> savedEvents;
            
            public TestEventStore(IEnumerable<IEvent> eventHistory)
            {
                this.eventHistory = new List<IEvent>(eventHistory);
            }

            public IEnumerable<IEvent> SavedEvents
            {
                get { return savedEvents.AsEnumerable(); }
            }

            public void SaveEvents(Guid aggregateId, IEnumerable<IEvent> events, int expectedVersion)
            {
                savedEvents = new List<IEvent>(events);
            }

            public List<IEvent> GetEventsForAggregate(Guid aggregateId)
            {
                return eventHistory;
            }
        }
    }
}
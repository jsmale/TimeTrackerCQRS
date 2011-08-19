using System;
using System.Collections.Generic;
using TimeTrackerCQRS.Events;

namespace TimeTrackerCQRS.Messaging
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<IEvent> events, int expectedVersion);
        List<IEvent> GetEventsForAggregate(Guid aggregateId);
    }

    public class AggregateNotFoundException : Exception
    {
    }

    public class ConcurrencyException : Exception
    {
    }
}
using System;
using System.Collections.Generic;
using TimeTrackerCQRS.Events;

namespace TimeTrackerCQRS.Messaging
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        List<Event> GetEventsForAggregate(Guid aggregateId);
        IEnumerable<Event> GetAllEvents();
    }

    public class AggregateNotFoundException : Exception
    {
    }

    public class ConcurrencyException : Exception
    {
    }
}
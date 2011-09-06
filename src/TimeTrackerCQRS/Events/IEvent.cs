using System;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.Events
{
    public interface IEvent : IMessage
    {
        int Version { get; set;  }
    }
    public class Event : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
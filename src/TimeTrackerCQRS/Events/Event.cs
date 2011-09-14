using System;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.Events
{
    public class Event : IMessage
    {
        public int Version { get; set;  }
    }
}
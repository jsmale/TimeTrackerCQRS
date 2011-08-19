using System;

namespace TimeTrackerCQRS.Events
{
    public class TaskCreated : Event
    {
        public TaskCreated(Guid id)
        {
            Id = id;
        }
    }
}
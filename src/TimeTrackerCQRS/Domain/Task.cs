using System;
using TimeTrackerCQRS.Events;

namespace TimeTrackerCQRS.Domain
{
    public class Task : AggregateRoot
    {
        private Guid id;

        public Task()
        {
        }

        public Task(Guid id)
        {
            ApplyChange(new TaskCreated(id));
        }

        public override Guid Id
        {
            get { return id; }
        }

        private void Apply(TaskCreated e)
        {
            id = e.Id;
        }
    }
}
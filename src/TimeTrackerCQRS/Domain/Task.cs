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

        public Task(Guid id, string task, string project)
        {
            ApplyChange(new TaskCreated(id, task, project));
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
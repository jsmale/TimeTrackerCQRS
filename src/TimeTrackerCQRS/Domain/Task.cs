using System;
using TimeTrackerCQRS.Events;

namespace TimeTrackerCQRS.Domain
{
    public class Task : AggregateRoot
    {
        private Guid id;
        private DateTime? lastStartTime;

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

        public void Start(DateTime? startTime, string comment)
        {
            if (lastStartTime.HasValue)
            {
                throw new InvalidOperationException("Task already started");
            }
            ApplyChange(new TaskStarted(id, startTime, comment));
        }

        private void Apply(TaskStarted e)
        {
            lastStartTime = e.StartTime;            
        }

        public void Stop(DateTime? stopTime, string comment)
        {
            if (lastStartTime == null)
            {
                throw new InvalidOperationException("Task already stopped");
            }
            ApplyChange(new TaskStopped(id, stopTime, comment));
        }

        private void Apply(TaskStopped e)
        {
            lastStartTime = null;
        }
    }
}
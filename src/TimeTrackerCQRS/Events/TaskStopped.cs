using System;
using TimeTrackerCQRS.Infrastructure;

namespace TimeTrackerCQRS.Events
{
    public class TaskStopped : Event
    {
        readonly Guid taskId;
        readonly DateTime? stopTime;
        readonly string comment;

        public TaskStopped(Guid taskId, DateTime? stopTime, string comment)
        {
            this.taskId = taskId;
            this.stopTime = stopTime.GetValueOrDefault(ServiceLocator.DateTimeService.GetUtcNow());
            this.comment = comment;
        }

        public Guid TaskId
        {
            get { return taskId; }
        }

        public string Comment
        {
            get { return comment; }
        }

        public DateTime? StopTime
        {
            get { return stopTime; }
        }
    }
}
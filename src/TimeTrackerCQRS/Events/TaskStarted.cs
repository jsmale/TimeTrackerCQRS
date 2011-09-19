using System;
using TimeTrackerCQRS.Infrastructure;

namespace TimeTrackerCQRS.Events
{
    public class TaskStarted : Event
    {
        readonly Guid taskId;
        readonly DateTime startTime;
        readonly string comment;

        public TaskStarted(Guid taskId, DateTime startTime, string comment)
        {
            this.taskId = taskId;
            this.startTime = startTime;
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

        public DateTime? StartTime
        {
            get { return startTime; }
        }
    }
}
using System;

namespace TimeTrackerCQRS.Events
{
    public class TaskCreated : Event
    {
        private readonly string task;
        private readonly string project;

        public TaskCreated(Guid id, string task, string project)
        {
            this.task = task;
            this.project = project;
            Id = id;
        }

        public string Task
        {
            get { return task; }
        }

        public string Project
        {
            get { return project; }
        }
    }
}
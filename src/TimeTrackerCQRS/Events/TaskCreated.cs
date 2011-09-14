using System;

namespace TimeTrackerCQRS.Events
{
    public class TaskCreated : Event
    {
        readonly Guid id;
        readonly string task;
        readonly string project;

        public TaskCreated(Guid id, string task, string project)
        {
            this.id = id;
            this.task = task;
            this.project = project;
        }

        public Guid Id
        {
            get { return id; }
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
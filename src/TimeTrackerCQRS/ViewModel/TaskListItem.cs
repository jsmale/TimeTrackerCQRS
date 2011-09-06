using System;

namespace TimeTrackerCQRS.ViewModel
{
    public class TaskListItem
    {
        public Guid Id { get; set; }
        public string Task { get; set; }
        public string Project { get; set; }
        public DateTime? LastStartTime { get; set; }
    }
}
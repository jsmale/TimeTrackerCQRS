using System;

namespace TimeTrackerCQRS.ViewModel
{
    public class TaskDetail
    {
        public Guid Id { get; set; }
        public string Task { get; set; }
        public string Project { get; set; }
        public bool Started { get; set; }
        public DateTime? StartTime { get; set; }
        public string LastNote { get; set; }
        public int Version { get; set; }
    }
}

using System;

namespace TimeTrackerCQRS.Commands
{
    public class CreateTask : Command
    {
        public string Task { get; set; }
        public string Project { get; set; }
    }
}
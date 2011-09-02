using System;

namespace TimeTrackerCQRS.Commands
{
    public class CreateTask : Command
    {
        public CreateTask(Guid id)
        {
            Id = id;
        }
    }
}
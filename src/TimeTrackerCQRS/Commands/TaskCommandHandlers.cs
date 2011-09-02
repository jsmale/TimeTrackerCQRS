using TimeTrackerCQRS.Domain;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.Commands
{
    public class TaskCommandHandlers : IHandles<CreateTask>
    {
        private readonly IRepository repository;

        public TaskCommandHandlers(IRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(CreateTask message)
        {
            var task = new Task(message.Id);
            repository.Save(task, 0);
        }
    }
}
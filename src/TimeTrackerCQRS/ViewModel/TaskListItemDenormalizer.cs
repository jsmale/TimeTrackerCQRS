using TimeTrackerCQRS.Events;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.ViewModel
{
    public class TaskListItemDenormalizer : IHandles<TaskCreated>
    {
        private readonly IPersistentViewModel persistentViewModel;

        public TaskListItemDenormalizer(IPersistentViewModel persistentViewModel)
        {
            this.persistentViewModel = persistentViewModel;
        }

        public void Handle(TaskCreated message)
        {
            var taskListItem = new TaskListItem
            {
                Id = message.Id,
                Task = message.Task,
                Project = message.Project
            };
            persistentViewModel.Insert(taskListItem);
        }
    }
}
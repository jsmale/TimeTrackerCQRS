using System;
using System.Linq;
using TimeTrackerCQRS.Events;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.ViewModel
{
    public class TaskListItemDenormalizer : IHandles<TaskCreated>, IHandles<TaskStarted>
    {
        private readonly IPersistentViewModelFactory persistentViewModelFactory;

        public TaskListItemDenormalizer(IPersistentViewModelFactory persistentViewModelFactory)
        {
            this.persistentViewModelFactory = persistentViewModelFactory;
        }

        public void Handle(TaskCreated message)
        {
            var taskListItem = new TaskListItem
            {
                Id = message.Id,
                Task = message.Task,
                Project = message.Project
            };
            using (var viewModel = persistentViewModelFactory.GetPersitentViewModel())
            {
                viewModel.Insert(taskListItem);
                viewModel.SaveChanges();
            }
        }

        public void Handle(TaskStarted message)
        {
            using (var viewModel = persistentViewModelFactory.GetPersitentViewModel())
            {
                var item = GetListItem(message.TaskId, viewModel);
                item.LastStartTime = message.StartTime;
                viewModel.SaveChanges();
            }
        }

        TaskListItem GetListItem(Guid id, IPersistentViewModel viewModel)
        {
            return viewModel.Query<TaskListItem>().Single(x => x.Id == id);
        }
    }
}
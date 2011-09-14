using System;
using System.Linq;
using TimeTrackerCQRS.Events;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.ViewModel
{
    public class TaskDetailDenormalizer : IHandles<TaskCreated>, IHandles<TaskStarted>, IHandles<TaskStopped>
    {
        private readonly IPersistentViewModelFactory persistentViewModelFactory;

        public TaskDetailDenormalizer(IPersistentViewModelFactory persistentViewModelFactory)
        {
            this.persistentViewModelFactory = persistentViewModelFactory;
        }

        public void Handle(TaskCreated message)
        {
            var taskDetail = new TaskDetail
            {
                Id = message.Id,
                Task = message.Task,
                Project = message.Project,
                Version = message.Version
            };
            using (var viewModel = persistentViewModelFactory.GetPersitentViewModel())
            {
                viewModel.Insert(taskDetail);
                viewModel.SaveChanges();
            }
        }

        public void Handle(TaskStarted message)
        {
            using (var viewModel = persistentViewModelFactory.GetPersitentViewModel())
            {
                var detail = viewModel.Query<TaskDetail>().Single(x => x.Id == message.TaskId);
                detail.Started = true;
                detail.StartTime = message.StartTime;
                detail.LastNote = message.Comment;
                detail.Version = message.Version;
                viewModel.SaveChanges();
            }
        }

        public void Handle(TaskStopped message)
        {
            using (var viewModel = persistentViewModelFactory.GetPersitentViewModel())
            {
                var detail = viewModel.Query<TaskDetail>().Single(x => x.Id == message.TaskId);
                detail.Started = false;
                detail.StartTime = null;
                detail.LastNote = message.Comment;
                detail.Version = message.Version;
                viewModel.SaveChanges();
            }
        }
    }
}
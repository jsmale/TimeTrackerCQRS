using TimeTrackerCQRS.Events;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.ViewModel
{
    public class TaskEventHandlers : IHandles<TaskCreated>
    {
        private readonly IPersistentViewModel persistentViewModel;

        public TaskEventHandlers(IPersistentViewModel persistentViewModel)
        {
            this.persistentViewModel = persistentViewModel;
        }

        public void Handle(TaskCreated message)
        {
        }
    }
}
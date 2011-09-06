using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Domain;
using TimeTrackerCQRS.Events;
using TimeTrackerCQRS.Messaging;
using TimeTrackerCQRS.ViewModel;

namespace TimeTrackerCQRS.Infrastructure
{
    public static class Bootstrapper
    {
        public static void Bootstrap()
        {
            var bus = new FakeBus();
            var eventStore = new EventStore(bus);
            var repository = new Repository(eventStore);
            var taskCommandHandlers = new TaskCommandHandlers(repository);
            bus.RegisterHandler<CreateTask>(taskCommandHandlers.Handle);

            var persistentViewModel = new PersistentViewModel();
            var taskEventHandlers = new TaskListItemDenormalizer(persistentViewModel);
            bus.RegisterHandler<TaskCreated>(taskEventHandlers.Handle);

            ServiceLocator.CommandSender = bus;
            ServiceLocator.PersistentViewModel = persistentViewModel;
        }
    }
}
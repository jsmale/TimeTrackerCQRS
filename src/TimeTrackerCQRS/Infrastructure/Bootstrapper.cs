using System;
using System.Linq;
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
            var eventStore = new Messaging.EventStore(bus);
            var repository = new Repository(eventStore);
            var taskCommandHandlers = new TaskCommandHandlers(repository);
            bus.RegisterHandler<CreateTask>(taskCommandHandlers.Handle);
            bus.RegisterHandler<StartTask>(taskCommandHandlers.Handle);
            bus.RegisterHandler<StopTask>(taskCommandHandlers.Handle);

            var persistentViewModelFactory = new PersistentViewModelFactory();
            var taskEventHandlers = new TaskListItemDenormalizer(persistentViewModelFactory);
            var taskDetailEventHandlers = new TaskDetailDenormalizer(persistentViewModelFactory);
            bus.RegisterHandler<TaskCreated>(taskEventHandlers.Handle);
            bus.RegisterHandler<TaskStarted>(taskEventHandlers.Handle);
            bus.RegisterHandler<TaskCreated>(taskDetailEventHandlers.Handle);
            bus.RegisterHandler<TaskStarted>(taskDetailEventHandlers.Handle);
            bus.RegisterHandler<TaskStopped>(taskDetailEventHandlers.Handle);

            ServiceLocator.CommandSender = bus;
            ServiceLocator.EventPublisher = bus;
            ServiceLocator.PersistentViewModelFactory = persistentViewModelFactory;

//            bool loadAllEvents ;
//            using (var viewModel = persistentViewModelFactory.GetPersitentViewModel())
//            {
//                loadAllEvents = (viewModel.Query<TaskListItem>().Count() == 0);
//            }
//            if (loadAllEvents)
//            {
//                bus.Publish(eventStore.GetAllEvents());
//            }
        }
    }
}
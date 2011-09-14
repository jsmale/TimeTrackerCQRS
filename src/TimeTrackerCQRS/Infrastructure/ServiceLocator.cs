using TimeTrackerCQRS.Messaging;
using TimeTrackerCQRS.ViewModel;

namespace TimeTrackerCQRS.Infrastructure
{
    public static class ServiceLocator
    {
        public static ICommandSender CommandSender;
        public static IEventPublisher EventPublisher;
        public static IPersistentViewModelFactory PersistentViewModelFactory;
        public static IDateTimeService DateTimeService = new DateTimeService();
    }
}
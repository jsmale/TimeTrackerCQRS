using TimeTrackerCQRS.Messaging;
using TimeTrackerCQRS.ViewModel;

namespace TimeTrackerCQRS.Infrastructure
{
    public static class ServiceLocator
    {
        public static ICommandSender CommandSender;
        public static IPersistentViewModel PersistentViewModel;
    }
}
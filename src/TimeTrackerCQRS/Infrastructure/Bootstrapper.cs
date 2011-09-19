using System.Linq;
using StructureMap;
using StructureMap.Configuration.DSL;
using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Events;
using TimeTrackerCQRS.Messaging;
using TimeTrackerCQRS.ViewModel;

namespace TimeTrackerCQRS.Infrastructure
{
    public static class Bootstrapper
    {
		public static IContainer InitializeContainer(params Registry[] registries)
        {
			var container = new Container(new DefaultRegistry(registries));
			RegisterHandlers<ICommand>(container);
			RegisterHandlers<Event>(container);
            PopulateViewModelFromEvents(container);
			return container;
        }

    	static void PopulateViewModelFromEvents(Container container)
    	{
    		using (var viewModel = container.GetInstance<IPersistentViewModelFactory>().GetPersitentViewModel())
    		{
				if (viewModel.Query<TaskListItem>().Count() > 0) return;
    		}
    		var eventPublisher = container.GetInstance<IEventPublisher>();
			var eventStore = container.GetInstance<IEventStore>();
    		eventPublisher.Publish(eventStore.GetAllEvents());
    	}

    	static void RegisterHandlers<T>(IContainer container)
    	{
    		var bus = container.GetInstance<IBus>();
			var handlerType = typeof(IHandles<>);
			var messageType = typeof(T);
			var messageTypes = messageType.Assembly.GetTypes().Where(messageType.IsAssignableFrom);
			var assembly = messageType.Assembly;
			foreach (var concreteMessageType in messageTypes)
			{
				var messageHandlerType = handlerType.MakeGenericType(concreteMessageType);
				var messageHandlerTypes = assembly.GetTypes().Where(messageHandlerType.IsAssignableFrom);
				foreach (var type in messageHandlerTypes)
				{
					var implementation = container.GetInstance(type);
					var handleMethod = (from m in type.GetMethods()
										where m.Name == "Handle"
											&& m.GetParameters()[0].ParameterType == concreteMessageType
										select m).Single();
					bus.RegisterHandler(x => handleMethod.Invoke(implementation, new []{x}), concreteMessageType);
				}
			}
    	}
    }

	public class DefaultRegistry : Registry
	{
		public DefaultRegistry(params Registry[] registries)
		{
			foreach (var registry in registries)
			{
				IncludeRegistry(registry);
			}

			var bus = new FakeBus();
			var eventStore = new RavenEventStore(bus);
			var persistentViewModelFactory = new PersistentViewModelFactory();
			For<ICommandSender>().Use(bus);
			For<IEventPublisher>().Use(bus);
			For<IBus>().Use(bus);
			For<IEventStore>().Use(eventStore);
			For<IPersistentViewModelFactory>().Use(persistentViewModelFactory);

			Scan(a =>
			{
				a.AssemblyContainingType<DefaultRegistry>();
				a.WithDefaultConventions();
			});
		}
	}
}
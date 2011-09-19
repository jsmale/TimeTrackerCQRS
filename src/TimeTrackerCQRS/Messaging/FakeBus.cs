using System;
using System.Collections.Generic;
using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Events;

namespace TimeTrackerCQRS.Messaging
{
    public class FakeBus : IBus
    {
		private readonly Dictionary<Type, List<Action<object>>> _routes = new Dictionary<Type, List<Action<object>>>();

		public void RegisterHandler<T>(Action<T> handler) where T : IMessage
		{
			RegisterHandler(x => handler((T) x), typeof (T));
		}

    	public void RegisterHandler(Action<object> handler, Type type)
		{
			List<Action<object>> handlers;
			if (!_routes.TryGetValue(type, out handlers))
			{
				handlers = new List<Action<object>>();
				_routes.Add(type, handlers);
			}
			handlers.Add(handler);
    	}

    	public void Send<T>(T command) where T : ICommand
        {
            List<Action<object>> handlers;
            if (_routes.TryGetValue(typeof(T), out handlers))
            {
                if (handlers.Count != 1) throw new InvalidOperationException("cannot send to more than one handler");
				handlers[0](command);
            }
            else
            {
                throw new InvalidOperationException("no handler registered");
            }
        }

        public void Publish<T>(T @event) where T : Event
        {
            List<Action<object>> handlers;
            if (!_routes.TryGetValue(@event.GetType(), out handlers)) return;
            foreach (var handler in handlers)
            {
            	handler(@event);
                //dispatch on thread pool for added awesomeness
//				var handler1 = handler;
//				(new Task(() => handler1(@event))).Start();
            }
        }

        public void Publish<T>(IEnumerable<T> events) where T : Event
        {
            foreach (var @event in events)
            {
                Publish(@event);
            }
        }
    }

    public interface IHandles<T>
    {
        void Handle(T message);
    }

    public interface ICommandSender
    {
        void Send<T>(T command) where T : ICommand;

    }
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
        void Publish<T>(IEnumerable<T> events) where T : Event;
    }
	public interface IBus : ICommandSender, IEventPublisher
	{
		void RegisterHandler<T>(Action<T> handler) where T : IMessage;
		void RegisterHandler(Action<object> handler, Type type);
	}
}
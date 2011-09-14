using System;
using System.Collections.Generic;
using System.Threading;
using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Events;
using TimeTrackerCQRS.Infrastructure;

namespace TimeTrackerCQRS.Messaging
{
    public class FakeBus : ICommandSender, IEventPublisher
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();

        public void RegisterHandler<T>(Action<T> handler) where T : IMessage
        {
            List<Action<IMessage>> handlers;
            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<IMessage>>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add(DelegateAdjuster.CastArgument<IMessage, T>(x => handler(x)));
        }

        public void Send<T>(T command) where T : ICommand
        {
            List<Action<IMessage>> handlers;
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
            List<Action<IMessage>> handlers;
            if (!_routes.TryGetValue(@event.GetType(), out handlers)) return;
            foreach (var handler in handlers)
            {
                handler(@event);
                //dispatch on thread pool for added awesomeness
//                var handler1 = handler;
//                ThreadPool.QueueUserWorkItem(x => handler1(@event));
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
}
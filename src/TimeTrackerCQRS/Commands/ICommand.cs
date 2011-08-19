using System;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.Commands
{
    public interface ICommand : IMessage
    {
         
    }
    public class Command : ICommand
    {
        public Guid Id { get; protected set; }
    }
}
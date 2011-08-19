using System;

namespace TimeTrackerCQRS.Messaging
{
    public interface IMessage
    {
        Guid Id { get; }  
    }
}
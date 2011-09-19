using System;
using TimeTrackerCQRS.Domain;
using TimeTrackerCQRS.Infrastructure;
using TimeTrackerCQRS.Messaging;

namespace TimeTrackerCQRS.Commands
{
    public class TaskCommandHandlers : IHandles<CreateTask>, IHandles<StartTask>, IHandles<StopTask>
    {
        private readonly IRepository repository;
    	readonly IDateTimeService dateTimeService;

    	public TaskCommandHandlers(IRepository repository, IDateTimeService dateTimeService)
        {
        	this.repository = repository;
        	this.dateTimeService = dateTimeService;
        }

    	public void Handle(CreateTask message)
        {
            var task = new Task(message.Id, message.Task, message.Project);
            repository.Save(task, 0);
        }

        public void Handle(StartTask message)
        {
            var task = repository.GetById<Task>(message.CommandId);
			task.Start(message.StartTime, message.Comment, dateTimeService);
            repository.Save(task, message.OriginalVersion);
        }

        public void Handle(StopTask message)
        {
            var task = repository.GetById<Task>(message.CommandId);
			task.Stop(message.StopTime, message.Comment, dateTimeService);
            repository.Save(task, message.OriginalVersion);
        }
    }
}
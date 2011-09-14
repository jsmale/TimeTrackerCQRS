using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Domain;
using TimeTrackerCQRS.Events;

namespace TimeTrackerCQRS.Tests
{
    [TestClass]
    public class CreateTaskTests : CommandSpecification<TaskCommandHandlers, CreateTask>
    {
        protected override TaskCommandHandlers CreateHandler(IRepository repository)
        {
            return new TaskCommandHandlers(repository);
        }

        protected override IEnumerable<Event> Given()
        {
            return new Event[] {};
        }

        protected override CreateTask When()
        {
            return new CreateTask
            {
                Id = Guid.NewGuid(),
                Task = "Task",
                Project = "Project"
            };
        }

        [TestMethod]
        public void ShouldPublishTaskCreated()
        {
            AssertEventOccured(new TaskCreated(command.Id, command.Task, command.Project));
        }
    }
}
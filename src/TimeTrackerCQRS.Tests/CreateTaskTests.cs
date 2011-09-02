using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTrackerCQRS.Commands;
using TimeTrackerCQRS.Events;

namespace TimeTrackerCQRS.Tests
{
    [TestClass]
    public class CreateTaskTests : CommandSpecification<TaskCommandHandlers, CreateTask>
    {
        private CreateTask command;

        protected override IEnumerable<IEvent> Given()
        {
            return new IEvent[] {};
        }

        protected override CreateTask When()
        {
            command = new CreateTask(Guid.NewGuid());
            return command;
        }

        [TestMethod]
        public void ShouldPublishTaskCreated()
        {
            AssertEventOccured(new TaskCreated(command.Id));
        }
    }
}
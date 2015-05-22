using NUnit.Framework;
using Shared.Domain;

namespace SharedTest.DomainTest
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void TaskIsAnEntity()
        {
            const int TaskId = 2;
            var incompleteTask = new Task("this is a title", "this is a description", 13, 1, 1, TaskCategory.Other);

            var task = new Task(TaskId, incompleteTask);
            Assert.IsInstanceOf<Entity>(task);
        }

        [Test]
        public void IsInJamIfHasJamId()
        {
            var incompleteTask = new Task("Task", "Description", 23, 2, 1, TaskCategory.Other);
            var task = new Task(2, incompleteTask);

            task.AssignTaskToJam(2);
            Assert.IsTrue(task.IsInJam);
        }

        [Test]
        public void IsNotInJamIfNoJamIdFound()
        {
            var incompleteTask = new Task("Task", "Description", 23, 2, 1, TaskCategory.Other);
            var task = new Task(2, incompleteTask);

            Assert.IsFalse(task.IsInJam);
        }

        [Test]
        public void TaskHasAssignedId()
        {
            const int TaskId = 120;
            var incompleteTask = new Task("this is a title", "this is a description", 13, 1, 1, TaskCategory.Other);
            var task = new Task(TaskId, incompleteTask);
            Assert.AreEqual(TaskId, task.Id);
        }

        [Test]
        public void NewTaskWithoutIdIsSetAsNew()
        {
            var task = new Task("this is a title", "this is a description", 13, 1, 1, TaskCategory.Other);
            Assert.IsTrue(task.IsNew);
        }

        [Test]
        public void TaskHasBandId()
        {
            const int TeamId = 12;

            var task = new Task("this is a title", "this is a description", 13, TeamId, 1, TaskCategory.Other);
            Assert.AreEqual(TeamId, task.BandId);
        }

        [Test]
        public void TaskHasNoUserIfnoIdIsAssignedTest()
        {
            var task = new Task("hello", "description", 3, 1, 0, TaskCategory.Other);

            Assert.IsFalse(task.HasAssignedUser);
        }

        [Test]
        public void TaskHasNoPointsTest()
        {
            var task = new Task("hello", "description", 0, 1, 1, TaskCategory.Other);

            Assert.IsFalse(task.HasPoints);
        }

        [Test]
        public void CanSetTaskToCompletedTest()
        {
            var task = new Task("hello", "description", 0, 1, 1, TaskCategory.Other);

            task.IsCompleted = true;

            Assert.IsTrue(task.IsCompleted);
        }

        [Test]
        public void CannotSetCompletedTaskToIncompletedTest()
        {
            var task = new Task("hello", "description", 0, 1, 1, TaskCategory.Other);

            task.IsCompleted = true;

            task.IsCompleted = false;

            Assert.IsTrue(task.IsCompleted);
        }
    }
}
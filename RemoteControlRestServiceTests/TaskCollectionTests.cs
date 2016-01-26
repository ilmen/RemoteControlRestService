using NSubstitute;
using NUnit.Framework;
using RemoteControlRestService.Classes;
using RemoteControlRestService.Controllers;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class TaskCollectionTests
    {
        Task CreateTask(string guidString)
        {
            return CreateTask(new Guid(guidString));
        }

        Task CreateTask(Guid guid)
        {
            var SOME_TIME = new DateTime(2015, 12, 10);
            return new Task()
            {
                Id = guid,
                CreateTime = SOME_TIME,
                RunTime = SOME_TIME.AddSeconds(1),
                CommandType = "command"
            };
        }

        IList<Task> GetTaskCollection()
        {
            return new List<Task>()
            {
                CreateTask("{40BD4CAA-A505-4620-84B8-C78D4C830878}"),
                CreateTask("{45FD28F6-3425-4B22-B6D8-8CF174951362}"),
                CreateTask("{425ACFB2-B1E7-49F2-AC90-8D286F9144E1}"),
                CreateTask("{7DDC3432-AD41-4104-B4F9-36C09B3855BB}"),
                CreateTask("{1AFFAB20-2A05-434B-BB77-8B591C3D24FD}"),
            };
        }

        IFactory<IList<Task>> GetFakeTaskProvider(IList<Task> tasks)
        {
            var provider = Substitute.For<IFactory<IList<Task>>>();
            provider.Create().Returns(tasks);

            return provider;
        }

        IFactory<IEnumerable<string>> GetFakeCommandFactory()
        {
            var command = Substitute.For<IFactory<IEnumerable<string>>>();
            command.Create().Returns(new string[0]);

            return command;
        }

        TaskCollection GetController(IList<Task> tasks)
        {
            var stubValidator = ValidateTestHelper.GetFakeTaskValidator(ValidResult.Valid);
            var taskFactory = GetFakeTaskProvider(tasks);
            var commandFactory = GetFakeCommandFactory();

            return new TaskCollection(stubValidator, taskFactory, commandFactory);
        }

        [Test]
        public void GetAll_Always_UseTaskCollectionFactory()
        {
            var expected = GetTaskCollection();
            var controller = GetController(expected);

            var tasks = controller.GetAll();

            Assert.AreEqual(expected, tasks);
        }

        [Test]
        public void GetOne_CorrectId_RetunsTask()
        {
            var expected = CreateTask("{BC2CF9DA-271B-4E19-AF8D-859F0691195D}");
            var tasks = GetTaskCollection();
            tasks.Add(expected);
            var controller = GetController(tasks);

            var task = controller.GetOne(expected.Id);

            Assert.AreEqual(expected, task);
        }

        [Test]
        public void GetOne_WrongId_RetunsNull()
        {
            var wrongId = new Guid("{F0756779-4AE3-4012-9FF1-231E36A25A40}");
            var tasks = GetTaskCollection();
            var controller = GetController(tasks);

            var nullTask = controller.GetOne(wrongId);

            Assert.Null(nullTask);
        }

        [Test]
        public void Delete_CorrectId_RemovedTask()
        {
            var tasks = GetTaskCollection();
            var guid = tasks.First().Id;
            var controller = GetController(tasks);

            controller.Delete(guid);

            Assert.IsTrue(tasks.Count(x => x.Id == guid) == 0);
        }

        [Test]
        public void Delete_WrongId_ThrownArgumentException()
        {
            var wrongId = new Guid("{F0756779-4AE3-4012-9FF1-231E36A25A40}");
            var tasks = GetTaskCollection();
            var countBeforeDelete = tasks.Count;
            var controller = GetController(tasks);

            var ex = Assert.Catch<ArgumentException>(() => controller.Delete(wrongId));
            StringAssert.Contains("Задача с таким Id не найдена", ex.Message);
        }

        [Test]
        public void Insert_NewTask_TaskAddedToCollection()
        {
            var newTask = CreateTask("{340DC812-C25B-491B-97BA-DAE3E52680E0}");
            var tasks = GetTaskCollection();
            var controller = GetController(tasks);

            controller.Insert(newTask);

            CollectionAssert.Contains(tasks, newTask);
        }

        [Test]
        public void Insert_NullTask_ThrowsArgumentNullException()
        {
            var tasks = GetTaskCollection();
            var controller = GetController(tasks);

            Assert.Catch<ArgumentNullException>(() => controller.Insert(null));
        }

        [Test]
        public void Update_Always_TaskUpdatedInCollection()
        {
            var guid = new Guid("{D82FA792-6FCE-4F19-A4E0-3B7FCF7C28C9}");
            var oldTask = CreateTask(guid);
            var newTask = CreateTask(guid);
            var otherTask = CreateTask("{3B6FDF0F-0789-4E37-A95B-1B485CDCB9B2}");
            var tasks = new List<Task>() { oldTask, otherTask };
            var controller = GetController(tasks);
            Assert.IsFalse(Object.ReferenceEquals(tasks.Single(x => x.Id == guid), newTask));

            controller.Update(oldTask.Id, newTask);

            Assert.IsTrue(Object.ReferenceEquals(tasks.Single(x => x.Id == guid), newTask));
        }

        [Test]
        public void Update_NullTask_ThrowsArgumentNullException()
        {
            var tasks = GetTaskCollection();
            var controller = GetController(tasks);

            Assert.Catch<ArgumentNullException>(() => controller.Update(Arg.Any<Guid>(), null));
        }

        [Test]
        public void Update_IdParameterAndTaskIdFieldNoMatch_ThrowsArgumentException()
        {
            var TASK_ID = new Guid("{866A4D3C-71C3-4AE4-B4EA-EBA0855EFCD6}");
            var OTHER_TASK_ID = new Guid("{005541BD-F397-4DF2-8C46-C886AA02D5E5}");
            var tasks = GetTaskCollection();
            var controller = GetController(tasks);

            Assert.Catch<ArgumentException>(() => controller.Update(TASK_ID, CreateTask(OTHER_TASK_ID)));
        }

        [Test]
        public void Update_WrongTaskId_ThrowsArgumentException()
        {
            var TASK_ID = new Guid("{866A4D3C-71C3-4AE4-B4EA-EBA0855EFCD6}");
            var OTHER_TASK_ID = new Guid("{005541BD-F397-4DF2-8C46-C886AA02D5E5}");
            var task = CreateTask(TASK_ID);
            var controller = GetController(new List<Task>() { task });

            Assert.Catch<ArgumentException>(() => controller.Update(OTHER_TASK_ID, task));
        }

        [Test]
        public void Update_DublicatedTasks_ThrowsArgumentException()
        {
            var oneTask = CreateTask("{866A4D3C-71C3-4AE4-B4EA-EBA0855EFCD6}");
            var tasks = new List<Task>() { oneTask, oneTask };
            var controller = GetController(tasks);

            Assert.Catch<ArgumentException>(() => controller.Update(oneTask.Id, Arg.Any<Task>()));
        }
    }
}

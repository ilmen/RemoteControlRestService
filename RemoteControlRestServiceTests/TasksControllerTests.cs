using NSubstitute;
using NUnit.Framework;
using RemoteControlRestService.Controllers;
using RemoteControlRestService.Infrastracture;
using RemoteControlRestService.Infrastracture.Commands;
using RemoteControlRestService.Infrastracture.Tasks;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class TasksControllerTests
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
                Cmd = new Command()
                {
                    Id = 1,
                    FilePath = "stub.bat",
                    Name = "Stub"
                }
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

        TasksController GetController()
        {
            var stubValidator = ValidateTestHelper.GetFakeTaskValidator(ValidResult.Valid);
            return new TasksController(stubValidator);
        }

        [Test]
        public void GetAll_Always_UseTaskCollectionFactory()
        {
            var expected = GetTaskCollection();
            TaskCollectionFactory.SetCollection(expected);
            var controller = GetController();

            var tasks = controller.Get();

            Assert.AreEqual(expected, tasks);
        }

        [Test]
        public void Get_CorrectId_RetunsTask()
        {
            var expected = CreateTask("{BC2CF9DA-271B-4E19-AF8D-859F0691195D}");
            var tasks = GetTaskCollection();
            tasks.Add(expected);
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            var task = controller.Get(expected.Id);

            Assert.AreEqual(expected, task);
        }

        [Test]
        public void Get_WrongId_RetunsNull()
        {
            var wrongId = new Guid("{F0756779-4AE3-4012-9FF1-231E36A25A40}");
            var tasks = GetTaskCollection();
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            var nullTask = controller.Get(wrongId);

            Assert.Null(nullTask);
        }

        [Test]
        public void Delete_CorrectId_RemovedTask()
        {
            var tasks = GetTaskCollection();
            var guid = tasks.First().Id;
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            controller.Delete(guid);

            Assert.IsTrue(tasks.Count(x => x.Id == guid) == 0);
        }

        [Test]
        public void Delete_WrongId_TaskCollectionNotChanged()
        {
            var wrongId = new Guid("{F0756779-4AE3-4012-9FF1-231E36A25A40}");
            var tasks = GetTaskCollection();
            var countBeforeDelete = tasks.Count;
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            controller.Delete(wrongId);

            Assert.IsTrue(tasks.Count == countBeforeDelete);
        }

        [Test]
        public void Post_NewTask_TaskAddedToCollection()
        {
            var newTask = CreateTask("{340DC812-C25B-491B-97BA-DAE3E52680E0}");
            var tasks = GetTaskCollection();
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            controller.Post(newTask);

            CollectionAssert.Contains(tasks, newTask);
        }

        [Test]
        public void Post_NullTask_ThrowsArgumentNullException()
        {
            var tasks = GetTaskCollection();
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            Assert.Catch<ArgumentNullException>(() => controller.Post(null));
        }

        [Test]
        public void Put_Always_TaskUpdatedInCollection()
        {
            var guid = new Guid("{D82FA792-6FCE-4F19-A4E0-3B7FCF7C28C9}");
            var oldTask = CreateTask(guid);
            var newTask = CreateTask(guid);
            var otherTask = CreateTask("{3B6FDF0F-0789-4E37-A95B-1B485CDCB9B2}");
            var tasks = new List<Task>() { oldTask, otherTask };
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();
            Assert.IsFalse(Object.ReferenceEquals(tasks.Single(x => x.Id == guid), newTask));

            controller.Put(oldTask.Id, newTask);

            Assert.IsTrue(Object.ReferenceEquals(tasks.Single(x => x.Id == guid), newTask));
        }

        [Test]
        public void Put_NullTask_ThrowsArgumentNullException()
        {
            var tasks = GetTaskCollection();
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            Assert.Catch<ArgumentNullException>(() => controller.Put(Arg.Any<Guid>(), null));
        }

        [Test]
        public void Put_IdParameterAndTaskIdFieldNoMatch_ThrowsArgumentException()
        {
            var TASK_ID = new Guid("{866A4D3C-71C3-4AE4-B4EA-EBA0855EFCD6}");
            var OTHER_TASK_ID = new Guid("{005541BD-F397-4DF2-8C46-C886AA02D5E5}");
            var tasks = GetTaskCollection();
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            Assert.Catch<ArgumentException>(() => controller.Put(TASK_ID, CreateTask(OTHER_TASK_ID)));
        }

        [Test]
        public void Put_WrongTaskId_ThrowsArgumentException()
        {
            var TASK_ID = new Guid("{866A4D3C-71C3-4AE4-B4EA-EBA0855EFCD6}");
            var OTHER_TASK_ID = new Guid("{005541BD-F397-4DF2-8C46-C886AA02D5E5}");
            var tasks = new List<Task>() { CreateTask(TASK_ID) };
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            Assert.Catch<ArgumentException>(() => controller.Put(OTHER_TASK_ID, Arg.Any<Task>()));
        }

        [Test]
        public void Put_DublicatedTasks_ThrowsArgumentException()
        {
            var oneTask = CreateTask("{866A4D3C-71C3-4AE4-B4EA-EBA0855EFCD6}");
            var tasks = new List<Task>() { oneTask, oneTask };
            TaskCollectionFactory.SetCollection(tasks);
            var controller = GetController();

            Assert.Catch<ArgumentException>(() => controller.Put(oneTask.Id, Arg.Any<Task>()));
        }

        [TearDown]
        public void ResetTaskCollectionFactory()
        {
            TaskCollectionFactory.SetCollection(null);
        }
    }
}

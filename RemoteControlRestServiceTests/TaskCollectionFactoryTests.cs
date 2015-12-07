using NUnit.Framework;
using RemoteControlRestService.Infrastracture;
using RemoteControlRestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class TaskCollectionFactoryTests
    {
        IList<Task> GetTaskCollection()
        {
            return new List<Task>()
            {
                new Task() { Id = new Guid("{40BD4CAA-A505-4620-84B8-C78D4C830878}") },
                new Task() { Id = new Guid("{45FD28F6-3425-4B22-B6D8-8CF174951362}") },
                new Task() { Id = new Guid("{425ACFB2-B1E7-49F2-AC90-8D286F9144E1}") },
                new Task() { Id = new Guid("{7DDC3432-AD41-4104-B4F9-36C09B3855BB}") },
                new Task() { Id = new Guid("{1AFFAB20-2A05-434B-BB77-8B591C3D24FD}") },
            };
        }

        [Test]
        public void GetCollection_CorrectCollection_ReturnsEqualsCollection()
        {
            var expected = GetTaskCollection();
            TaskCollectionFactory.SetCollection(expected);
            var factory = new TaskCollectionFactory();

            var actual = factory.GetCollection();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCollection_NullCollection_ThrownsConfigurationException()
        {
            TaskCollectionFactory.SetCollection(null);
            var factory = new TaskCollectionFactory();

            var exc = Assert.Catch<ConfigurationException>(() => factory.GetCollection());
            StringAssert.Contains("Не задана коллекция задач", exc.Message);
        }

        [TearDown]
        public void ResetTaskCollection()
        {
            TaskCollectionFactory.SetCollection(null);
        }
    }
}

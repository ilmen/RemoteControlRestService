using NSubstitute;
using NUnit.Framework;
using RemoteControlRestService.Infrastracture.Tasks;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public void Equals_NullObjectEquals_ReturnsFalse()
        {
            var excepted = new Task() { Id = Arg.Any<Guid>() };
            object nullObject = null;

            Assert.IsFalse(excepted.Equals(nullObject));
        }

        [Test]
        public void Equals_NullTaskEquals_ReturnsFalse()
        {
            var excepted = new Task() { Id = Arg.Any<Guid>() };
            Task nullObject = null;

            Assert.IsFalse(excepted.Equals(nullObject));
        }

        [Test]
        public void Equals_EqualsTasks_ReturnsTrue()
        {
            var guid = new Guid("{F931D3B5-0E3D-4106-B61A-A4C9A7361A83}");
            var excepted = new Task() { Id = guid };
            var actual = new Task() { Id = guid };

            Assert.IsTrue(excepted.Equals(actual));
        }

        [Test]
        public void Equals_NotEqualsTasks_ReturnsFalse()
        {
            var excepted = new Task() { Id = new Guid("{9E5EB77F-8D37-445F-B0B1-17862B54768A}") };
            var actual = new Task() { Id = new Guid("{F931D3B5-0E3D-4106-B61A-A4C9A7361A83}") };

            Assert.IsFalse(excepted.Equals(actual));
        }

        [Test]
        public void GetHashCode_NotEqualsTasks_ReturnsNotEqualsHashCodes()
        {
            var excepted = new Task() { Id = new Guid("{9E5EB77F-8D37-445F-B0B1-17862B54768A}") };
            var actual = new Task() { Id = new Guid("{F931D3B5-0E3D-4106-B61A-A4C9A7361A83}") };

            Assert.AreNotEqual(excepted.GetHashCode(), actual.GetHashCode());
        }

        [Test]
        public void GetHashCode_EqualsTasks_ReturnsEqualsHashCodes()
        {
            var guid = new Guid("{9E5EB77F-8D37-445F-B0B1-17862B54768A}");
            var excepted = new Task() { Id = guid };
            var actual = new Task() { Id = guid };

            Assert.AreEqual(excepted.GetHashCode(), actual.GetHashCode());
        }

        [Test]
        public void GetHashCode_DefaulTask_NotThrowns()
        {
            var defaultTask = new Task();
    
            defaultTask.GetHashCode();
        }
    }
}

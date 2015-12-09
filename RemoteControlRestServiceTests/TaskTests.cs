using NSubstitute;
using NUnit.Framework;
using RemoteControlRestService.Infrastracture.Validation;
using RemoteControlRestService.Models;
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

        [Test]
        public void IsValid_Always_UseDescriptionValidator()
        {
            const string DESCRIPTION = "some_description";
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                Description = DESCRIPTION
            };
            var invalidResult = ValidResult.GetInvalidResult("some description validation error");
            var mock = Substitute.For<IValidator<string>>();
            mock.Validate(DESCRIPTION).Returns(invalidResult);
            task.DescriptionValidator = mock;

            var result = task.IsValid();

            mock.ReceivedWithAnyArgs().Validate(DESCRIPTION);
            Assert.AreEqual(invalidResult, result);
        }

        [Test]
        public void IsValid_Always_UseFilePathValidator()
        {
            const string FILEPATH = "somefilename.type";
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                ScriptFilePath = FILEPATH
            };
            var invalidResult = ValidResult.GetInvalidResult("some file path validation error");
            var mock = Substitute.For<IValidator<string>>();
            mock.Validate(FILEPATH).Returns(invalidResult);
            task.FilePathValidator = mock;

            var result = task.IsValid();

            mock.ReceivedWithAnyArgs().Validate(FILEPATH);
            Assert.AreEqual(invalidResult, result);
        }

        [Test]
        public void IsValid_WrongId_ReturnsInvalidResult()
        {
            var task = new Task();

            var result = task.IsValid();

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains("Не задан идентификатор", result.ErrorMessage);
        }

        [Test]
        public void IsValid_WrongTimes_ReturnsInvalidResult()
        {
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                CreateTime = DateTime.MaxValue,
                RunTime = DateTime.MaxValue.Subtract(TimeSpan.FromSeconds(1))
            };

            var result = task.IsValid();

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains("Время запуска задачи не может быть меньше времени создания", result.ErrorMessage);
        }

        [Test]
        public void IsValid_AllValid_ReturnsValidResult()
        {
            const string DESCRIPTION = "some desc";
            const string FILEPARH = "some path";
            var ONE_TIME = new DateTime(2015, 12, 9);
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                CreateTime = ONE_TIME,
                RunTime = ONE_TIME,
                ScriptFilePath = FILEPARH,
                Description = DESCRIPTION
            };
            var stub = Substitute.For<IValidator<string>>();
            stub.Validate("any string").ReturnsForAnyArgs(ValidResult.Valid);
            task.DescriptionValidator = stub;
            task.FilePathValidator = stub;

            var result = task.IsValid();

            Assert.AreEqual(ValidResult.Valid, result);
        }
    }
}

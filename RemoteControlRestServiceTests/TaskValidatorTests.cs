using NSubstitute;
using NUnit.Framework;
using RemoteControlRestService.Classes;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class TaskValidatorTests
    {
        [Test]
        public void Validate_WrongId_ReturnsInvalidResult()
        {
            var stub = ValidateTestHelper.GetFakeCommandCollection();
            var validator = new TaskValidator(stub);
            var task = new Task() { Id = Guid.Empty };

            var result = validator.Validate(task);

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains("Не задан идентификатор", result.ErrorMessage);
        }

        [Test]
        public void Validate_WrongTimes_ReturnsInvalidResult()
        {
            var stub = ValidateTestHelper.GetFakeCommandCollection();
            var validator = new TaskValidator(stub);
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                CreateTime = DateTime.MaxValue,
                RunTime = DateTime.MaxValue.Subtract(TimeSpan.FromSeconds(1))
            };

            var result = validator.Validate(task);

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains("Время запуска задачи не может быть меньше времени создания", result.ErrorMessage);
        }

        [Test]
        public void Validate_WrongCmd_ReturnsInvalidResult()
        {
            var COMMAND = "some_command";
            var OTHER_COMMAND = "other_command";
            var mock = ValidateTestHelper.GetFakeCommandCollection(new string[] { COMMAND });
            var validator = new TaskValidator(mock);
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                CommandType = OTHER_COMMAND
            };

            var result = validator.Validate(task);

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains("Некорректное значение поля CommandType", result.ErrorMessage);
        }

        [Test]
        public void Validate_AllValid_ReturnsValidResult()
        {
            var SOME_COMMAND = "some_command";
            var mock = ValidateTestHelper.GetFakeCommandCollection(new string[] { SOME_COMMAND });
            var validator = new TaskValidator(mock);
            var ONE_TIME = new DateTime(2015, 12, 9);
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                CreateTime = ONE_TIME,
                RunTime = ONE_TIME,
                CommandType = SOME_COMMAND
            };

            var result = validator.Validate(task);

            Assert.AreEqual(ValidResult.Valid, result);
        }
    }
}

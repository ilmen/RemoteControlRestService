using NUnit.Framework;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using RemoteControlRestService.Infrastracture.Tasks;
using RemoteControlRestService.Infrastracture.Commands;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class TaskValidatorTests
    {
        IEnumerable<Command> GetStubCommandCollection()
        {
            return new Command[0];
        }

        [Test]
        public void Validate_WrongId_ReturnsInvalidResult()
        {
            var stubCollection = GetStubCommandCollection();
            var validator = new TaskValidator(stubCollection);
            var task = new Task() { Id = Guid.Empty };

            var result = validator.Validate(task);

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains("Не задан идентификатор", result.ErrorMessage);
        }

        [Test]
        public void Validate_WrongTimes_ReturnsInvalidResult()
        {
            var stubCollection = GetStubCommandCollection();
            var validator = new TaskValidator(stubCollection);
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
            var stubCollection = GetStubCommandCollection();
            var validator = new TaskValidator(stubCollection);
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                Cmd = null
            };

            var result = validator.Validate(task);

            Assert.IsFalse(result.IsValid);
            StringAssert.Contains("Поле Cmd не задана", result.ErrorMessage);
        }

        [Test]
        public void Validate_AllValid_ReturnsValidResult()
        {
            var cmd = new Command() { Id = 1, FilePath = "SOMEFILE.bat", Name = "SOME_NAME" };
            var validator = new TaskValidator(new List<Command>() { cmd });
            var ONE_TIME = new DateTime(2015, 12, 9);
            var task = new Task()
            {
                Id = new Guid("{15C97E19-48A9-451F-8F66-549505B41268}"),
                CreateTime = ONE_TIME,
                RunTime = ONE_TIME,
                Cmd = cmd
            };

            var result = validator.Validate(task);

            Assert.AreEqual(ValidResult.Valid, result);
        }
    }
}

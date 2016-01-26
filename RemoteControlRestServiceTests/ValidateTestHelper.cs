using System;
using System.Collections.Generic;
using NSubstitute;
using RemoteControlRestService.Classes;
using RemoteControlRestService.Infrastracture.Validation;

namespace RemoteControlRestServiceTests
{
    public class ValidateTestHelper
    {
        public static IValidator<string> GetFakeStringValidator(ValidResult expected)
        {
            var stub = Substitute.For<IValidator<string>>();
            stub.Validate("some string").ReturnsForAnyArgs(expected);

            return stub;
        }

        public static IValidator<Task> GetFakeTaskValidator(ValidResult expected)
        {
            const Task SOME_TASK = null;
            var stub = Substitute.For<IValidator<Task>>();
            stub.Validate(SOME_TASK).ReturnsForAnyArgs(expected);

            return stub;
        }

        //public static IValidator<Command> GetFakeCommandValidator(ValidResult expected)
        //{
        //    const Command SOME_CMD = null;
        //    var stub = Substitute.For<IValidator<Command>>();
        //    stub.Validate(SOME_CMD).ReturnsForAnyArgs(expected);

        //    return stub;
        //}

        public static IFactory<IEnumerable<string>> GetFakeCommandCollection(IEnumerable<string> values)
        {
            // TODO: все таки сделать это через NSubstitute
            return new FakeCommandCollection(values);
        }

        public static IFactory<IEnumerable<string>> GetFakeCommandCollection() => GetFakeCommandCollection(new string[0]);

        public class FakeCommandCollection : IFactory<IEnumerable<string>>
        {
            private IEnumerable<string> _Collection;

            public FakeCommandCollection(IEnumerable<string> collection)
            {
                this._Collection = collection;
            }

            public IEnumerable<string> Create()
            {
                return _Collection;
            }
        }
    }
}

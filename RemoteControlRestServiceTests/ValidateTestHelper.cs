using NSubstitute;
using RemoteControlRestService.Infrastracture.Commands;
using RemoteControlRestService.Infrastracture.Tasks;
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

        public static IValidator<Command> GetFakeCommandValidator(ValidResult expected)
        {
            const Command SOME_CMD = null;
            var stub = Substitute.For<IValidator<Command>>();
            stub.Validate(SOME_CMD).ReturnsForAnyArgs(expected);

            return stub;
        }
    }
}

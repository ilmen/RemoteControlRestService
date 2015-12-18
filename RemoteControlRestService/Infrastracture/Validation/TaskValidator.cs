using RemoteControlRestService.Infrastracture.Commands;
using RemoteControlRestService.Infrastracture.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class TaskValidator : IValidator<Task>
    {
        IEnumerable<Command> CommandCollection;

        public TaskValidator(IEnumerable<Command> commandCollection)
        {
            CommandCollection = commandCollection;
        }

        public ValidResult Validate(Task value)
        {
            if (value == null) throw new ArgumentNullException("Получена неинициализированная задача!");

            if (value.Id == Guid.Empty) return ValidResult.GetInvalidResult("Не задан идентификатор задачи!");

            if (value.CreateTime > value.RunTime) return ValidResult.GetInvalidResult("Время запуска задачи не может быть меньше времени создания задачи!");

            if (value.Cmd == null) return ValidResult.GetInvalidResult("Поле Cmd не задана!");

            var cmd = CommandCollection.SingleOrDefault(x => x.Id == value.Cmd.Id);
            if (cmd == null) throw new ArgumentException("Команда не найдена!");

            return ValidResult.Valid;
        }
    }
}

using RemoteControlRestService.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class TaskValidator : IValidator<Task>
    {
        IEnumerable<string> CommandCollection;

        public TaskValidator()
        {
            var factory = new CommandCollectionFactory();
            CommandCollection = factory.GetCollection();
        }

        public ValidResult Validate(Task value)
        {
            if (value == null) throw new ArgumentNullException("Получена неинициализированная задача!");

            if (value.Id == Guid.Empty) return ValidResult.GetInvalidResult("Не задан идентификатор задачи!");

            if (value.CreateTime > value.RunTime) return ValidResult.GetInvalidResult("Время запуска задачи не может быть меньше времени создания задачи!");

            if (!CommandCollection.Contains(value.CommandType)) return ValidResult.GetInvalidResult("Некорректное значение поля CmdType!");

            return ValidResult.Valid;
        }
    }
}

using RemoteControlRestService.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class TaskValidator : IValidator<Task>
    {
        IEnumerable<string> CommandCollection;

        public TaskValidator() : this(new CommandCollectionFactory()) { }

        public TaskValidator(IFactory<IEnumerable<string>> commandFactory)
        {
            CommandCollection = commandFactory.Create();
        }

        public ValidResult Validate(Task value)
        {
            if (value == null) throw new ArgumentNullException("Получена неинициализированная задача!");

            if (value.Id == Guid.Empty) return ValidResult.GetInvalidResult("Не задан идентификатор задачи!");

            if (value.CreateTime > value.RunTime) return ValidResult.GetInvalidResult("Время запуска задачи не может быть меньше времени создания задачи!");

            if (!CommandCollection.Contains(value.CommandType)) return ValidResult.GetInvalidResult("Некорректное значение поля CommandType!");

            return ValidResult.Valid;
        }
    }
}

using RemoteControlRestService.Infrastracture.Commands;
using RemoteControlRestService.Infrastracture.Tasks;
using System;
using System.Collections.Generic;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class TaskValidator : IValidator<Task>
    {
        public ValidResult Validate(Task value)
        {
            if (value.Id == Guid.Empty) return ValidResult.GetInvalidResult("Не задан идентификатор задачи!");

            if (value.CreateTime > value.RunTime) return ValidResult.GetInvalidResult("Время запуска задачи не может быть меньше времени создания задачи!");

            if (value.Cmd == null) return ValidResult.GetInvalidResult("Поле Cmd не задана!");

            return ValidResult.Valid;
        }
    }
}

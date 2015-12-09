using RemoteControlRestService.Infrastracture.Validation;
using RemoteControlRestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Infrastracture
{
    public class TaskToRunProvider
    {
        IEnumerable<Task> TaskCollection;

        public TaskToRunProvider(IEnumerable<Task> taskCollection)
        {
            this.TaskCollection = taskCollection;
        }

        public IEnumerable<Task> GetTasks()
        {
            var now = SystemTime.Now;

            return TaskCollection
                .Where(x => x.IsValid() == ValidResult.Valid)   // TODO: убрать проверку, так как невалидные задачи не должны появляться в коллекции! Они должны валидироваться при создании и изменении, а не при использовании
                .Where(x => x.RunTime <= now)
                .ToList();
        }
    }
}

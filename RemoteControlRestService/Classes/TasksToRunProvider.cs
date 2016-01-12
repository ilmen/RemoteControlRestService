using RemoteControlRestService.Infrastracture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestService.Classes
{
    public interface ITasksToRunProvider
    {
        IEnumerable<Task> GetTasksToRun();
    }

    public class TasksToRunProvider : ITasksToRunProvider
    {
        IEnumerable<Task> TaskCollection;

        public TasksToRunProvider(IEnumerable<Task> taskCollection)
        {
            this.TaskCollection = taskCollection;
        }

        public IEnumerable<Task> GetTasksToRun()
        {
            Func<Task, bool> isReadyToRun = (t) => t.RunTime >= SystemTime.Now;

            Func<Task, bool> isNew = (t) => t.RunnableTask == null;
            Func<Task, bool> isAdded = (t) => t.RunnableTask?.Status == enRunnableTaskStatus.Added;
            Func<Task, bool> isNeedRestart = (t) => t.RunnableTask?.Status == enRunnableTaskStatus.Error;

            return TaskCollection
                .Where(t => isReadyToRun(t))
                .Where(t => isNew(t) || isAdded(t) || isNeedRestart(t))
                .ToList();
        }
    }
}

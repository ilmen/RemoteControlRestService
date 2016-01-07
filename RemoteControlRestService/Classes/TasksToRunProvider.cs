using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var isNew = new Func<Task, bool>((t) => t.RunnableTask == null);
            var isAdded = new Func<Task, bool>((t) => t.RunnableTask.Status == enRunnableTaskStatus.Added);
            var isNeedRestart = new Func<Task, bool>((t) => t.RunnableTask.Status == enRunnableTaskStatus.Error);

            return TaskCollection.Where(t => isNew(t) || isAdded(t) || isNeedRestart(t)).ToList();
        }
    }
}

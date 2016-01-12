using RemoteControlRestService.Classes.RunnableTasks;

namespace RemoteControlRestService.Classes
{
    public class TaskRunner
    {
        ITasksToRunProvider TaskProvider;

        RunnableTaskFactory RTaskFactory;

        object startedNowSync = new object();
        bool startedNow = false;

        public TaskRunner(ITasksToRunProvider taskProvider)
        {
            this.TaskProvider = taskProvider;

            this.RTaskFactory = new RunnableTaskFactory();
        }

        public void TryStartNewTask()
        {
            lock (startedNowSync)
            {
                if (startedNow)
                {
                    return;
                }
                else
                {
                    startedNow = true;
                }
            }

            StartNewTask();

            lock (startedNowSync)
            {
                startedNow = false;
            }
        }

        void StartNewTask()
        {
            var newTasks = TaskProvider.GetTasksToRun();

            foreach (var task in newTasks)
            {
                if (task.RunnableTask == null)
                {
                    var rTask = RTaskFactory.Create(task.CommandType);
                    task.RunnableTask = rTask;
                }

                task.RunnableTask.Run();
            }
        }
    }
}

using RemoteControlRestService.Classes;
using RemoteControlRestService.Infrastracture;
using System;
using System.Collections.Generic;

namespace RemoteControlRestService
{
    public class StartUp
    {
        static ServiceSettings settings;

        public ServiceSettings GetServiceSettings()
        {
            return settings;
        }

        public void Configure()
        {
            // getting service settings
            var provider = new ServiceSettingsProvider();
            settings = provider.GetSettings();

            // load commands collection
            CommandCollectionFactory.LoadCollection();

            // setup task collection
            var tasks = GetDefaultTaskCollection();
            TaskCollectionFactory.SetCollection(tasks);

            // configure task runner
            var tasksToRunProvider = new TasksToRunProvider(tasks);
            var worker = new TaskRunner(tasksToRunProvider);
            var timerInterval = TimeSpan.FromSeconds(settings.FindNewTaskTimerInteval).TotalMilliseconds;
            var timer = new System.Timers.Timer(timerInterval);
            timer.Elapsed += (s, e) => worker.TryStartNewTasks();
            timer.Start();
        }


        IList<Task> GetDefaultTaskCollection()
        {
            return new List<Task>();

            //var cmdType = "testcommand";
            //var factory = new RunnableTaskFactory();
            //var command = factory.Create(cmdType);

            //return new List<Task>()
            //    {
            //        new RemoteControlRestService.Classes.Task()
            //        {
            //            Id = new Guid("{D713368A-73D0-4054-82FD-BA6F95586FE9}"),
            //            CreateTime = DateTime.MinValue,
            //            RunTime = DateTime.MinValue,
            //            CommandType = cmdType,
            //            RunnableTask = command,
            //        }
            //    };
        }
    }
}

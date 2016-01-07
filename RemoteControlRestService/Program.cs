using Microsoft.Owin.Hosting;
using RemoteControlRestService.Classes;
using RemoteControlRestService.Classes.RunnableTasks;
using RemoteControlRestService.Infrastracture;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RemoteControlRestService
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new CommandLineSettingProvider();
            var settings = provider.GetSettings(args);
            var baseAddress = String.Format("http://localhost:{0}/", settings.Port);

            var tasks = GetDefaultTaskCollection();
            TaskCollectionFactory.SetCollection(tasks);

            var tasksToRunProvider = new TasksToRunProvider(tasks);
            var worker = new TaskRunner(tasksToRunProvider);
            var timer = new System.Timers.Timer(30000);
            timer.Elapsed += (s, e) => worker.TryStartNewTask();
            timer.Start();

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Server started on <" + baseAddress + ">");

                if (System.Diagnostics.Debugger.IsAttached) TestRestService(baseAddress + "api/tasks");

                Console.WriteLine("Press <ENTER> to exit");
                Console.ReadLine();
                Console.WriteLine("Exiting...");
            }
        }

        static IList<Task> GetDefaultTaskCollection()
        {
            return new List<Task>();

            //var cmdType = "testcommand";
            //var factory = new RunnableTaskFactory();
            //var command = factory.Create(cmdType);

            //return new List<Task>()
            //    {
            //        new Task()
            //        {
            //            Id = new Guid("{D713368A-73D0-4054-82FD-BA6F95586FE9}"),
            //            CreateTime = DateTime.MinValue,
            //            RunTime = DateTime.MinValue,
            //            CommandType = cmdType,
            //            RunnableTask = command,
            //        }
            //    };
        }

        private static void TestRestService(string url)
        {
            Console.WriteLine("Test request:");

            var client = new HttpClient();

            var response = client.GetAsync(url).Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}

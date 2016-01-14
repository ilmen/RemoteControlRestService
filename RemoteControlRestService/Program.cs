using Microsoft.Owin.Hosting;
using RemoteControlRestService.Classes;
using RemoteControlRestService.Infrastracture;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RemoteControlRestService
{
    class Program
    {
        static volatile int hostCounter = 0;

        static void Main(string[] args)
        {
            // getting service settings
            var provider = new ServiceSettingsProvider();
            var settings = provider.GetSettings();

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
            
            // runnig hosts on all interfaces
            var cts = new CancellationTokenSource();
            foreach (var endpoint in settings.Endpoints)
            {
                var baseAddress = $"http://{endpoint}/";
                System.Threading.Tasks.Task.Run(() => StartOWINServer(baseAddress, cts.Token), cts.Token);
            }

            Console.WriteLine("Press <ENTER> to exit");
            Console.ReadLine();

            // stopping all hosts
            cts.Cancel();
            while (hostCounter > 0)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("Exiting...");

            #region Simple starting OWIN host
            //// Start OWIN host 
            //using (WebApp.Start<Startup>(url: baseAddress))
            //{
            //    Console.WriteLine("Server started on <" + baseAddress + ">");

            //    if (System.Diagnostics.Debugger.IsAttached) TestRestService(baseAddress + "api/tasks");

            //    Console.WriteLine("Press <ENTER> to exit");
            //    Console.ReadLine();
            //    Console.WriteLine("Exiting...");
            //} 
            #endregion
        }

        static IList<Task> GetDefaultTaskCollection()
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

        static void StartOWINServer(string baseUrl, CancellationToken ct)
        {
            using (WebApp.Start<Startup>(url: baseUrl))
            {
                Interlocked.Increment(ref hostCounter);

                Console.WriteLine("Server started on <" + baseUrl + ">");

                //if (System.Diagnostics.Debugger.IsAttached) TestRestService(baseAddress + "api/tasks");

                ct.WaitHandle.WaitOne();

                Interlocked.Decrement(ref hostCounter);
            };
        }

        //private static void TestRestService(string url)
        //{
        //    Console.WriteLine("Test request:");

        //    var client = new HttpClient();

        //    var response = client.GetAsync(url).Result;

        //    Console.WriteLine(response);
        //    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        //}
    }
}

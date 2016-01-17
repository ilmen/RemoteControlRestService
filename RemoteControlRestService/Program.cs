using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteControlRestService
{
    class Program
    {
        static volatile int hostCounter = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");
            
            var startUp = new StartUp();
            startUp.Configure();
            
            // load endpoints
            var endpoints = startUp.GetServiceSettings()?.Endpoints ?? new string[0];
            Console.WriteLine($"Loaded {endpoints.Length} endpoints.");

            // runnig hosts on all interfaces
            var cts = new CancellationTokenSource();
            foreach (var endpoint in endpoints)
            {
                var baseAddress = $"http://{endpoint}/";
                Task.Run(() => StartOWINServer(baseAddress, cts.Token), cts.Token);
            }

            Console.WriteLine("Press <ENTER> to exit.");
            Console.ReadLine();

            // stopping all hosts
            cts.Cancel();
            while (hostCounter > 0)
            {
                Thread.Sleep(100);
            }

            Console.WriteLine("Exiting...");
        }

        static void TestRestService(string url)
        {
            Console.WriteLine("Test request:");

            var client = new HttpClient();

            var response = client.GetAsync(url).Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        static void StartOWINServer(string baseUrl, CancellationToken ct)
        {
            using (WebApp.Start<OwinStartup>(url: baseUrl))
            {
                Interlocked.Increment(ref hostCounter);

                Console.WriteLine("Server started on <" + baseUrl + "> endpoint.");

                if (System.Diagnostics.Debugger.IsAttached) TestRestService(baseUrl + "api/tasks");
                
                ct.WaitHandle.WaitOne();

                Interlocked.Decrement(ref hostCounter);
            };
        }
    }
}

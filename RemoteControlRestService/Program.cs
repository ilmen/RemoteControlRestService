using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
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

        static void StartOWINServer(string baseUrl, CancellationToken ct)
        {
            using (WebApp.Start<OwinStartup>(url: baseUrl))
            {
                Interlocked.Increment(ref hostCounter);

                Console.WriteLine("Server started on <" + baseUrl + "> endpoint.");

                var useTesting = !System.Diagnostics.Debugger.IsAttached;
                if (useTesting)
                {
                    TestRestService(baseUrl + "api/tasks", HttpMethod.Get);

                    foreach (var item in System.Linq.Enumerable.Range(0, 2))
                    {
                        //var json = "{\"id\":\"" + Guid.NewGuid() + "\"}";
                        var task = new RemoteControlRestService.Classes.Task() { Id = Guid.NewGuid() };
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(task);
                        TestRestService(baseUrl + "api/tasks", HttpMethod.Post, json);
                    }

                    TestRestService(baseUrl + "api/tasks", HttpMethod.Get);
                }
                
                ct.WaitHandle.WaitOne();

                Interlocked.Decrement(ref hostCounter);
            };
        }

        static void TestRestService(string url, HttpMethod method, string postData = null)
        {
            Console.WriteLine("Test request: " + method.Method + " <" + url + ">");

            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                if (method == HttpMethod.Post)
                {
                    var postDataKV = new KeyValuePair<string, string>("value", postData);
                    var content = new FormUrlEncodedContent(new[] { postDataKV });

                    response = client.PostAsync(url, content).Result;
                }
                else
                {
                    response = client.GetAsync(url).Result;
                }

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}

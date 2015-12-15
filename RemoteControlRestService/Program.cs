using Microsoft.Owin.Hosting;
using RemoteControlRestService.Infrastracture;
using RemoteControlRestService.Infrastracture.Tasks;
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

            var tasks = new List<Task>();
            TaskCollectionFactory.SetCollection(tasks);


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

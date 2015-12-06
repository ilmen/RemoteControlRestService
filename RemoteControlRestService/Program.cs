using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace RemoteControlRestService
{
    class Program
    {
        static void Main(string[] args)
        {
            var portProvider = new PortProvider();
            var port = portProvider.GetPort(args);
            var baseAddress = String.Format("http://localhost:{0}/", port);

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Server started on <" + baseAddress + ">");

                // Create HttpCient and make a request to api/values 
                var client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/tasks").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            Console.ReadLine(); 
        }
    }
}

using Owin;
using System.Web.Http;

namespace RemoteControlRestService
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // настраиваем JSON сериализацию
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.Indent = true;

            appBuilder.UseWebApi(config);
        } 
    }
}

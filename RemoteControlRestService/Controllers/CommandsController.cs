using System.Collections.Generic;
using System.Linq;
using RemoteControlRestService.Classes;
using Nancy;

namespace RemoteControlRestService.Controllers
{
    public class CommandsController : NancyModule
    {
        readonly IEnumerable<string> _CommandCollection;

        public CommandsController() : this(new CommandCollectionFactory()) { }

        public CommandsController(IFactory<IEnumerable<string>> commandFactory)
        {
            _CommandCollection = commandFactory.Create();

            Get["/api/commands"] = p => Response.AsJson(_CommandCollection);

            Get["/api/commands/{Id}"] = p => GetOne(p.Id);
        }

        public Response GetOne(int id)
        {
            if (id >= _CommandCollection.Count()) return HttpStatusCode.NotFound;

            return Response.AsJson(_CommandCollection.Skip(id).FirstOrDefault());
        }
    }
}

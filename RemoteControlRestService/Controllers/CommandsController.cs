using RemoteControlRestService.Infrastracture.Commands;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RemoteControlRestService.Controllers
{
    public class CommandsController : ApiController
    {
        readonly IEnumerable<Command> CommandCollection;

        public CommandsController()
        {
            var provider = new CommandCollectionFactory();
            CommandCollection = provider.GetCollection();
        }

        public IEnumerable<Command> Get()
        {
            return CommandCollection;
        }

        public Command Get(int id)
        {
            // TODO: если элемент с таким Id не существует - возвращать ошибку 404
            return CommandCollection.FirstOrDefault(x => x.Id == id);
        }

        public HttpResponseMessage Post([FromBody]Command value)
        {
            return GetReadOnlyCollectionError();
        }

        public HttpResponseMessage Put(int id, [FromBody]Command value)
        {
            return GetReadOnlyCollectionError();
        }

        public HttpResponseMessage Delete(int id)
        {
            return GetReadOnlyCollectionError();
        }

        HttpResponseMessage GetReadOnlyCollectionError()
        {
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Коллекция доступна только для чтения!");
        }
    }
}

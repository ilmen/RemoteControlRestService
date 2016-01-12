using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RemoteControlRestService.Classes;

namespace RemoteControlRestService.Controllers
{
    public class CommandsController : ApiController
    {
        readonly IEnumerable<string> CommandCollection;

        public CommandsController()
        {
            var factory = new CommandCollectionFactory();
            CommandCollection = factory.GetCollection();
        }

        public IEnumerable<string> Get()
        {
            return CommandCollection;
        }

        public string Get(int id)
        {
            // TODO: если элемент с таким Id не существует - возвращать ошибку 404
            return CommandCollection.Skip(id).FirstOrDefault();
        }

        public HttpResponseMessage Post([FromBody]string value)
        {
            return GetReadOnlyCollectionError();
        }

        public HttpResponseMessage Put(int id, [FromBody]string value)
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

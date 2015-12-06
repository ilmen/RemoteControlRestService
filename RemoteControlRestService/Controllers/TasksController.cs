using RemoteControlRestService.Infrastracture;
using RemoteControlRestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RemoteControlRestService.Controllers
{
    public class TasksController : ApiController
    {
        readonly IList<Task> TaskCollection;

        public TasksController()
        {
            var provider = new TaskCollectionFactory();
            TaskCollection = provider.GetCollection();
        }

        public IEnumerable<Task> Get()
        {
            return TaskCollection;
        }

        public Task Get(Guid id)
        {
            return TaskCollection.FirstOrDefault(x => x.Id == id);
        }

        public void Post([FromBody]Task value)
        {
            if (value == null) throw new ArgumentNullException();
            if ()

            TaskCollection.Add(value);
        }

        public void Put(Guid id, [FromBody]Task value)
        {
            if (value.Id != id) throw new ArgumentException("Входные параметры Id и value.Id не совпадают!");

            Remove(id);
            TaskCollection.Add(value);
        }

        public void Delete(Guid id)
        {
            Remove(id);
        }

        void Remove(Guid id)
        {
            var toRemove = Get(id);
            TaskCollection.Remove(toRemove);
        }
    }
}

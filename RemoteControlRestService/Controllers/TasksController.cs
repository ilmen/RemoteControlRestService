using RemoteControlRestService.Infrastracture;
using RemoteControlRestService.Infrastracture.Tasks;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RemoteControlRestService.Controllers
{
    public class TasksController : ApiController
    {
        readonly IList<Task> TaskCollection;
        readonly IValidator<Task> Validator;

        public TasksController() : this(new TaskValidator()) { }

        public TasksController(IValidator<Task> validator)
        {
            Validator = validator;

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
            ValidateValue(value);
            
            TaskCollection.Add(value);
        }

        public void Put(Guid id, [FromBody]Task value)
        {
            ValidateValue(value);
            if (value.Id != id) throw new ArgumentException("Входные параметры Id и value.Id не совпадают!");

            var toRemove = TaskCollection.Single(x => x.Id == id);
            TaskCollection.Remove(toRemove);
            TaskCollection.Add(value);
        }

        public HttpResponseMessage Delete(Guid id)
        {
            var toRemove = TaskCollection.FirstOrDefault(x => x.Id == id);
            TaskCollection.Remove(toRemove);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        void ValidateValue(Task value)
        {
            if (value == null) throw new ArgumentNullException("Получена неинициализированная задача!");
            Validator.Validate(value).ThrowExceptionIfNotValid();
        }
    }
}

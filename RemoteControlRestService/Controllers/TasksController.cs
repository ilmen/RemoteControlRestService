using Nancy;
using RemoteControlRestService.Classes;
using System;

namespace RemoteControlRestService.Controllers
{
    public class TasksController : NancyModule
    {
        const string SUPPORTED_FORMAT = "application/json";

        ICRUDCollection<Guid, Task> _TaskCollection;

        public TasksController() : this(new TaskCollection()) { }

        public TasksController(ICRUDCollection<Guid, Task> taskController)
        {
            _TaskCollection = taskController;

            Get["/api/tasks"] = p => Response.AsJson(_TaskCollection.GetAll());

            Get["/api/tasks/{Id}"] = p => Response.AsJson(_TaskCollection.GetOne((Guid)p.Id));

            Post["/api/tasks"] = p =>
            {
                var task = GetTaskFromRequestBody();
                _TaskCollection.Insert(task);
                return HttpStatusCode.Created;
            };

            Put["/api/tasks/{Id}"] = p =>
            {
                var task = GetTaskFromRequestBody();
                _TaskCollection.Update(p.Id, task);
                return HttpStatusCode.Accepted;
            };

            Delete["/api/tasks/{Id}"] = p =>
            {
                _TaskCollection.Delete(p.Id);
                return HttpStatusCode.OK;
            };
        }

        public Task GetTaskFromRequestBody()
        {
            var format = Request.Headers.ContentType;
            if (!format.StartsWith(SUPPORTED_FORMAT)) throw new Exception($"Формат \"{format}\" не поддерживаеться! Используйте \"{SUPPORTED_FORMAT}\"!");

            var reader = new System.IO.StreamReader(Request.Body);
            var body = reader.ReadToEnd();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Task>(body);
        }
    }
}

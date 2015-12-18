using RemoteControlRestService.Infrastracture.Commands;
using RemoteControlRestService.Infrastracture.Tasks;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RemoteControlRestService.Controllers
{
    public class TasksController : ApiController
    {
        IList<Task> TaskCollection;
        IEnumerable<Command> CommandCollection;
        IValidator<Task> Validator;

        public TasksController()
        {
            Create();

            Validator = new TaskValidator(CommandCollection);
        }

        public TasksController(IValidator<Task> validator)
            : this()
        {
            Validator = validator;
        }
        
        void Create()
        {
            var taskProvider = new TaskCollectionFactory();
            TaskCollection = taskProvider.GetCollection();

            var commandProvider = new CommandCollectionFactory();
            CommandCollection = commandProvider.GetCollection();
        }

        public IEnumerable<Task> Get()
        {
            return TaskCollection;
        }

        public Task Get(Guid id)
        {
            // TODO: если элемент с таким Id не существует - возвращать ошибку 404
            return TaskCollection.FirstOrDefault(x => x.Id == id);
        }

        public void Post([FromBody]Task value)
        {
            ValidateValue(value);
            ReplaceCommand(value);
            
            TaskCollection.Add(value);
        }

        public void Put(Guid id, [FromBody]Task value)
        {
            ValidateValue(value);
            if (value.Id != id) throw new ArgumentException("Входные параметры Id и value.Id не совпадают!");
            ReplaceCommand(value);

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
            // TODO: неравество задачи Null - это ограничение алгоритмов TasksController. Остальные проверки - ответвенность валидатора, ответственность которого - проверять логическую корректность задачи
            if (value == null) throw new ArgumentNullException("Команда не может быть равна Null!");
            Validator.Validate(value).ThrowExceptionIfNotValid();
        }

        void ReplaceCommand(Task value)
        {
            value.Cmd = CommandCollection.Single(x => x.Id == value.Cmd.Id);
        }
    }
}

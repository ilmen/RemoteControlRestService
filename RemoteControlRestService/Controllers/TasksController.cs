using RemoteControlRestService.Classes;
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
        static IList<Task> TaskCollection;
        IEnumerable<string> CommandCollection;
        IValidator<Task> Validator;

        public TasksController() : this(new TaskValidator()) { }

        public TasksController(IValidator<Task> validator)
        {
            Validator = validator;

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
            if (TaskCollection.Select(x => x.Id).Contains(value.Id)) throw new ArgumentException("Нарушение уникальности идентификатора задачи!");

            TaskCollection.Add(value);
        }

        public void Put(Guid id, [FromBody]Task value)
        {
            ValidateValue(value);
            if (value.Id != id) throw new ArgumentException("Входные параметры Id и value.Id не совпадают!");

            // TODO: поменять Single на SingleOrDefault и сделать возврат ошибки, если элемент с таким id не найден
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
            // TODO: неравество задачи Null - это ограничение алгоритмов TasksController. Остальные проверки - ответвенность валидатора - проверять логическую корректность задачи
            if (value == null) throw new ArgumentNullException("Команда не может быть равна Null!");
            Validator.Validate(value).ThrowExceptionIfNotValid();
            if (value.RunnableTask != null) throw new ArgumentException("Конкретный реализация задачи формируется только переред первым вызовом!");
        }
    }
}

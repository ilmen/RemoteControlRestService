using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestService.Classes
{
    public class TaskCollection : ICRUDCollection<Guid, Task>
    {
        IList<Task> _TaskCollection;
        IEnumerable<string> _CommandCollection;
        IValidator<Task> _Validator;

        public TaskCollection() : this(new TaskValidator(), new TasksProvider(), new CommandCollectionFactory()) { }

        public TaskCollection(IValidator<Task> validator, IFactory<IList<Task>> taskFactory, IFactory<IEnumerable<string>> commandFactory)
        {
            _Validator = validator;
            _TaskCollection = taskFactory.Create();
            _CommandCollection = commandFactory.Create();
        }

        public IEnumerable<Task> GetAll() => _TaskCollection;

        public Task GetOne(Guid id) => _TaskCollection.FirstOrDefault(x => x.Id == id);

        public void Insert(Task value)
        {
            ValidateValue(value);
            if (_TaskCollection.Select(x => x.Id).Contains(value.Id)) throw new ArgumentException("Задача с таким Id уже существует! Id задачи должно быть уникально.");

            _TaskCollection.Add(value);
        }

        public void Update(Guid id, Task value)
        {
            ValidateValue(value);
            if (value.Id != id) throw new ArgumentException("Не совпадающие значения Id в параметре и в переданной задаче!");

            var toRemove = _TaskCollection.SingleOrDefault(x => x.Id == id);
            if (toRemove == null) throw new ArgumentException("Задача с таким Id не найдена!");

            // TODO: реализовать taskCollection как наследника от ReaderWriterLockSlim для межзапроссной синхронизации

            _TaskCollection.Remove(toRemove);

            _TaskCollection.Add(value);
        }

        public void Delete(Guid id)
        {
            var toRemove = _TaskCollection.FirstOrDefault(x => x.Id == id);
            if (toRemove == null) throw new ArgumentException("Задача с таким Id не найдена!");

            _TaskCollection.Remove(toRemove);
        }

        void ValidateValue(Task value)
        {
            // TODO: неравество задачи Null - это ограничение алгоритмов TasksController. Остальные проверки - ответвенность валидатора - проверять логическую корректность задачи
            if (value == null) throw new ArgumentNullException("Команда не может быть равна Null!");
            _Validator.Validate(value).ThrowExceptionIfNotValid();
            if (value.RunnableTask != null) throw new ArgumentException("Конкретный реализация задачи формируется только переред первым вызовом!");
        }
    }
}

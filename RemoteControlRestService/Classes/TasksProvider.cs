using RemoteControlRestService.Infrastracture;
using System.Collections.Generic;

namespace RemoteControlRestService.Classes
{
    public class TasksProvider : IFactory<IList<Task>>
    {
        static IList<Task> TaskCollection;

        public static void SetCollection(IList<Task> collection)
        {
            TaskCollection = collection;
        }

        public IList<Task> Create()
        {
            if (TaskCollection == null) throw new ConfigurationException("Не задана коллекция задач! Необходимо вызвать метод \"TaskCollectionFactory.SetCollection(IList<Task> collection)\"");

            return TaskCollection;
        }
    }
}

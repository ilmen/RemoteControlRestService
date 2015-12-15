using System.Collections.Generic;

namespace RemoteControlRestService.Infrastracture.Tasks
{
    public class TaskCollectionFactory
    {
        static IList<Task> TaskCollection;

        public static void SetCollection(IList<Task> collection)
        {
            TaskCollection = collection;
        }

        public IList<Task> GetCollection()
        {
            if (TaskCollection == null) throw new ConfigurationException("Не задана коллекция задач! Необходимо вызвать метод \"TaskCollectionFactory.SetCollection(IList<Task> collection)\"");

            return TaskCollection;
        }
    }
}

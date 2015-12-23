using RemoteControlRestService.Infrastracture.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Infrastracture.Sheduler
{
    public class TasksWorker
    {
        SimleObservableCollection<Task> TaskCollection;
        List<RunnableTask> RunnableTaskCollection;

        public TasksWorker(SimleObservableCollection<Task> taskCollection)
        {
            this.TaskCollection = taskCollection;
            this.TaskCollection.CollectionChanged += TaskCollection_CollectionChanged;
        }

        void TaskCollection_CollectionChanged(object sender, CollectionChangedEventArgs<Task> e)
        {
            if (e.Action == enChanges.Added) AddTask(e.NewItem);
            if (e.Action == enChanges.Removed) RemoveTask(e.OldItem);
        }

        private void RemoveTask(Task task)
        {
            var taskToStart = new RunnableTask(task);
            RunnableTaskCollection.Add(taskToStart);
        }

        private void AddTask(Task task)
        {
            var taskToStop = RunnableTaskCollection.FirstOrDefault(x => x.Model == task);
            if (taskToStop != null) taskToStop.Stop();
            RunnableTaskCollection.Remove(taskToStop);
        }
    }
}
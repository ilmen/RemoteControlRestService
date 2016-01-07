using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Classes.RunnableTasks
{
    public class TestCommand : IRunnableTask
    {
        bool stopped = false;

        enRunnableTaskStatus status = enRunnableTaskStatus.Added;

        public enRunnableTaskStatus Status
        {
            get
            {
                return status;
            }
        }

        public void SetStatus(enRunnableTaskStatus newStatus)
        {
            status = newStatus;
            Console.WriteLine("TestCommand " + newStatus.ToString());
        }

        public void Run()
        {
            SetStatus(enRunnableTaskStatus.Running);
            Sleep();
            if (!stopped) SetStatus(enRunnableTaskStatus.Completed);
        }

        protected virtual void Sleep()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        public void Stop()
        {
            stopped = true;
            SetStatus(enRunnableTaskStatus.Stopped);
        }
    }
}

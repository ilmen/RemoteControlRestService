using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Classes.RunnableTasks
{
    public class BatchCommand: IRunnableTask
    {
        string BatchFilePath;

        public enRunnableTaskStatus Status
        { get; set; }

        public BatchCommand(string batchFilePath)
        {
            this.BatchFilePath = batchFilePath;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            // не поддерживаеться
        }
    }
}

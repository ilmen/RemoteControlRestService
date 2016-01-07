using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Classes.RunnableTasks
{
    public class BatchCommand: IRunnableTask
    {
        string BatchFilePath;

        enRunnableTaskStatus status = enRunnableTaskStatus.Added;
        public enRunnableTaskStatus Status
        {
            get
            {
                return status;
            }
        }

        public BatchCommand(string batchFilePath)
        {
            this.BatchFilePath = batchFilePath;
        }

        public void Run()
        {
            try
            {
                status = enRunnableTaskStatus.Running;

                ExecuteCommand(this.BatchFilePath);

                status = enRunnableTaskStatus.Completed;
            }
            catch (Exception ex)
            {
                status = enRunnableTaskStatus.Error;
            }
        }

        static void ExecuteCommand(string filePath)
        {
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo(filePath);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;

            process = Process.Start(processInfo);
            // TODO: выход по таймауту реализовать
            process.WaitForExit();
            process.Close();
        }
    }
}

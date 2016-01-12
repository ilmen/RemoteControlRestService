using System;
using System.Diagnostics;

namespace RemoteControlRestService.Classes.RunnableTasks
{
    public class BatchCommand: IRunnableTask
    {
        string BatchFilePath;

        public enRunnableTaskStatus Status { get; set; } = enRunnableTaskStatus.Added;

        public BatchCommand(string batchFilePath)
        {
            this.BatchFilePath = batchFilePath;
        }

        public void Run()
        {
            try
            {
                Status = enRunnableTaskStatus.Running;

                ExecuteCommand(this.BatchFilePath);

                Status = enRunnableTaskStatus.Completed;
            }
            catch (Exception ex)
            {
                Status = enRunnableTaskStatus.Error;
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

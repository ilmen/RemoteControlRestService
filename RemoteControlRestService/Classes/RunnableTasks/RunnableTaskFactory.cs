using System;

namespace RemoteControlRestService.Classes.RunnableTasks
{
    public class RunnableTaskFactory
    {
        const string BATCH_FILE_PREFIX = @"BatchFiles\";

        public IRunnableTask Create(string command)
        {
            switch (command)
            {
                case "testcommand": return new TestCommand();
                case "hibernate":
                case "restart":
                case "shutdown":
                case "echo": return CreateBatchCommand(command.ToString());
                default: throw new NotImplementedException("Используеться не реализованная команда!");
            }
        }

        BatchCommand CreateBatchCommand(string batchFileName)
        {
            var batchFolder = AppDomain.CurrentDomain.BaseDirectory;
            return new BatchCommand(batchFolder + BATCH_FILE_PREFIX + batchFileName + ".bat");
        }
    }
}

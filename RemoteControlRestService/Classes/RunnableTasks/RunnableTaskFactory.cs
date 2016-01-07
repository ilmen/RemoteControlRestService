using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestService.Classes.RunnableTasks
{
    public class RunnableTaskFactory
    {
        public IRunnableTask Create(string command)
        {
            switch (command)
            {
                case "testcommand": return new TestCommand();
                case "hibernate": return new BatchCommand("hibernate.bat");
                default: throw new NotImplementedException("Используеться не реализованная команда!");
            }
        }
    }
}

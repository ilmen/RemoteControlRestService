using RemoteControlRestService.Infrastracture;
using System.Collections.Generic;

namespace RemoteControlRestService.Classes
{
    public class CommandCollectionFactory : IFactory<IEnumerable<string>>
    {
        static IEnumerable<string> commands = null;
        
        public static void LoadCollection()
        {
            // TODO: уйти от жесткого списка к загрузке содержимого из ФС или конфиг-файла
            commands = new string[] { "testcommand", "hibernate", "restart", "shutdown", "echo" };
        }

        public IEnumerable<string> Create()
        {
            if (commands == null) throw new ConfigurationException("Коллекция команд для задач не загружена!");

            return commands;
        }
    }
}

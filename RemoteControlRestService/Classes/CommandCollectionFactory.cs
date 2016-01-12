using System.Collections.Generic;

namespace RemoteControlRestService.Classes
{
    public class CommandCollectionFactory
    {
        public IEnumerable<string> GetCollection()
        {
            // TODO: уйти от жесткого списка к загрузке содержимого из ФС или конфиг-файла
            return new string[] { "testcommand", "hibernate", "restart", "shutdown", "echo" };
        }
    }
}

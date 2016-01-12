using System.Collections.Generic;

namespace RemoteControlRestService.Classes
{
    public class CommandCollectionFactory
    {
        public IEnumerable<string> GetCollection()
        {
            // TODO: реализовать
            return new string[] { "testcommand", "hibernate", "restart", "shutdown", "echo" };
        }
    }
}

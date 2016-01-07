using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestService.Infrastracture
{
    public class ConfigurationException : Exception
    {
        //public ConfigurationException() : base() { }

        public ConfigurationException(string message) : base(message) { }
    }
}

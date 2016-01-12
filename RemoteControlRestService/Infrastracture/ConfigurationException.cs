using System;

namespace RemoteControlRestService.Infrastracture
{
    public class ConfigurationException : Exception
    {
        //public ConfigurationException() : base() { }

        public ConfigurationException(string message) : base(message) { }
    }
}

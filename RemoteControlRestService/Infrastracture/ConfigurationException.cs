using System;

namespace RemoteControlRestService.Infrastracture
{
    [Serializable]
    public class ConfigurationException : Exception
    {
        //public ConfigurationException() : base() { }

        public ConfigurationException(string message) : base(message) { }
    }
}

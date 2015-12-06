using Newtonsoft.Json;
using System;
using System.Linq;

namespace RemoteControlRestService
{
    public static class Extentions
    {
        //public static string Format(this string value, params object[] formatParameters)
        //{
        //    return String.Format(value, formatParameters);
        //}

        public static bool IsNumeric(this string value)
        {
            if (String.IsNullOrEmpty(value)) return false;

            return value.All(x => char.IsDigit(x));
        }

        public static string GetJsonView(this object value)
        {
            return "Instance of <" + value.GetType().ToString() + "> in JSON view:\r\n" +
                JsonConvert.SerializeObject(value, Formatting.Indented);
        }
    }
}

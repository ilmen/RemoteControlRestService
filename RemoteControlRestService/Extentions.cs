using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestService
{
    public static class Extentions
    {
        //public static string Format(this String value, params object[] formatParameters)
        //{
        //    return String.Format(value, formatParameters);
        //}

        public static bool IsNumeric(this String value)
        {
            return value.All(x => char.IsDigit(x));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestService.Infrastracture
{
    public class SystemTime
    {
        static DateTime? now = null;

        public static DateTime Now
        {
            get
            {
                return now ?? DateTime.Now;
            }
        }

        public static void Set(DateTime value)
        {
            now = value;
        }

        public static void Reset()
        {
            now = null;
        }
    }
}

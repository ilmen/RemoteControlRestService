using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestService
{
    public enum enRunnableTaskStatus
    {
        Added = 0,
        Running = 1,
        Completed = 2,
        Error = 3,
        TimeOut = 4,
        Stopped = 5,
    }
}

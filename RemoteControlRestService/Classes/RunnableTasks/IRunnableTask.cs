using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Classes.RunnableTasks
{
    public interface IRunnableTask
    {
        enRunnableTaskStatus Status
        { get; }

        void Run();

        void Stop();
    }
}

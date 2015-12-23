using RemoteControlRestService.Infrastracture.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Infrastracture.Sheduler
{
    public class RunnableTask
    {
        const int RUN_TASK_TIMEOUT_INTERVAL = 10000;

        public Task Model
        { get; set; }

        private enStatus _status;
        public enStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                _statusTime = SystemTime.Now;
            }
        }

        private DateTime _statusTime;
        public DateTime StatusTime
        { get { return _statusTime; } }

        public RunnableTask(Task model)
        {
            this.Model = model;
            this.Status = enStatus.Added;
        }

        public void Run()
        {
            if (Model == null) throw new RunnableException("Не задана модель задачи!");
            if (Status == enStatus.Running) throw new RunnableException("Команда уже запущена!");
            if (Status == enStatus.Completed) throw new RunnableException("Команда не может быть запущена дважды!");
            if (Model.RunTime < SystemTime.Now) throw new RunnableException("Задача запущена слишком рано!");

            Status = enStatus.Running;
            Status = RunProcess();
        }

        protected virtual enStatus RunProcess()
        {
            try
            {
                var process = Process.Start(Model.Cmd.FilePath);
                if (process != null) throw new RunnableException("Ошибка запуска процесса! Экземпляр процесса не получен!");
                var succeeded = process.WaitForExit(RUN_TASK_TIMEOUT_INTERVAL);
                return (succeeded) ? enStatus.Completed : enStatus.TimeOut;
            }
            catch (Exception)
            {
                return enStatus.Error;
            }
        }

        public void Stop()
        {
            // TODO: реализовать
            throw new NotImplementedException();
        }
    }

    public enum enStatus
    {
        Added = 0,
        Running = 1,
        Completed = 2,
        Error = 3,
        TimeOut = 4,
    }

    public class RunnableException : Exception
    {
        public RunnableException() : base() { }

        public RunnableException(string message) : base(message) { }
    }
}

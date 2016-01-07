using RemoteControlRestService.Classes.RunnableTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace RemoteControlRestService.Classes
{
    [DataContract]
    public class Task
    {
        [DataMember]
        public Guid Id
        { get; set; }

        [DataMember]
        public DateTime CreateTime
        { get; set; }

        [DataMember]
        public DateTime RunTime
        { get; set; }

        [DataMember]
        public string CommandType
        { get; set; }

        public IRunnableTask RunnableTask
        { get; set; }

        #region Equals and = overriding
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return Equals(obj as Task);
        }

        public bool Equals(Task obj)
        {
            if (obj == null) return false;

            return
                this.Id == obj.Id &&
                this.CreateTime == obj.CreateTime &&
                this.RunTime == obj.RunTime &&
                this.CommandType == obj.CommandType &&
                this.RunnableTask == obj.RunnableTask;
        }

        public override int GetHashCode()
        {
            return
                Id.GetHashCode() +
                CreateTime.GetHashCode() +
                RunTime.GetHashCode() +
                (CommandType == null ? 0 : CommandType.GetHashCode()) +
                (RunnableTask == null ? 0 : RunnableTask.GetHashCode());
        }
        #endregion

        public override string ToString()
        {
            return this.GetJsonView();
        }
    }
}

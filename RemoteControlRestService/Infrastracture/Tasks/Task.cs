using Newtonsoft.Json;
using RemoteControlRestService.Infrastracture.Commands;
using System;
using System.Runtime.Serialization;

namespace RemoteControlRestService.Infrastracture.Tasks
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
        public Command Cmd
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
                this.Cmd == obj.Cmd;
        }

        public override int GetHashCode()
        {
            return
                Id.GetHashCode() +
                CreateTime.GetHashCode() +
                RunTime.GetHashCode() +
                (Cmd == null ? 0 : Cmd.GetHashCode());
        }
        #endregion

        public override string ToString()
        {
            return this.GetJsonView();
        }
    }
}

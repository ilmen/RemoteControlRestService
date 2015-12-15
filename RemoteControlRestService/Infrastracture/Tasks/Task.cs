using RemoteControlRestService.Infrastracture.Commands;
using System;

namespace RemoteControlRestService.Infrastracture.Tasks
{
    [Serializable]
    public class Task
    {
        public Guid Id
        { get; set; }

        public DateTime CreateTime
        { get; set; }

        public DateTime RunTime
        { get; set; }

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

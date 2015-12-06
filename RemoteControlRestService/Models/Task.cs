using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Models
{
    [Serializable]
    public class Task
    {
        public Guid Id
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

            return this.Id == obj.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        } 
        #endregion

        public override string ToString()
        {
            return this.GetJsonView();
        }
    }
}

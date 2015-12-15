using Newtonsoft.Json;

namespace RemoteControlRestService.Infrastracture.Commands
{
    public class Command
    {
        public int Id
        { get; set; }

        [JsonIgnore]
        public string FilePath
        { get; set; }

        public string Name
        { get; set; }

        #region Equals and = overriding
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return Equals(obj as Command);
        }

        public bool Equals(Command obj)
        {
            if (obj == null) return false;

            return
                this.Id == obj.Id &&
                this.FilePath == obj.FilePath &&
                this.Name == obj.Name;
        }

        public override int GetHashCode()
        {
            return
                Id.GetHashCode() +
                (FilePath ?? "").GetHashCode() +
                (Name ?? "").GetHashCode();
        }
        #endregion

        public override string ToString()
        {
            return this.GetJsonView();
        }
    }
}

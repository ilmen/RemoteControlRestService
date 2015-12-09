using RemoteControlRestService.Infrastracture;
using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Models
{
    [Serializable]
    public class Task : IValidate
    {
        public Guid Id
        { get; set; }

        public DateTime CreateTime
        { get; set; }

        public DateTime RunTime 
        { get; set; }

        public string Description 
        { get; set; }

        public string ScriptFilePath
        { get; set; }

        public IValidator<string> DescriptionValidator
        { get; set; }

        public IValidator<string> FilePathValidator
        { get; set; }

        public Task()
        {
            DescriptionValidator = new StringLengthValidator(200);
            FilePathValidator = new FilePathValidator();
        }

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
                this.Description == obj.Description &&
                this.ScriptFilePath == obj.ScriptFilePath;
        }

        public override int GetHashCode()
        {
            return
                Id.GetHashCode() +
                CreateTime.GetHashCode() +
                RunTime.GetHashCode() +
                (Description ?? "").GetHashCode() +
                (ScriptFilePath ?? "").GetHashCode();
        } 
        #endregion

        public override string ToString()
        {
            return this.GetJsonView();
        }

        public ValidResult IsValid()
        {
            if (this.Id == Guid.Empty) return ValidResult.GetInvalidResult("Не задан идентификатор задачи!");
            
            if (this.CreateTime > this.RunTime) return ValidResult.GetInvalidResult("Время запуска задачи не может быть меньше времени создания задачи!");

            var descResult = DescriptionValidator.Validate(this.Description);
            if (descResult != ValidResult.Valid) return descResult;

            var scriptFilePathResult = FilePathValidator.Validate(this.ScriptFilePath);
            if (scriptFilePathResult != ValidResult.Valid) return scriptFilePathResult;

            return ValidResult.Valid;
        }
    }
}

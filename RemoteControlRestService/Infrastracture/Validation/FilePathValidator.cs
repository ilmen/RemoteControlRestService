using System;
using System.IO;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class FilePathValidator : IValidator<string>
    {
        public ValidResult Validate(string value)
        {
            if (String.IsNullOrEmpty(value)) return ValidResult.GetInvalidResult("Не задан путь к файлу!");
            if (!File.Exists(value)) return ValidResult.GetInvalidResult("Файл по пути \"" + value + "\" не найден!");

            return ValidResult.Valid;
        }
    }
}

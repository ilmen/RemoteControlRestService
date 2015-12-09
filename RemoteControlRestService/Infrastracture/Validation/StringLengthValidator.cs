using System;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class StringLengthValidator : IValidator<string>
    {
        int MaxStringLength;

        public StringLengthValidator(int maxStringLength)
        {
            if (maxStringLength < 1) throw new ArgumentException("Ограничитель максимальной длины строки должен быть натуральным числом!");

            this.MaxStringLength = maxStringLength;
        }

        public ValidResult Validate(string value)
        {
            if (value != null && value.Length > MaxStringLength) return ValidResult.GetInvalidResult("Строка \"" + value + "\" слишком длинная! Максимальная длина строки: " + MaxStringLength);

            return ValidResult.Valid;
        }
    }
}

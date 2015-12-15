using System;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class ValidResult
    {
        static ValidResult valid = new ValidResult()
        {
            IsValid = true,
            ErrorMessage = String.Empty
        };

        public static ValidResult Valid
        {
            get
            {
                return valid;
            }
        }

        #region Свойства
        public bool IsValid
        { get; private set; }

        // TODO: Вместо текстового поля сделать расширяемый список ErrorMessages
        public string ErrorMessage
        { get; private set; }
        #endregion

        private ValidResult() { }

        public static ValidResult GetInvalidResult(string errorMessage)
        {
            return new ValidResult()
            {
                IsValid = false,
                ErrorMessage = errorMessage
            };
        }

        public void ThrowExceptionIfNotValid()
        {
            if (!this.IsValid) throw new CheckValidException(this.ErrorMessage);
        }

        public override string ToString()
        {
            return this.GetJsonView();
        }
    }

    public class CheckValidException : Exception
    {
        //public CheckValidException() : base() { }

        public CheckValidException(string message) : base(message) { }
    }
}

using System;
using System.Linq;

namespace RemoteControlRestService.Classes
{
    public class CommandLineSettingProvider
    {
        const string PORT_PARAMETER_NAME = "port";

        public CommandLineSetting GetSettings(string[] args)
        {
            if (args == null) throw new ArgumentNullException();
            if (args.Any(x => !x.Contains('='))) throw new ArgumentException("Допустимы лишь параметры вида <ключ>=<значение>!");

            var argsParams = args
                .Select(x => x.Split('='))
                .Select(x => new
                {
                    Key = x[0],
                    Value = x[1]
                });

            var portParam = argsParams.SingleOrDefault(x => x.Key == PORT_PARAMETER_NAME);
            if (portParam == null) ThrowPortParametrNotFounded();
            if (!portParam.Value.IsNumeric()) ThrowWrongPortException();

            int portInt = Convert.ToInt32(portParam.Value);
            if (portInt < 1 || portInt > ushort.MaxValue) ThrowWrongPortException();

            return new CommandLineSetting() { Port = (ushort)portInt };
        }

        void ThrowPortParametrNotFounded()
        {
            throw new ArgumentException($"Не найден параметр \"{PORT_PARAMETER_NAME}=<номер порта>\" во входных аргументах!";
        }

        void ThrowWrongPortException()
        {
            throw new InvalidCastException($"Номер порта должен быть натуральным числом не больше {ushort.MaxValue}!";
        }
    }

    public class CommandLineSetting
    {
        public ushort Port
        { get; set; }

        #region Equals overriding
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return Equals(obj as CommandLineSetting);
        }

        public bool Equals(CommandLineSetting obj)
        {
            if (obj == null) return false;

            return this.Port == obj.Port;
        }

        public override int GetHashCode()
        {
            return Port;
        } 
        #endregion

        public override string ToString()
        {
            return this.GetJsonView();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestService.Infrastracture
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
            throw new ArgumentException(String.Format("Не найден параметр \"{0}=<номер порта>\" во входных аргументах!", PORT_PARAMETER_NAME));
        }

        void ThrowWrongPortException()
        {
            throw new InvalidCastException(String.Format("Номер порта должен быть натуральным числом не больше {0}!", ushort.MaxValue));
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestService
{
    public class PortProvider
    {
        const string PORT_PARAMETER_NAME = "port";

        public ushort GetPort(string[] args)
        {
            if (args == null) throw new ArgumentNullException();

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
            
            return (ushort)portInt;                
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
}

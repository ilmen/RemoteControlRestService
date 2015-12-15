using RemoteControlRestService.Infrastracture.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class CommandValidator : IValidator<Command>
    {
        public ValidResult Validate(Command value)
        {
            throw new NotImplementedException();
        }
    }
}

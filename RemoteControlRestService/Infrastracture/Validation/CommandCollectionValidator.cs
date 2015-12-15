using RemoteControlRestService.Infrastracture.Commands;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestService.Infrastracture.Validation
{
    public class CommandCollectionValidator : IValidator<IEnumerable<Command>>
    {
        public ValidResult Validate(IEnumerable<Command> value)
        {
            if (value == null) return ValidResult.GetInvalidResult("Не задана ссылка на коллекцию команд!");

            var existsDublicates = value.Count() != value.Distinct().Count();
            if (existsDublicates) return ValidResult.GetInvalidResult("В коллекции команд не допускаються дубликаты!");

            return ValidResult.Valid;
        }
    }
}

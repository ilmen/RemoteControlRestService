using RemoteControlRestService.Infrastracture.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestService.Infrastracture.Commands
{
    public class CommandCollectionFactory
    {
        IValidator<IEnumerable<Command>>  Validator;

        public CommandCollectionFactory() : this(new CommandCollectionValidator()) { }

        public CommandCollectionFactory(IValidator<IEnumerable<Command>> validator)
        {
            Validator = validator;
        }

        public IEnumerable<Command> GetCollection()
        {
            var files = GetBatFiles();

            var rawCommands = files
                .Select(x => new
                {
                    File = x,
                    Name = GetReadableCommandName(x)
                });

            var commands = rawCommands
                .Select((x, index) => new Command()
                {
                    Id = index + 1,
                    FilePath = x.File,
                    Name = x.Name
                })
                .ToArray();

            ThrownIfNotValid(commands);

            return commands;
        }

        void ThrownIfNotValid(IEnumerable<Command> commands)
        {
            var validateResult = Validator.Validate(commands);
            if (validateResult != ValidResult.Valid) throw new ConfigurationException("Ошибка формирования команд! Одна или несколько команд не прошли валидацию! Об ошибке: " + validateResult.ToString());
        }

        public string GetReadableCommandName(string fileName)
        {
            if (String.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("Некорректное имя файла!");

            return fileName
                .ToLower()
                .Replace('_', ' ')
                .Replace('-', ' ')
                .Replace(".bat", "")
                .ToCapitalize();
        }

        protected virtual IEnumerable<string> GetBatFiles()
        {
            // TODO: анализировать файловую систему (только на один уровень вложенности, это позволить имени BAT файла быть уникальным). Уникальность имени также требует работы только с один расширением, вот в чем подвох
            return new List<string>()
            {
                "reset.bat",
                "shutdown.bat",
                "hibernate.bat",
                "restart_serviio.bat"
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteControlRestService.Infrastracture.Commands
{
    public class CommandCollectionFactory
    {
        public IEnumerable<Command> GetCommands()
        {
            var files = GetBatFiles();

            var commands = files
                .Select(x => new
                {
                    File = x,
                    Name = GetReadableCommandName(x)
                });

            return commands
                .Select((x, index) => new Command()
                {
                    Index = index + 1,
                    FilePath = x.File,
                    Name = x.Name
                })
                .ToList();
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

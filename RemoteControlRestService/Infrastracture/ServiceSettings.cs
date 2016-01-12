using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RemoteControlRestService.Infrastracture
{
    public class ServiceSettings
    {
        public int FindNewTaskTimerInteval { get; set; } = 30;
        public string[] Endpoints { get; set; } = new string[] { "localhost:9000" };
        
        public static ServiceSettings Default => new ServiceSettings();

        public IEnumerable<IPEndPoint> GetEndpoints()
        {
            // TODO: использовать метод в момент считывания настроек из файла чтобы не откладывать исключение к моменту использования уже считанных настроек
            return Endpoints
                .Select(x => x.Split(new char[] { ':' }))
                .Where(x => x?[0] != null && x?[1] != null)
                .Select(x => new IPEndPoint(IPAddress.Parse(x[0]), ushort.Parse(x[1])))
                .ToList();
        }
    }
}

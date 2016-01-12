using Newtonsoft.Json;
using System;
using System.IO;

namespace RemoteControlRestService.Infrastracture
{
    public class ServiceSettingsProvider
    {
        static volatile ServiceSettings CurrentSettings;

        public ServiceSettings GetSettings()
        {
            if (CurrentSettings == null) CurrentSettings = ReadSettings();

            return CurrentSettings;
        }

        ServiceSettings ReadSettings()
        {
            try
            {
                var filename = "settings.json";

                if (File.Exists(filename))
                {
                    var json = File.ReadAllText(filename);
                    return JsonConvert.DeserializeObject<ServiceSettings>(json);
                }
                else
                {
                    var json = JsonConvert.SerializeObject(ServiceSettings.Default);
                    File.WriteAllText(filename, json);
                    return ServiceSettings.Default;
                }
            }
            catch (Exception ex)
            {
                return ServiceSettings.Default;
            }
        }
    }
}

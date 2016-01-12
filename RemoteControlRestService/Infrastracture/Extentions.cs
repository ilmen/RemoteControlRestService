using Newtonsoft.Json;
using System;
using System.Linq;

namespace RemoteControlRestService
{
    public static class Extentions
    {
        public static bool IsNumeric(this string value)
        {
            if (String.IsNullOrEmpty(value)) return false;

            return value.All(x => char.IsDigit(x));
        }

        public static string ToCapitalize(this string value)
        {
            if (String.IsNullOrEmpty(value)) return value;

            if (value.Length == 1) return value.ToUpper();

            // TODO: Пока нет необходимости обрабатывать тут строку из нескольких предпложений. В будущем (невероятно удаленном) можно будет разбивать строку по '.', '!', '?', ';' и по отдельности обрабатывать, не забывая о табуляции и переносах...но кому это нужно?

            return char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }

        public static string GetJsonView(this object value)
        {
            return "Instance of <" + value.GetType().ToString() + "> in JSON view:\r\n" +
                JsonConvert.SerializeObject(value, Formatting.Indented);
        }
    }
}

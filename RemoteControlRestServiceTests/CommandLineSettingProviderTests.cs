using NUnit.Framework;
using RemoteControlRestService;
using System;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class CommandLineSettingProviderTests
    {
        [Test]
        public void GetSettings_NullArgs_ThrownArgumentNullException()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = null;

            Assert.Catch<ArgumentNullException>(() => prov.GetSettings(args));
        }

        [Test]
        public void GetSettings_PortParameterMissed_ThrownArgumentException()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = new string[] { };

            var exc = Assert.Catch<ArgumentException>(() => prov.GetSettings(args));
            StringAssert.Contains("Не найден параметр \"port", exc.Message);
        }

        [Test]
        public void GetSettings_WrongParametersSeparator_ThrownArgumentException()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = new string[] { "port:8000" };

            var exc = Assert.Catch<ArgumentException>(() => prov.GetSettings(args));
            StringAssert.Contains("<ключ>=<значение>", exc.Message);
        }

        [Test]
        public void GetSettings_TwoPortParametersInArgs_ThrownException()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = new string[] { "port=1", "port=2" };

            var exc = Assert.Catch<InvalidOperationException>(() => prov.GetSettings(args));
            StringAssert.Contains("Последовательность содержит более одного соответствующего элемента", exc.Message);
        }

        [Test]
        public void GetSettings_NotNumericPortParameterValue_ThrownInvalidCastException()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = new string[] { "port=not_numeric_value" };

            var exc = Assert.Catch<InvalidCastException>(() => prov.GetSettings(args));
            StringAssert.Contains("Номер порта", exc.Message);
        }

        [Test]
        public void GetSettings_TooMuchPortParameterValue_ThrownInvalidCastException()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = new string[] { "port=9999999" };

            var exc = Assert.Catch<InvalidCastException>(() => prov.GetSettings(args));
            StringAssert.Contains("не больше 65535", exc.Message);
        }

        [Test]
        public void GetSettings_ZeroPortParameterValue_ThrownInvalidCastException()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = new string[] { "port=0" };

            var exc = Assert.Catch<InvalidCastException>(() => prov.GetSettings(args));
            StringAssert.Contains("Номер порта", exc.Message);
        }

        [Test]
        public void GetSettings_PortParameterValueMissed_ThrownInvalidCastException()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = new string[] { "port=" };

            var exc = Assert.Catch<InvalidCastException>(() => prov.GetSettings(args));
            StringAssert.Contains("Номер порта", exc.Message);
        }

        [Test]
        public void GetSettings_CorrectPortParameter_ReturnsCorrectSettings()
        {
            var prov = new CommandLineSettingProvider();
            string[] args = new string[] { "port=8000" };
            var expected = new CommandLineSetting() { Port = 8000 };

            var settings = prov.GetSettings(args);

            Assert.AreEqual(expected, settings);
        }
    }
}

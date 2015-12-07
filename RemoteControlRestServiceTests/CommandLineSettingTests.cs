using NSubstitute;
using NUnit.Framework;
using RemoteControlRestService.Infrastracture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class CommandLineSettingTests
    {
        [Test]
        public void Equals_NullObjectEquals_ReturnsFalse()
        {
            var excepted = new CommandLineSetting() { Port = Arg.Any<ushort>() };
            object nullObject = null;

            Assert.IsFalse(excepted.Equals(nullObject));
        }

        [Test]
        public void Equals_NullCommandLineSettingEquals_ReturnsFalse()
        {
            var excepted = new CommandLineSetting() { Port = Arg.Any<ushort>() };
            CommandLineSetting nullObject = null;

            Assert.IsFalse(excepted.Equals(nullObject));
        }

        [Test]
        public void Equals_EqualsSettings_ReturnsTrue()
        {
            var excepted = new CommandLineSetting() { Port = 1000 };
            var actual = new CommandLineSetting() { Port = 1000 };

            Assert.IsTrue(excepted.Equals(actual));
        }

        [Test]
        public void Equals_NotEqualsSettings_ReturnsFalse()
        {
            var excepted = new CommandLineSetting() { Port = 1000 };
            var actual = new CommandLineSetting() { Port = 2000 };

            Assert.IsFalse(excepted.Equals(actual));
        }

        [Test]
        public void GetHashCode_NotEqualsSettings_ReturnsNotEqualsHashCodes()
        {
            var excepted = new CommandLineSetting() { Port = 1000 };
            var actual = new CommandLineSetting() { Port = 2000 };

            Assert.AreNotEqual(excepted.GetHashCode(), actual.GetHashCode());
        }


        [Test]
        public void GetHashCode_EqualsSettings_ReturnsEqualsHashCodes()
        {
            var excepted = new CommandLineSetting() { Port = 1000 };
            var actual = new CommandLineSetting() { Port = 1000 };

            Assert.AreEqual(excepted.GetHashCode(), actual.GetHashCode());
        }
    }
}

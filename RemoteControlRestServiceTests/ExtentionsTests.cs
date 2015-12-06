using NUnit.Framework;
using RemoteControlRestService;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class ExtentionsTests
    {
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("1abc", false)]
        [TestCase("1.", false)]
        [TestCase("1.0", false)]
        [TestCase("1", true)]
        public void IsNumeric_Always_RetunsCorrectValue(string value, bool expected)
        {
            Assert.AreEqual(expected, value.IsNumeric());
        }
    }
}

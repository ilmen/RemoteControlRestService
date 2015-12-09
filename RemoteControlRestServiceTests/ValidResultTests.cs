using NUnit.Framework;
using RemoteControlRestService.Infrastracture.Validation;
using System;

namespace RemoteControlRestServiceTests
{
    [TestFixture]
    public class ValidResultTests
    {
        [Test]
        public void ValidProperty_Always_ReturnsValidResult()
        {
            var valid = ValidResult.Valid;

            Assert.IsTrue(valid.IsValid);
        }

        [Test]
        public void ValidProperty_Always_ReturnsValidResultWithEmptyErrorMessage()
        {
            var valid = ValidResult.Valid;

            Assert.AreEqual(String.Empty, valid.ErrorMessage);
        }

        [Test]
        public void GetInvalidResult_Always_ReturnsInvalidValue()
        {
            var valid = ValidResult.GetInvalidResult("some error message");

            Assert.IsFalse(valid.IsValid);
        }

        [Test]
        public void GetInvalidResult_Always_ReturnsInvalidValueWithCorrectErrorMessage()
        {
            const string ERROR_MESSAGE = "my error message";
            var valid = ValidResult.GetInvalidResult(ERROR_MESSAGE);

            StringAssert.Contains(ERROR_MESSAGE, valid.ErrorMessage);
        }

        [Test]
        public void ThrowExceptionIfNotValid_Valid_NotThrowns()
        {
            var valid = ValidResult.Valid;

            valid.ThrowExceptionIfNotValid();
        }

        [Test]
        public void ThrowExceptionIfNotValid_Invalid_ThrownsCheckValidException()
        {
            const string INVALID_ERROR_MESSAGE = "error!";
            var invalid = ValidResult.GetInvalidResult(INVALID_ERROR_MESSAGE);

            var exc = Assert.Catch<CheckValidException>(() => invalid.ThrowExceptionIfNotValid());
            StringAssert.Contains(INVALID_ERROR_MESSAGE, exc.Message);
        }
    }
}

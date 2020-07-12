using Lib;
using NUnit.Framework;

namespace HolidayTests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [Test()]
        public void is_valid()
        {
            var target = new AuthenticationService();

            var actual = target.IsValid("joey", "91000000");

            //always failed
            Assert.IsTrue(actual);
        } 
    }
}
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
            // var target = new AuthenticationService();
            var target = new AuthenticationService(new FakeProfile(), new FakeToken());

            var actual = target.IsValid("joey", "91000000");

            //always failed
            Assert.IsTrue(actual);
        }
    }

    public class FakeProfile : IProfile
    {
        public string GetPassword(string account)
        {
            return account == "joey" ? "91" : "";
        }
    }

    public class FakeToken : IToken
    {
        public string GetRandom(string account)
        {
            return "000000";
        }
    }
}
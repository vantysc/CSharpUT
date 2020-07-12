#region

using Lib;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace HolidayTests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private IProfile _profile;
        private AuthenticationService _target;
        private IToken _token;
        private ILogger _logger;

        [SetUp]
        public void SetUp()
        {
            _profile = Substitute.For<IProfile>();
            _token = Substitute.For<IToken>();
            _logger = Substitute.For<ILogger>();

            _target = new AuthenticationService(_profile, _token, _logger);
        }

        [Test]
        public void is_valid()
        {
            GivenPassword("joey", "91");
            GivenToken("000000");
            ShouldBeValid("joey", "91000000");
        }

        [Test]
        public void is_invalid()
        {
            GivenPassword("joey", "91");
            GivenToken("000000");
            ShouldBeInvalid("joey", "wrong password");
        }

        [Test]
        public void should_log_account_when_invalid()
        {
            GivenPassword("joey", "91");
            GivenToken("000000");
            _target.IsValid("joey", "wrong password");
            _logger.Received(1)
                   .Info(Arg.Is<string>(m=>m.Contains("joey") && m.Contains("login failed")));
                   // .Info("account:joey try to login failed");
        }

        private void ShouldBeInvalid(string account, string password)
        {
            var actual = _target.IsValid(account, password);
            Assert.IsFalse(actual);
        }

        private void ShouldBeValid(string account, string password)
        {
            var actual = _target.IsValid(account, password);
            Assert.IsTrue(actual);
        }

        private void GivenToken(string token)
        {
            _token.GetRandom("").ReturnsForAnyArgs(token);
        }

        private void GivenPassword(string account, string password)
        {
            _profile.GetPassword(account).Returns(password);
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
using System;
using Lib;
using NUnit.Framework;

namespace HolidayTests
{
    [TestFixture]
    public class HolidayTests
    {
        [Test]
        public void today_is_xmas()
        {
            var holiday = new Holiday();
            Assert.AreEqual("Merry Xmas", holiday.SayHello());
        }
    }
}
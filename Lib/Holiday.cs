using System;

namespace Lib
{
    public class Holiday
    {
        public string SayHello()
        {
            var today = DateTime.Today;
            if (today.Month == 12 && today.Day == 25)
            {
                return "Merry Xmas";
            }

            return "Today is not Xmas";
        }
    }
}
using System.Text.RegularExpressions;

namespace Tools
{
    public static class Extensions
    {
        static Regex _integerRegex = new Regex(@"\d+");

        public static int[] GetIntegers(this string data) =>
            _integerRegex.Matches(data).Select(m => int.Parse(m.Value)).ToArray();

    }
}
using System.Text.RegularExpressions;

namespace Tools
{
    public static class Extensions
    {
        static Regex _integerRegex = new Regex(@"\d+");

        public static int[] GetIntegers(this string data) =>
            _integerRegex.Matches(data).Select(m => int.Parse(m.Value)).ToArray();

        public static IEnumerable<T[]> Window<T>(this IEnumerable<T> items, int size) =>
            Enumerable.Range(0, items.Count() - size + 1).Select(i => items.Skip(i).Take(size).ToArray());

    }
}
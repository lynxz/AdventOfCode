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

        public static T[,]? ToMultidimensionalArray<T>(this IEnumerable<IEnumerable<T>> array) {
            if (array == null)
                return null;
            
            var yMax = array.Count();
            var xMax = array.First().Count();
            var mArray = new T[yMax, xMax];
            for(int y = 0; y < yMax; y++) 
                for (int x = 0; x < xMax; x++)
                    mArray[y,x] = array.ElementAt(y).ElementAt(x);
                    
            return mArray;
        }

    }
}
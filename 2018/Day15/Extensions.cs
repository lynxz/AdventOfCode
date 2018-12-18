using System.Collections.Generic;
using System.Linq;

namespace Day15
{

    public static class Extensions
    {
        public static T[,] ToTwoDimensionalArray<T>(this IEnumerable<IEnumerable<T>> enumerable)
        {
            var lines = enumerable.Select(inner => inner.ToArray()).ToArray();
            var columnCount = lines.Max(columns => columns.Length);
            var twa = new T[lines.Length, columnCount];
            for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];
                for (var columnIndex = 0; columnIndex < line.Length; columnIndex++)
                {
                    twa[lineIndex, columnIndex] = line[columnIndex];
                }
            }
            return twa;
        }
    }

}
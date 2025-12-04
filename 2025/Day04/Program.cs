using System.Runtime.InteropServices;
using Tools;

var day = new Day04();
day.OutputSecondStar();

public class Day04 : DayBase
{
    public Day04() : base("4")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(s => s.Trim().ToCharArray()).ToMultidimensionalArray();
        (int x, int y)[] offsets = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];
        var sum = 0;
        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                var current = data[y, x];

                var rolls = 0;
                if (current != '@')
                {
                    continue;
                }
                foreach (var (dx, dy) in offsets)
                {
                    if (x + dx < data.GetLength(1) && y + dy < data.GetLength(0) && y + dy >= 0 && x + dx >= 0)
                    {
                        if (data[y + dy, x + dx] == '@')
                        {
                            rolls++;
                        }
                    }
                }
                if (rolls < 4)
                {
                    sum++;
                }
            }

        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(s => s.Trim().ToCharArray()).ToMultidimensionalArray();
        (int x, int y)[] offsets = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];

        var totalSum = 0;
        var sum = 0;

        do
        {
            sum = 0;
            var copy = new char[data.GetLength(0), data.GetLength(1)];
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    var current = data[y, x];

                    var rolls = 0;
                    copy[y, x] = current;
                    if (current != '@')
                    {
                        continue;
                    }
                    foreach (var (dx, dy) in offsets)
                    {
                        if (x + dx < data.GetLength(1) && y + dy < data.GetLength(0) && y + dy >= 0 && x + dx >= 0)
                        {
                            if (data[y + dy, x + dx] == '@')
                            {
                                rolls++;
                            }
                        }
                    }
                    if (rolls < 4)
                    {
                        copy[y, x] = '.';
                        sum++;
                    }
                }
            }
            data = copy;
            totalSum += sum;
        } while (sum > 0);

        return totalSum.ToString();
    }
}
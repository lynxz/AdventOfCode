
using System.Runtime.CompilerServices;
using System.Text;
using Tools;

Day03 day = new Day03();
day.OutputSecondStar();

public class Day03 : DayBase
{
    public Day03() : base("3")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData();
        var array = data.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(y => y.ToCharArray()).ToMultidimensionalArray<char>();
        var offsets = new (int y, int x)[] { (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1) };
        var yMax = array.GetLength(0);
        var xMax = array.GetLength(1);
        var sum = 0;
        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                if (array[y, x] == '.')
                {
                    continue;
                }

                if (char.IsDigit(array[y, x]))
                {
                    var num = new StringBuilder();
                    var isPart = false;
                    while (x < xMax && char.IsDigit(array[y, x]))
                    {
                        if (!isPart)
                        {
                            isPart = offsets.Any(o =>
                                o.x + x > 0 
                                && o.x + x < xMax 
                                && o.y + y > 0 
                                && o.y + y < yMax
                                && array[o.y + y, o.x + x] != '.'
                                && !char.IsNumber(array[o.y + y, o.x + x]));
                        }
                        num.Append(array[y, x]);
                        x++;

                    }
                    if (isPart)
                    {
                        sum += int.Parse(num.ToString());
                    }
                }

            }
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData();
        var array = data.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(y => y.ToCharArray()).ToMultidimensionalArray<char>();
        var offsets = new (int y, int x)[] { (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1) };
        var yMax = array.GetLength(0);
        var xMax = array.GetLength(1);
        var sum = 0L;
        var gears = new List<(int y, int x, long num)>();
        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                if (array[y, x] == '.')
                {
                    continue;
                }

                if (char.IsDigit(array[y, x]))
                {
                    var num = new StringBuilder();
                    var isPart = false;
                    var gear = (y, x, 0L);
                    while (x < xMax && char.IsDigit(array[y, x]))
                    {
                        if (!isPart)
                        {
                            var offset = offsets.FirstOrDefault(o =>
                                o.x + x > 0 
                                && o.x + x < xMax 
                                && o.y + y > 0 
                                && o.y + y < yMax
                                && array[o.y + y, o.x + x] == '*'
                                && !char.IsNumber(array[o.y + y, o.x + x]));
                                if (offset != default)
                                {
                                    isPart = true;
                                    gear = (y + offset.y, x + offset.x, 0);
                                }
                        }
                        num.Append(array[y, x]);
                        x++;

                    }
                    if (isPart)
                    {
                       gears.Add((gear.y, gear.x, int.Parse(num.ToString())));
                    }
                }

            }
        }

        var groups = gears.GroupBy(g => (g.y, g.x)).Where(g => g.Count() > 1);
        return groups.Select(g => g.Aggregate(1L, (acc, s) => acc * s.num)).Sum().ToString();
    }
}
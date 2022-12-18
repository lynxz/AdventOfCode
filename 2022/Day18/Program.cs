using Tools;

Day18 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day18 : DayBase
{
    public Day18() : base("18")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.GetIntegers()).ToArray();

        var offsets = new List<int[]> {
            new [] {1, 0, 0},
            new [] {0, 1, 0},
            new [] {0, 0, 1},
            new [] {-1, 0, 0},
            new [] {0, -1, 0},
            new [] {0, 0, -1},
            };

        var sum = 0;
        foreach (var d in data)
        {
            foreach (var of in offsets.Select(o => Enumerable.Range(0, 3).Select(i => o[i] + d[i]).ToArray()))
            {
                if (!data.Any(d => d[0] == of[0] && d[1] == of[1] && d[2] == of[2]))
                {
                    sum++;
                }
            }
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.GetIntegers()).ToArray();
        var offsets = new List<int[]> {
            new [] {1, 0, 0},
            new [] {0, 1, 0},
            new [] {0, 0, 1},
            new [] {-1, 0, 0},
            new [] {0, -1, 0},
            new [] {0, 0, -1},
            };
        var xMax = data.Max(d => d[0]);
        var xMin = data.Min(d => d[0]);
        var yMax = data.Max(d => d[1]);
        var yMin = data.Min(d => d[1]);
        var zMax = data.Max(d => d[2]);
        var zMin = data.Min(d => d[2]);

        var points = new HashSet<(int X, int Y, int Z)>();
        Fill(points, xMax, yMax, zMax, data);

        foreach (var p in data)
            points.Add((X: p[0], Y: p[1], Z: p[2]));

        var sum = 0;
        var i = 0;
        for (int x = xMin; x < xMax; x++)
        {
            for (int y = yMin; y < yMax; y++)
            {
                for (int z = zMin; z < zMax; z++)
                {
                    var coord = (X: x, Y: y, Z: z);
                    if (!points.Contains(coord))
                    {
                        i++;
                        sum += offsets.Sum(o => data.Count(d => d[0] == o[0] + x && d[1] == o[1] + y && d[2] == o[2] + z));
                    }
                }
            }
        }

        return (4192 - sum).ToString();
    }

    void Fill(HashSet<(int X, int Y, int Z)> set, int xMax, int yMax, int zMax, int[][] drops)
    {
        var stack = new Stack<int[]>();
        stack.Push(new[] { 0, 0, 0 });
        while (stack.Count > 0)
        {
            var p = stack.Pop();
            if (p[0] > xMax || p[1] > yMax || p[2] > zMax || p[0] < 0 || p[1] < 0 || p[2] < 0)
                continue;
            if (drops.Any(d => d[0] == p[0] && d[1] == p[1] && d[2] == p[2]))
                continue;

            var coord = (X: p[0], Y: p[1], Z: p[2]);
            if (set.Contains(coord))
                continue;
            set.Add(coord);

            stack.Push(new[] { 1 + p[0], p[1], p[2] });
            stack.Push(new[] { p[0], 1 + p[1], p[2] });
            stack.Push(new[] { p[0], p[1], 1 + p[2] });
            stack.Push(new[] { p[0] - 1, p[1], p[2] });
            stack.Push(new[] { p[0], p[1] - 1, p[2] });
            stack.Push(new[] { p[0], p[1], p[2] - 1 });
        }
    }

}
using Tools;

Day09 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day09 : DayBase
{
    public Day09() : base("9")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.GetIntegers().ToArray()).ToArray();
        return data.DifferentCombinations(2).Max(pair =>
        {
            var first = pair.First();
            var last = pair.Last();
            return (long)(Math.Abs(first[0] - last[0]) + 1) * (Math.Abs(first[1] - last[1]) + 1);
        }).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.GetIntegers().ToArray()).ToArray();
        var max = 0L;
        for (int i = 0; i < data.Length; i++)
        {
            var j = (i + 1) % data.Length;
            while (j != i)
            {
                var first = data[i];
                var last = data[j];
                var rect = new Rectangle(
                    Math.Min(first[0], last[0]),
                    Math.Min(first[1], last[1]),
                    Math.Max(first[0], last[0]),
                    Math.Max(first[1], last[1]));

                if (rect.Size <= max)
                {
                    j = (j + 1) % data.Length;
                    continue;
                }

                if (AreCornersInside(rect, data) && AreSidesNotIntersecting(rect, data))
                {
                    max = Math.Max(max, rect.Size);
                }


                j = (j + 1) % data.Length;
            }
        }

        return max.ToString();
    }

    private bool AreSidesNotIntersecting(Rectangle rect, int[][] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            var first = points[i];
            var last = points[(i + 1) % points.Length];
            if (rect.Intersects((first[0], first[1]), (last[0], last[1])))
            {
                return false;
            }
        }
        return true;
    }

    bool AreCornersInside(Rectangle rect, int[][] points)
    {
        var lines = new List<((int X, int Y) Start, (int X, int Y) End)>();
        for (int i = 0; i < points.Length; i++)
        {
            var first = points[i];
            var last = points[(i + 1) % points.Length];
            lines.Add(((first[0], first[1]), (last[0], last[1])));
        }


        foreach (var corner in rect.Corners)
        {
            if (points.Any(p => p[0] == corner.X && p[1] == corner.Y))
            {
                continue;
            }

            var ls = lines.Where(l =>
            l.Start.Y != l.End.Y &&
            Math.Min(l.Start.Y, l.End.Y) <= corner.Y &&
            Math.Max(l.Start.Y, l.End.Y) >= corner.Y &&
            Math.Min(l.Start.X, l.End.X) <= corner.X).ToList();
            var sum = 0;

            for (int i = 0; i < ls.Count; i++)
            {
                var line = ls[i];
                if (i == 0)
                {
                    sum++;
                    continue;
                }

                var prevLine = ls[i - 1];
                var lStart = (line.Start.Y < line.End.Y) ? line.Start : line.End;
                var prevEnd = (prevLine.Start.Y < prevLine.End.Y) ? prevLine.End : prevLine.Start;
                var lEnd = (line.Start.Y < line.End.Y) ? line.End : line.Start;
                var prevStart = (prevLine.Start.Y < prevLine.End.Y) ? prevLine.Start : prevLine.End;

                if (corner.Y == lStart.Y && prevEnd.Y == lStart.Y)
                {
                    continue;
                }
                if (corner.Y == lEnd.Y && prevStart.Y == lEnd.Y)
                {
                    continue;
                }

                sum++;
            }

            if (sum % 2 == 0)
                return false;
        }
        return true;
    }

}

record Rectangle(int X1, int Y1, int X2, int Y2)
{

    public bool Intersects((int X, int Y) p1, (int X, int Y) p2)
    {
        if (p1.X != p2.X)
        {
            return p1.Y > Y1 && p1.Y < Y2 &&
            ((Math.Min(p1.X, p2.X) >= X1 && Math.Min(p1.X, p2.X) < X2 && Math.Max(p1.X, p2.X) > X2) ||
             (Math.Max(p1.X, p2.X) <= X2 && Math.Max(p1.X, p2.X) > X1 && Math.Min(p1.X, p2.X) < X1) ||
             (Math.Min(p1.X, p2.X) < X1 && Math.Max(p1.X, p2.X) > X2));
        }

        return p1.X > X1 && p1.X < X2 &&
        ((Math.Min(p1.Y, p2.Y) >= Y1 && Math.Min(p1.Y, p2.Y) < Y2 && Math.Max(p1.Y, p2.Y) > Y2) ||
         (Math.Max(p1.Y, p2.Y) <= Y2 && Math.Max(p1.Y, p2.Y) > Y1 && Math.Min(p1.Y, p2.Y) < Y1) ||
         (Math.Min(p1.Y, p2.Y) < Y1 && Math.Max(p1.Y, p2.Y) > Y2));
    }

    public (int X, int Y)[] Corners =>
    [
        (X1, Y1),
        (X1, Y2),
        (X2, Y1),
        (X2, Y2)
    ];

    public long Size => (long)(Math.Abs(X2 - X1) + 1) * (Math.Abs(Y2 - Y1) + 1);

    public override string ToString() => $"({X1},{Y1})-({X2},{Y2})";
}
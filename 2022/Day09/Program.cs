using Tools;

Day09 day = new();
day.OutputFirstStar();
day.OutputSecondStar();


public class Day09 : DayBase
{
    readonly Dictionary<string, int[]> DirectionMap = new()
    {
            {"R", new[]  {1, 0}},
            {"L", new[]  {-1, 0}},
            {"U", new[]  {0, 1}},
            {"D", new[]  {0, -1}},
    };
    readonly List<int[]> Offsets = new()
    {
            new int[] {0,0},
            new int[] {0,1},
            new int[] {1,1},
            new int[] {1,0},
            new int[] {1,-1},
            new int[] {0,-1},
            new int[] {-1,-1},
            new int[] {-1,0},
            new int[] {-1,1},
    };

    public Day09() : base("9")
    {

    }
    public override string FirstStar()
    {
        var data = GetRowData().Select(d => (Dir: d[0..1], Steps: int.Parse(d[2..])));
        var headPos = (X: 0, Y: 0);
        var tailPos = (X: 0, Y: 0);
        var tailSteps = new HashSet<(int, int)> { (0, 0) };

        foreach (var entry in data)
        {
            var inc = DirectionMap[entry.Dir];
            for (int i = 0; i < entry.Steps; i++)
            {
                var prev = (headPos.X, headPos.Y);
                headPos.X += inc[0];
                headPos.Y += inc[1];
                if (!Offsets.Any(o => tailPos.X + o[0] == headPos.X && tailPos.Y + o[1] == headPos.Y))
                {
                    tailPos.X = prev.X;
                    tailPos.Y = prev.Y;
                    tailSteps.Add((tailPos.X, tailPos.Y));
                }
            }
        }

        return tailSteps.Count.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(d => (Dir: d[0..1], Steps: int.Parse(d[2..])));
        var knots = Enumerable.Range(0, 10).Select(i => new int[] { 0, 0 }).ToList();
        var tailSteps = new HashSet<(int, int)> { (0, 0) };

        foreach (var entry in data)
        {
            var inc = DirectionMap[entry.Dir];
            for (int i = 0; i < entry.Steps; i++)
            {
                var prev = (X: 0, Y: 0);
                for (int k = 0; k < 10; k++)
                {
                    var knotPos = knots[k];
                    if (k == 0)
                    {
                        prev = (X: knots[k][0], Y: knots[k][1]);
                        knotPos[0] += inc[0];
                        knotPos[1] += inc[1];
                    }
                    else if (!Offsets.Any(o => knots[k][0] + o[0] == knots[k - 1][0] && knots[k][1] + o[1] == knots[k - 1][1]))
                    {
                        var temp = (X: knots[k][0], Y: knots[k][1]);
                        knotPos[0] = prev.X;
                        knotPos[1] = prev.Y;
                        if (Math.Abs(prev.X - temp.X) == 1 && Math.Abs(prev.Y - temp.Y) == 1 && k != 9)
                        {
                            prev.X = Math.Abs(knots[k + 1][0] - prev.X) > 0 ? knots[k + 1][0] + prev.X - temp.X : knots[k + 1][0];
                            prev.Y = Math.Abs(knots[k + 1][1] - prev.Y) > 0 ? knots[k + 1][1] + prev.Y - temp.Y : knots[k + 1][1];
                        }
                        else
                        {
                            prev = temp;
                        }

                        if (k == 9)
                            tailSteps.Add((knotPos[0], knotPos[1]));
                    }
                }
            }
        }

        return tailSteps.Count.ToString();
    }
}
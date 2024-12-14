using Tools;

var day = new Day14();
day.OutputSecondStar();

public class Day14 : DayBase
{

    private int XMax = 101;
    private int YMax = 103;
    // private int XMax = 11;
    // private int YMax = 7;
    public Day14() : base("14")
    {
    }

    public override string FirstStar()
    {
        var robots = GetRowData().Select(x => x.GetIntegers()).ToList();

        for (int i = 0; i < 100; i++)
        {
            Step(robots);
        }

        var q1 = robots.Count(r => r[0] < XMax / 2 && r[1] < YMax / 2);
        var q2 = robots.Count(r => r[0] > XMax / 2 && r[1] < YMax / 2);
        var q3 = robots.Count(r => r[0] < XMax / 2 && r[1] > YMax / 2);
        var q4 = robots.Count(r => r[0] > XMax / 2 && r[1] > YMax / 2);
        return (q1 * q2 * q3 * q4).ToString();
    }

    private void Step(List<int[]> robots)
    {
        foreach (var robot in robots)
        {
            var x = robot[0];
            var y = robot[1];
            var dx = robot[2];
            var dy = robot[3];

            x += dx;
            y += dy;
            if (x < 0)
            {
                x += XMax;
            }
            if (y < 0)
            {
                y += YMax;
            }
            if (x >= XMax)
            {
                x %= XMax;
            }
            if (y >= YMax)
            {
                y %= YMax;
            }
            robot[0] = x;
            robot[1] = y;
        }
    }

    public override string SecondStar()
    {
        var robots = GetRowData().Select(x => x.GetIntegers()).ToList();

        int i = 0;
        while (true)
        {
            if (Enumerable.Range(0, YMax).Any(y => robots.Count(r => r[1] == y) >= (XMax - 70))
            && Enumerable.Range(0, XMax).Any(x => robots.Count(r => r[0] == x) >= (YMax - 70)))
            {
                System.Console.WriteLine(i);
                for (int x = 0; x < XMax; x++)
                {
                    for (int y = 0; y < YMax; y++)
                    {
                        if (robots.Any(r => r[0] == x && r[1] == y))
                        {
                            System.Console.Write("#");
                        }
                        else
                        {
                            System.Console.Write(".");
                        }
                    }
                    System.Console.WriteLine();
                }
                System.Console.WriteLine();
                break;
            }
            Step(robots);
            i++;
        }
        return string.Empty;
    }
}
using Tools;

var day = new Day08();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day08 : DayBase
{

    int yMax;
    int xMax;

    public Day08() : base("8")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(x => x.Trim()).ToArray();
        yMax = data.Length;
        xMax = data[0].Length;
        var antennas = Enumerable.Range(0, yMax)
            .SelectMany(y => Enumerable.Range(0, xMax)
                .Where(x => data[y][x] != '.')
                .Select(x => (c: data[y][x], x, y))
            ).GroupBy(x => x.c)
            .ToDictionary(x => x.Key, x => x.Select(r => (r.y, r.x)).ToList());

        var nodes = new HashSet<(int x, int y)>();
        foreach (var antenna in antennas)
        {
            for (int i = 0; i < antenna.Value.Count - 1; i++)
            {
                for (int j = i + 1; j < antenna.Value.Count; j++)
                {
                    var (y1, x1) = antenna.Value[i];
                    var (y2, x2) = antenna.Value[j];
                    var dx = x2 - x1;
                    var dy = y2 - y1;
                    (int x, int y)[] ns = [
                        (x: x2 + dx, y: y2 + dy),
                        (x: x1 - dx, y: y1 - dy)
                    ];

                    ns.Where(InBounds).ToList().ForEach(x => nodes.Add(x));
                }
            }
        }

        return nodes.Count.ToString();
    }


    public override string SecondStar()
    {
        var data = GetRowData().Select(x => x.Trim()).ToArray();
        yMax = data.Length;
        xMax = data[0].Length;
        var antennas = Enumerable.Range(0, yMax)
            .SelectMany(y => Enumerable.Range(0, xMax)
                .Where(x => data[y][x] != '.')
                .Select(x => (c: data[y][x], x, y))
            ).GroupBy(x => x.c)
            .ToDictionary(x => x.Key, x => x.Select(r => (r.x, r.y)).ToList());

        var nodes = new HashSet<(int x, int y)>();
        foreach (var antenna in antennas)
        {
            for (int i = 0; i < antenna.Value.Count - 1; i++)
            {
                for (int j = i + 1; j < antenna.Value.Count; j++)
                {
                    var (y1, x1) = antenna.Value[i];
                    var (y2, x2) = antenna.Value[j];
                    nodes.Add((x1, y1));
                    nodes.Add((x2, y2));
                    var dx = x2 - x1;
                    var dy = y2 - y1;

                    (int x, int y)[] ns = [
                        (x: x2 + dx, y: y2 + dy),
                        (x: x1 - dx, y: y1 - dy)
                    ];

                    while (InBounds(ns[0]) || InBounds(ns[1]))
                    {
                        ns.Where(InBounds).ToList().ForEach(x => nodes.Add(x));
                        ns = [
                            (x: ns[0].x + dx, y: ns[0].y + dy),
                            (x: ns[1].x - dx, y: ns[1].y - dy)
                        ];
                    }
                }
            }
        }

        return nodes.Count.ToString();
    }

    private bool InBounds((int x, int y) a) =>
        a.x >= 0 && a.x < xMax && a.y >= 0 && a.y < yMax;
        
}
using Tools;

Day24 day = new();
day.OutputSecondStar();

public class Day24 : DayBase
{

    public Day24() : base("24")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var map = Enumerable.Range(1, data.Length - 2).Select(y => Enumerable.Range(1, data[0].Length - 2).Select(x => new List<char> { data[y][x] })).ToMultidimensionalArray() ?? new List<char>[0, 0];
        return Walk(map, false).Turns.ToString();

    }

    private (int Turns, List<char>[,] Map) Walk(List<char>[,] map, bool back)
    {
        var h = map.GetLength(0);
        var w = map.GetLength(1);
        var offsets = new List<(int Y, int X)> { (0, -1), (0, 1), (1, 0), (-1, 0), (0, 0) };
        var paths = new List<(int Y, int X, int C)>();

        var done = false;
        var round = 0;
        while (!done)
        {
            var newMap = Enumerable.Range(0, h).Select(_ => Enumerable.Range(0, w).Select(__ => new List<char>())).ToMultidimensionalArray()!;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    foreach (var p in map[y, x])
                    {
                        if (p == '<')
                            newMap[y, x - 1 < 0 ? w - 1 : x - 1].Add('<');
                        else if (p == '>')
                            newMap[y, (x + 1) % w].Add('>');
                        else if (p == '^')
                            newMap[y - 1 < 0 ? h - 1 : y - 1, x].Add('^');
                        else if (p == 'v')
                            newMap[(y + 1) % h, x].Add('v');

                        if (!newMap[y, x].Contains('.'))
                            newMap[y, x].Add('.');

                    }

                }
            }
            map = newMap;
            var newPaths = new List<(int Y, int X, int C)>();
            round++;
            foreach (var p in paths)
            {
                newPaths.AddRange(offsets.Where(o => p.Y + o.Y >= 0 && p.Y + o.Y < h && p.X + o.X >= 0 && p.X + o.X < w && map[p.Y + o.Y, p.X + o.X].Count == 1).Select(o => (p.Y + o.Y, p.X + o.X, p.C + 1)));
            }
            if (!back)
            {
                if (map[0, 0].Count == 1 && !newPaths.Any(p => p.Y == 0 && p.X == 0))
                    newPaths.Add((0, 0, round));
            }
            else
            {
                if (map[h - 1, w - 1].Count == 1 && !newPaths.Any(p => p.Y == h - 1 && p.X == w - 1))
                    newPaths.Add((h - 1, w - 1, round));
            }
            paths = newPaths.Distinct().ToList();

            done = back ?
             paths.Any(p => p.Y == 0 && p.X == 0) :
             paths.Any(p => p.Y == h - 1 && p.X == w - 1);
        }

        var lastMap = Enumerable.Range(0, h).Select(_ => Enumerable.Range(0, w).Select(__ => new List<char>())).ToMultidimensionalArray()!;
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                foreach (var p in map[y, x])
                {
                    if (p == '<')
                        lastMap[y, x - 1 < 0 ? w - 1 : x - 1].Add('<');
                    else if (p == '>')
                        lastMap[y, (x + 1) % w].Add('>');
                    else if (p == '^')
                        lastMap[y - 1 < 0 ? h - 1 : y - 1, x].Add('^');
                    else if (p == 'v')
                        lastMap[(y + 1) % h, x].Add('v');

                    if (!lastMap[y, x].Contains('.'))
                        lastMap[y, x].Add('.');

                }

            }
        }
        map = lastMap;

        return back ?
        (paths.First(p => p.Y == 0 && p.X == 0).C + 1, map) :
        (paths.First(p => p.Y == h - 1 && p.X == w - 1).C + 1, map);
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var map = Enumerable.Range(1, data.Length - 2).Select(y => Enumerable.Range(1, data[0].Length - 2).Select(x => new List<char> { data[y][x] })).ToMultidimensionalArray() ?? new List<char>[0, 0];
        var t1 = 0;
        var t2 = 0;
        var t3 = 0;
        (t1, map) = Walk(map, false);
        (t2, map) = Walk(map, true);
        (t3, map) = Walk(map, false);

        return (t1 + t2 + t3).ToString();
    }
}
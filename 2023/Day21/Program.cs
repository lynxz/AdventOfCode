using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using Tools;

Day21 day = new Day21();
day.OutputSecondStar();

public class Day21 : DayBase
{
    public Day21() : base("21")
    {
    }

    List<(int dy, int dx)> directions = new List<(int dy, int dx)>()
    {
        (1, 0),
        (-1, 0),
        (0, 1),
        (0, -1)
    };

    public override string FirstStar()
    {
        var map = GetRowData().ToMultidimensionalArray();
        var start = Enumerable.Range(0, map.GetLength(0)).SelectMany(y => Enumerable.Range(0, map.GetLength(1)).Select(x => (y, x))).Single(c => map[c.y, c.x] == 'S');
        var steps = 64;

        var pos = new HashSet<(int y, int x)> { start };

        for (int i = 0; i < steps; i++)
        {
            var newPos = new HashSet<(int y, int x)>();
            foreach (var p in pos)
            {
                directions.Select(d => (y: p.y + d.dy, x: p.x + d.dx))
                    .Where(c => c.y >= 0 && c.y < map.GetLength(0) && c.x >= 0 && c.x < map.GetLength(1) && map[c.y, c.x] != '#')
                    .ToList()
                    .ForEach(c => newPos.Add(c));
            }

            pos = newPos;
        }


        return pos.Count().ToString();
    }

    public override string SecondStar()
    {
        var map = GetRowData().ToMultidimensionalArray();
        var start = Enumerable.Range(0, map.GetLength(0)).SelectMany(y => Enumerable.Range(0, map.GetLength(1)).Select(x => (y, x))).Single(c => map[c.y, c.x] == 'S');
        var max = map.GetLength(0);
        var half = max / 2;

        var totalSteps = 26501365;
        var frames = (totalSteps - 65) / 131;
        var rings = Enumerable.Range(0, frames).Select(i => i == 0L ? 1 : 4L * i).ToList();
        var zigFrames = Enumerable.Range(0, rings.Count).Where(i => i % 2 == 0).Select(i => rings[i]).Sum();
        var zagFrames = Enumerable.Range(0, rings.Count).Where(i => i % 2 == 1).Select(i => rings[i]).Sum();
        var smallSteps = frames;
        var bigSteps = smallSteps - 1;

        var tiles = new HashSet<(int y, int x)> { start };
        for (int i = 0; i < 2 * 131 + 65; i++)
        {
            var newPos = new HashSet<(int y, int x)>();
            foreach (var p in tiles)
            {
                var rawPos = directions.Select(d => (y: p.y + d.dy, x: p.x + d.dx));
                foreach (var (y, x) in rawPos)
                {
                    var relY = y >= 0 ? y % map.GetLength(0) : (map.GetLength(0) - (Math.Abs(y) % map.GetLength(0))) % map.GetLength(0);
                    var relX = x >= 0 ? x % map.GetLength(1) : (map.GetLength(1) - (Math.Abs(x) % map.GetLength(1))) % map.GetLength(1);
                    if (map[relY, relX] != '#')
                    {
                        newPos.Add((y, x));
                    }
                }
            }
            tiles = newPos;
        }
        var parts = new int[5, 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                parts[i, j] = Enumerable.Range(i * max - 2 * max, max).Sum(y => Enumerable.Range(j * max - 2 * max, max).Sum(x => tiles.Contains((y, x)) ? 1 : 0));
            }
        }

        var sum = zagFrames * parts[2, 1] + 
            zigFrames * parts[2, 2] + 
            parts[0, 2] + parts[2, 0] + parts[4, 2] + parts[2, 4] + 
            smallSteps * parts[0, 1] + smallSteps * parts[0, 3] + smallSteps * parts[4, 1] + smallSteps * parts[4, 3] + 
            bigSteps * parts[1, 1] + bigSteps * parts[1, 3] + bigSteps * parts[3, 1] + bigSteps * parts[3, 3];

        return sum.ToString();
    }
}
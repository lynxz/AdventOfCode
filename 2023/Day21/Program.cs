using System.Security.Cryptography.X509Certificates;
using Tools;

Day21 day = new Day21();
day.OutputFirstStar();

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
        var steps = 500000;

        var pos = new HashSet<(int y, int x)>();
        pos.Add(start);

        for(int i = 0; i < steps; i++)
        {
            var newPos = new HashSet<(int y, int x)>();
            foreach(var p in pos)
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
        var steps = 64;

        var pos = new HashSet<(int y, int x)>();
        pos.Add(start);

        for(int i = 0; i < steps; i++)
        {
            var newPos = new HashSet<(int y, int x)>();
            foreach(var p in pos)
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
}
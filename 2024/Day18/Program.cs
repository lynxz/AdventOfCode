using System.Data;
using Tools;

var day = new Day18();
day.OutputSecondStar();

public class Day18 : DayBase
{
    private int XMax = 71;
    private int YMax = 71;

    public Day18() : base("18")
    {
    }

    public override string FirstStar()
    {
        var coords = GetRowData()
            .Select(r => r.GetIntegers())
            .Select(c => (x: c[0], y: c[1]))
            .Take(1024)
            .ToList();

        var map = Enumerable.Range(0, YMax)
            .Select(y => Enumerable.Range(0, XMax).Select(x => coords.Contains((x, y)) ? '#' : '.'))
            .ToMultidimensionalArray()!;

        map.Print();

        var result = AStar(map, (0, 0), (XMax - 1, YMax - 1));

        return result.ToString();
    }

    public override string SecondStar()
    {
        var coords = GetRowData()
            .Select(r => r.GetIntegers())
            .Select(c => (x: c[0], y: c[1]))
            .ToArray();

        var sc = coords
            .Take(1023)
            .ToList();

        var map = Enumerable.Range(0, YMax)
            .Select(y => Enumerable.Range(0, XMax).Select(x => sc.Contains((x, y)) ? '#' : '.'))
            .ToMultidimensionalArray()!;

        for (int i = 1024; i < coords.Length; i++)
        {
            var c = coords[i];
            sc.Add(c);
            map[c.y, c.x] = '#';

            var result = AStar(map, (0, 0), (XMax - 1, YMax - 1));
            if (result == -1)
            {
                map.Print();
                return $"{c.x},{c.y}";
            }
        }

        return string.Empty;
    }

    public int AStar(char[,] graph, (int x, int y) start, (int x, int y) end)
    {
        var open = new List<(int x, int y)>();
        var closed = new List<(int x, int y)>();
        var gScore = new int[XMax, YMax];
        var fScore = new int[XMax, YMax];
        var cameFrom = new (int x, int y)[XMax, YMax];

        open.Add(start);
        gScore[start.x, start.y] = 0;
        fScore[start.x, start.y] = Heuristic(start, end);

        while (open.Any())
        {
            var current = open.OrderBy(o => fScore[o.x, o.y]).First();

            if (current == end)
            {
                return ReconstructPath(cameFrom, current);
            }

            open.Remove(current);
            closed.Add(current);

            foreach (var neighbor in GetNeighbors(graph, current))
            {
                if (closed.Contains(neighbor))
                {
                    continue;
                }

                var tentativeGScore = gScore[current.x, current.y] + 1;

                if (!open.Contains(neighbor))
                {
                    open.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor.x, neighbor.y])
                {
                    continue;
                }

                cameFrom[neighbor.x, neighbor.y] = current;
                gScore[neighbor.x, neighbor.y] = tentativeGScore;
                fScore[neighbor.x, neighbor.y] = gScore[neighbor.x, neighbor.y] + Heuristic(neighbor, end);
            }
        }

        return -1;
    }

    private int ReconstructPath((int x, int y)[,] cameFrom, (int x, int y) current)
    {
        var totalPath = 0;
        while (cameFrom[current.x, current.y] != (0, 0))
        {
            totalPath++;
            current = cameFrom[current.x, current.y];
        }

        return totalPath + 1;
    }

    private IEnumerable<(int x, int y)> GetNeighbors(char[,] graph, (int x, int y) current)
    {
        foreach (var (dx, dy) in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
        {
            var x = current.x + dx;
            var y = current.y + dy;

            if (x < 0 || x >= XMax || y < 0 || y >= YMax || graph[y, x] == '#')
            {
                continue;
            }

            yield return (x, y);
        }
    }

    private int Heuristic((int x, int y) start, (int x, int y) end)
    {
        var a = Math.Abs(start.x - end.x);
        var b = Math.Abs(start.y - end.y);
        return Convert.ToInt32(Math.Sqrt(a * a + b * b));
    }
}
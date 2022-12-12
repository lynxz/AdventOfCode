using Tools;

Day12 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day12 : DayBase
{
    public Day12() : base("12")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var map = data.Select(r => r.Select(c => c != 'S' && c != 'E' ? c - 'a' : c == 'S' ? 0 : 'z' - 'a')).ToMultidimensionalArray() ?? new int[0, 0];
        var source = Enumerable.Range(0, data.Length)
            .SelectMany(y => Enumerable.Range(0, data[y].Length).Select(x => (Y: y, X: x))).First(p => data[p.Y][p.X] == 'S');
        var end = Enumerable.Range(0, data.Length)
            .SelectMany(y => Enumerable.Range(0, data[y].Length).Select(x => (Y: y, X: x))).First(p => data[p.Y][p.X] == 'E');

        var sum = Dijkstra(map, source, end, new Dictionary<(int Y, int X), int>());
        return sum.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var map = data.Select(r => r.Select(c => c != 'S' && c != 'E' ? c - 'a' : c == 'S' ? 0 : 'z' - 'a')).ToMultidimensionalArray() ?? new int[0, 0];
        var sources = Enumerable.Range(0, data.Length)
            .SelectMany(y => Enumerable.Range(0, data[y].Length).Select(x => (Y: y, X: x))).Where(p => data[p.Y][p.X] == 'a' || data[p.Y][p.X] == 'S');
        var end = Enumerable.Range(0, data.Length)
            .SelectMany(y => Enumerable.Range(0, data[y].Length).Select(x => (Y: y, X: x))).First(p => data[p.Y][p.X] == 'E');

        var shortPath = new Dictionary<(int Y, int X), int>();
        var sum = sources.Min(s => Dijkstra(map, s, end, shortPath));
        return sum.ToString();
    }

    static int Dijkstra(int[,] graph, (int Y, int X) source, (int Y, int X) target, Dictionary<(int Y, int X), int> shortPath)
    {
        var nodes = Enumerable.Range(0, graph.GetLength(0)).SelectMany(y => Enumerable.Range(0, graph.GetLength(1)).Select(x => (Y: y, X: x))).ToList();

        var dist = nodes.ToDictionary(k => k, _ => 10000);
        var prev = new Dictionary<(int Y, int X), (int Y, int X)>();
        dist[source] = 0;

        var offsets = new[] {
            (Y: -1, X: 0),
            (Y: 1, X: 0),
            (Y: 0, X: -1),
            (Y: 0, X: 1),
        };

        while (nodes.Count > 0)
        {
            var u = nodes.OrderBy(n => dist[n]).First();
            if (shortPath.TryGetValue(u, out int val)) {
                while(prev.ContainsKey(u)) {
                    val++;
                    u = prev[u];
                }
                return val;
            }
            nodes.Remove(u);
            if (u.Y == target.Y && u.X == target.X)
            {
                var sum = 0;
                while(prev.ContainsKey(u)) {
                    shortPath[u] = sum;
                    sum++;
                    u = prev[u];
                }

                return sum;
            }

            var p = offsets.Select(o => (Y: u.Y + o.Y, X: u.X + o.X));
            var neighbors = p
                .Where(n => nodes.Any(o => n.Y == o.Y && n.X == o.X) && (graph[n.Y, n.X] - graph[u.Y, u.X] < 2))
                .ToList();
            foreach (var v in neighbors)
            {
                var alt = dist[u] + 1;
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        throw new InvalidOperationException("Never reached target");
    }

}
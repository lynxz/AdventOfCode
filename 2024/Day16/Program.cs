using Tools;

var day = new Day16();
day.PostFirstStar();
day.OutputSecondStar();

public class Day16 : DayBase
{
    public Day16() : base("16")
    {
    }

    public override string FirstStar()
    {
        var graph = new Dictionary<(int x, int y), List<(int x, int y)>>();
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var start = Enumerable.Range(0, data.Length)
            .SelectMany(y => Enumerable.Range(0, data[0].Length).Select(x => (x, y)))
            .First(p => data[p.y][p.x] == '.');
        var end = (x: 0, y: 0);

        var stack = new Stack<(int x, int y)>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (graph.ContainsKey(current))
            {
                continue;
            }

            graph[current] = new List<(int x, int y)>();
            foreach (var direction in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
            {
                var next = (x: current.x + direction.Item1, y: current.y + direction.Item2);
                if (next.x < 0 || next.x >= data[0].Length || next.y < 0 || next.y >= data.Length)
                {
                    continue;
                }

                if (data[next.y][next.x] != '#')
                {
                    graph[current].Add(next);
                    stack.Push(next);
                }
                if (data[next.y][next.x] == 'S')
                {
                    start = (next.x, next.y);
                }
                if (data[next.y][next.x] == 'E')
                {
                    end = (next.x, next.y);
                }
            }
        }

        var (cost, dist, prev) = Djikstra(graph, start, end);
        return cost.ToString();
    }

    private static (int cost, Dictionary<(int x, int y), int> dist, Dictionary<(int x, int y), (int x, int y)> prev) Djikstra(Dictionary<(int x, int y), List<(int x, int y)>> graph, (int x, int y) start, (int x, int y) end)
    {
        var distances = new Dictionary<(int x, int y), int>();
        var visited = new HashSet<(int x, int y)>();
        var queue = new PriorityQueue<(int x, int y, int dx, int dy), int>();
        queue.Enqueue((start.x, start.y, 1, 0), 0);
        distances[start] = 0;
        var prev = new Dictionary<(int x, int y), (int x, int y)>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var pos = (current.x, current.y);
            if (visited.Contains(pos))
            {
                continue;
            }

            visited.Add(pos);
            foreach (var neighbour in graph[pos])
            {
                var (cost, (dx, dy)) = CalculateDistance(pos, neighbour, current.dx, current.dy);
                if (!distances.ContainsKey(neighbour) || distances[neighbour] > distances[pos] + cost)
                {
                    prev[neighbour] = pos;
                    distances[neighbour] = distances[pos] + cost;
                }
                queue.Enqueue((neighbour.x, neighbour.y, dx, dy), distances[neighbour]);
            }
        }

        return (distances[end], distances, prev);
    }

    static (int cost, (int dx, int dy)) CalculateDistance((int x, int y) start, (int x, int y) end, int dx, int dy)
    {
        var sx = end.x - start.x;
        var sy = end.y - start.y;
        if (sx == dx && sy == dy)
        {
            return (1, (sx, sy));
        }
        return (1001, (sx, sy));
    }

    public override string SecondStar()
    {
        var graph = new Dictionary<(int x, int y), List<(int x, int y)>>();
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var start = Enumerable.Range(0, data.Length)
            .SelectMany(y => Enumerable.Range(0, data[0].Length).Select(x => (x, y)))
            .First(p => data[p.y][p.x] == '.');
        var end = (x: 0, y: 0);

        var stack = new Stack<(int x, int y)>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (graph.ContainsKey(current))
            {
                continue;
            }

            graph[current] = new List<(int x, int y)>();
            foreach (var direction in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
            {
                var next = (x: current.x + direction.Item1, y: current.y + direction.Item2);
                if (next.x < 0 || next.x >= data[0].Length || next.y < 0 || next.y >= data.Length)
                {
                    continue;
                }

                if (data[next.y][next.x] != '#')
                {
                    graph[current].Add(next);
                    stack.Push(next);
                }
                if (data[next.y][next.x] == 'S')
                {
                    start = (next.x, next.y);
                }
                if (data[next.y][next.x] == 'E')
                {
                    end = (next.x, next.y);
                }
            }
        }

        var (cost, dist, prev) = Djikstra(graph, start, end);
        var ns = new Stack<((int x, int y), (int dx, int dy), int cost, HashSet<(int x, int y)> visited)>();

        ns.Push((start, (0, 1), 0, new HashSet<(int x, int y)>()));
        var allVisited = new List<(int x, int y)>();
        var minScore = new Dictionary<(int x, int y, int dx, int dy), int>();   

        while (ns.Count > 0) {
            var (coord, dir, oc, ov) = ns.Pop();
            if (ov.Contains(coord))
            {
                continue;
            }
            ov.Add(coord);
            if (coord == end)
            {
                allVisited.AddRange(ov);
                continue;
            }
            foreach (var node in graph[coord])
            {
                if (ov.Contains(node))
                {
                    continue;
                }
                var (c, (dx, dy)) = CalculateDistance(coord, node, dir.dx, dir.dy);
                if (c + oc > cost)
                {
                    continue;
                }
                var nh = new HashSet<(int x, int y)>(ov);
                if (!minScore.ContainsKey((node.x, node.y, dx, dy)) || minScore[(node.x, node.y, dx, dy)] >= c + oc)
                {
                    minScore[(node.x, node.y, dx, dy)] = c + oc;
                    ns.Push((node, (dx, dy), c + oc, nh));
                }
                
            }
        }

        return allVisited.Distinct().Count().ToString();
    }
}
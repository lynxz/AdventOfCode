using System.Data;
using Tools;

Day23 day = new Day23();
day.OutputSecondStar();

public class Day23 : DayBase
{
    public Day23() : base("23")
    {
    }

    public override string FirstStar()
    {
        var map = GetRowData().ToMultidimensionalArray();
        var dir = new (int dy, int dx)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };

        var graph = new Dictionary<(int y, int x), List<(int y, int x)>>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (!graph.ContainsKey((y, x)) && map[y, x] != '#')
                {
                    graph.Add((y, x), new List<(int y, int x)>());
                }

                if (map[y, x] == '>')
                {
                    graph[(y, x)].Add((y, x + 1));
                }
                else if (map[y, x] == '<')
                {
                    graph[(y, x)].Add((y, x - 1));
                }
                else if (map[y, x] == '^')
                {
                    graph[(y, x)].Add((y - 1, x));
                }
                else if (map[y, x] == 'v')
                {
                    graph[(y, x)].Add((y + 1, x));
                }
                else if (map[y, x] == '.')
                {
                    var neighbors = dir.Select(n => (y: y + n.dy, x: x + n.dx))
                    .Where(n => n.y >= 0 && n.y < map.GetLength(0) && n.x >= 0 && n.x < map.GetLength(1))
                    .Where(n => map[n.y, n.x] != '#');
                    foreach (var n in neighbors)
                    {
                        if (!graph[(y, x)].Contains(n))
                        {
                            graph[(y, x)].Add(n);
                        }
                    }
                }
            }
        }

        var result = WalkPaths(graph, (0, 1), (map.GetLength(0) - 1, map.GetLength(1) - 2));

        return result.ToString();
    }

    public override string SecondStar()
    {
        var map = GetRowData().ToMultidimensionalArray();
        var dir = new (int dy, int dx)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };

        var graph = new Dictionary<(int y, int x), List<(int y, int x)>>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == '#')
                    continue;

                if (!graph.ContainsKey((y, x)))
                {
                    graph.Add((y, x), new List<(int y, int x)>());
                }

                var neighbors = dir.Select(n => (y: y + n.dy, x: x + n.dx))
                    .Where(n => n.y >= 0 && n.y < map.GetLength(0) && n.x >= 0 && n.x < map.GetLength(1))
                    .Where(n => map[n.y, n.x] != '#');
                foreach (var n in neighbors)
                {
                    if (!graph[(y, x)].Contains(n))
                    {
                        graph[(y, x)].Add(n);
                    }
                }
            }
        }

        var conGraph = CondenseGraph(graph);
        var result = Dfs(conGraph, (0, 1), (map.GetLength(0) - 1, map.GetLength(1) - 2), new HashSet<(int y, int x)>(), 0);

        return result.ToString();
    }

    int Dfs(Dictionary<(int y, int x), List<((int y, int x) pos, int cost)>> graf, (int y, int x) start, (int y, int x) end, HashSet<(int y, int x)> visited, int n)
    {
        if (start == end)
            return n;

        var max = 0;
        foreach (var (pos, cost) in graf[start])
        {
            if (visited.Add(pos))
            {
                max = Math.Max(max, Dfs(graf, pos, end, visited, n + cost));
                visited.Remove(pos);
            }
        }

        return max;
    }

    Dictionary<(int y, int x), List<((int y, int x) pos, int cost)>> CondenseGraph(Dictionary<(int y, int x), List<(int y, int x)>> graph)
    {
        var nodes = graph.Where(n => n.Value.Count != 2).Select(n => n.Key).ToList();
        var conGraph = nodes.ToDictionary(n => n, n => new List<((int y, int x) pos, int cost)>());

        foreach(var node in nodes) {
            var queue = new Queue<(int y, int x)>();
            queue.Enqueue(node);
            var visited = new HashSet<(int y, int x)>();
            var cost = 0;

            while (queue.Count > 0)
            {
                var count = queue.Count;
                for (int i = 0; i < count; i++)
                {
                    var pos = queue.Dequeue();
                    
                    if (node != pos && nodes.Contains(pos))
                    {
                        conGraph[node].Add((pos, cost));
                        continue;
                    }

                    foreach (var n in graph[pos])
                        if (visited.Add(n))
                            queue.Enqueue(n);
                        
                }
                cost++;
            }
        }        

        return conGraph;
    }


    public int WalkPaths(Dictionary<(int y, int x), List<(int y, int x)>> graph, (int y, int x) start, (int y, int x) end)
    {
        var memo = new Dictionary<(int y, int x), List<(int y, int x)>>();
        var stack = new Stack<((int y, int x) pos, List<(int y, int x)> prev)>();
        var queue = new PriorityQueue<((int y, int x) pos, List<(int y, int x)> prev), int>();
        var max = 0;
        stack.Push((start, new List<(int y, int x)>()));
        queue.Enqueue((start, new List<(int y, int x)>()), 0);

        while (queue.Count > 0)
        {
            var (pos, prev) = queue.Dequeue();
            prev.Add(pos);

            if (pos == end)
            {
                if (prev.Count > max)
                {
                    max = prev.Count;
                }
                continue;
            }

            
            foreach (var v in graph[pos].Where(v => !prev.Contains(v)))
            {
                queue.Enqueue((v, prev.ToList()), -prev.Count);
            }
        }

        return max-1;
    }

    public Dictionary<(int y, int x), List<(int y, int x)>> Backtrack(Dictionary<(int y, int x), List<(int y, int x)>> graph, (int y, int x) start)
    {
        var stack = new Stack<((int y, int x) pos, List<(int y, int x)> prev)>();
        var map = new Dictionary<(int y, int x), List<(int y, int x)>>();
        stack.Push((start, new List<(int y, int x)>()));

        while (stack.Count > 0)
        {
            var (pos, prev) = stack.Pop();
            prev.Add(pos);

            if (graph[pos].Count > 2)
            {
                if (!map.ContainsKey(pos))
                {
                    map.Add(pos, ((IEnumerable<(int, int)>)prev).Reverse().ToList());
                    foreach (var v in graph[pos].Where(v => !prev.Contains(v)))
                    {
                        stack.Push((v, new List<(int y, int x)> { pos }));
                    }
                    continue;
                }

            }

            foreach (var v in graph[pos].Where(v => !prev.Contains(v)))
            {
                stack.Push((v, prev.ToList()));
            }
        }

        return map; 
    }
}
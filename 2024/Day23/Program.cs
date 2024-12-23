using Tools;

var day = new Day23();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day23 : DayBase
{
    public Day23() : base("23")
    {
    }

    public override string FirstStar()
    {
        var rows = GetRowData().Select(r => r.Trim()).ToArray();
        var graph = CreateGraph(rows);
        var permTable = new HashSet<string>();
        List<List<string>> debug = new();
        var result = 0;

        foreach (var key in graph.Keys)
        {
            var cycles = FindCycles(key, graph);
            foreach (var cycle in cycles)
            {
                if (!cycle.Any(c => c.StartsWith("t")))
                {
                    continue;
                }
                var combinations = Combinations(cycle);
                if (!combinations.Any(c => permTable.Contains(c)))
                {
                    result++;
                    debug.Add(cycle);
                    combinations.ForEach(c => permTable.Add(c));
                }
            }
        }

        debug.ForEach(d => Console.WriteLine(string.Join(",", d)));

        return result.ToString();
    }

    List<string> Combinations(List<string> cycle)
    {
        return [
            string.Join("", [cycle[0], cycle[1], cycle[2]]),
            string.Join("", [cycle[0], cycle[2], cycle[1]]),
            string.Join("", [cycle[1], cycle[0], cycle[2]]),
            string.Join("", [cycle[1], cycle[2], cycle[0]]),
            string.Join("", [cycle[2], cycle[0], cycle[1]]),
            string.Join("", [cycle[2], cycle[1], cycle[0]])
        ];
    }

    List<List<string>> FindCycles(string node, Dictionary<string, List<string>> graph)
    {
        var cycles = new List<List<string>>();
        var visited = new HashSet<string>();
        var stack = new Stack<(string node, List<string> v)>();
        stack.Push((node, new List<string> { node }));
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            foreach (var neighbor in graph[current.node])
            {
                if (current.v.Count > 2 && current.v[0] == neighbor)
                {
                    cycles.Add(current.v);
                    continue;
                }
                if (current.v.Count < 3 && !current.v.Any(c => c == neighbor))
                {
                    stack.Push((neighbor, new List<string>(current.v) { neighbor }));
                }
            }
        }
        return cycles;
    }

    private static Dictionary<string, List<string>> CreateGraph(string[] rows)
    {
        var graph = new Dictionary<string, List<string>>();
        foreach (var row in rows)
        {
            var parts = row.Split("-");
            if (!graph.ContainsKey(parts[0]))
            {
                graph[parts[0]] = [];
            }
            if (!graph.ContainsKey(parts[1]))
            {
                graph[parts[1]] = [];
            }
            graph[parts[0]].Add(parts[1]);
            graph[parts[1]].Add(parts[0]);
        }
        return graph;
    }

    public override string SecondStar()
    {
        var rows = GetRowData().Select(r => r.Trim()).ToArray();
        var graph = CreateGraph(rows);
        var largest = new List<string>();
        foreach (var key in graph.Keys)
        {
            var set = new List<string> { key };
            foreach (var neighbor in graph[key])
            {
                if (set.All(n => graph[neighbor].Contains(n)))
                {
                    set.Add(neighbor);
                }
            }
            if (set.Count > largest.Count)
            {
                largest = set;
            }
        }

        return string.Join(",", largest.OrderBy(l => l));
    }
}
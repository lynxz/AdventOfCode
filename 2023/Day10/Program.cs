using Tools;

Day10 day10 = new();
day10.OutputFirstStar();
day10.OutputSecondStar();

public class Day10 : DayBase
{
    public Day10() : base("10")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(c => c).ToMultidimensionalArray();
        var start = Enumerable.Range(0, data.GetLength(0)).Select(y => Enumerable.Range(0, data.GetLength(1)).Select(x => (y, x))).SelectMany(x => x).First(c => data[c.y, c.x] == 'S');
        var graph = GenerateGraph(data, start);

        return (graph.Count / 2).ToString();
    }

    
    public override string SecondStar()
    {
        var data = GetRowData().Select(c => c).ToMultidimensionalArray();
        var start = Enumerable.Range(0, data.GetLength(0)).Select(y => Enumerable.Range(0, data.GetLength(1)).Select(x => (y, x))).SelectMany(x => x).First(c => data[c.y, c.x] == 'S');
        var graph = GenerateGraph(data, start);

        var n = graph[start].OrderBy(n => n.y).ThenBy(n => n.x).ToList();
        if (n.First().x == n.Last().x)
            data[start.y, start.x] = '-';
        else if (n.First().y == n.Last().y)
            data[start.y, start.x] = '|';
        else if (n.First().y > n.Last().y && n.First().x < n.Last().x)
            data[start.y, start.x] = 'J';
        else if (n.First().y > n.Last().y && n.First().x > n.Last().x)
            data[start.y, start.x] = 'L';
        else if (n.First().y < n.Last().y && n.First().x < n.Last().x)
            data[start.y, start.x] = '7';
        else 
            data[start.y, start.x] = 'F';

        var inside = new HashSet<(int y, int x)>();

        start = Enumerable.Range(0, data.GetLength(0)).Select(y => Enumerable.Range(0, data.GetLength(1)).Select(x => (y, x))).SelectMany(x => x).First(c => graph.ContainsKey(c) && data[c.y, c.x] == 'F');
        var ins = graph[start].Last().x > start.x ? (y: 1, x: 0) : (y: 0, x: 1);
        var node = start;

        do
        {
            var inNode = (node.y + ins.y, node.x + ins.x);
            if (!graph.ContainsKey(inNode))
            {
                inside.Add(inNode);
            }

            var next = graph[node].Last();
            inNode = (next.y + ins.y, next.x + ins.x);
            if (!graph.ContainsKey(inNode))
            {
                inside.Add(inNode);
            }

            if (data[next.y, next.x] == 'F')
                ins = (ins.x, ins.y);
            else if (data[next.y, next.x] == '7')
                ins = (-1 * ins.x, -1 * ins.y);
            else if (data[next.y, next.x] == 'J')
                ins = (ins.x, ins.y);
            else if (data[next.y, next.x] == 'L')
                ins = (-1 * ins.x, -1 * ins.y);

            node = next;
            
        } while (node != start);

        inside.ToList().ForEach(i => Fill(inside, graph, i));

        return inside.Count.ToString();
    }

    private void Fill(HashSet<(int y, int x)> inside, Dictionary<(int y, int x), List<(int y, int x)>> graph, (int y, int x) node)
    {
        if (graph.ContainsKey(node))
            return;

        var neighbors = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
        foreach (var n in neighbors.Select(n => (node.y + n.Item1, node.x + n.Item2)))
        {
            if (!inside.Contains(n) && !graph.ContainsKey(n))
            {
                inside.Add(n);
                Fill(inside, graph, n);
            }
        }
    }

    private static Dictionary<(int y, int x), List<(int y, int x)>> GenerateGraph(char[,]? data, (int y, int x) start)
    {
        var graph = new Dictionary<(int y, int x), List<(int y, int x)>>
        {
            { start, new List<(int y, int x)>() }
        };

        var node = start;
        var neighbors = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
        var connected = neighbors.Where(n =>
        {
            return n switch
            {
                (0, 1) => node.x + 1 < data.GetLength(1) && "-J7".Contains(data[node.y, node.x + 1]),
                (0, -1) => node.x - 1 >= 0 && "-FL".Contains(data[node.y, node.x - 1]),
                (1, 0) => node.y + 1 < data.GetLength(0) && "|LJ".Contains(data[node.y + 1, node.x]),
                (-1, 0) => node.y - 1 >= 0 && "|7F".Contains(data[node.y - 1, node.x]),
                _ => false
            };
        }).Select(n => (y: node.y + n.Item1, x: node.x + n.Item2)).ToList();

        node = connected.First();
        graph[start].Add(node);
        graph.Add(node, new List<(int y, int x)> { start });

        do
        {
            var pn = data[node.y, node.x] switch
            {
                '-' => new[] { (node.y, node.x + 1), (node.y, node.x - 1) },
                '|' => new[] { (node.y + 1, node.x), (node.y - 1, node.x) },
                '7' => new[] { (node.y, node.x - 1), (node.y + 1, node.x) },
                'J' => new[] { (node.y, node.x - 1), (node.y - 1, node.x) },
                'L' => new[] { (node.y, node.x + 1), (node.y - 1, node.x) },
                'F' => new[] { (node.y, node.x + 1), (node.y + 1, node.x) },
                _ => throw new Exception("Unknown node")
            };

            if (pn.Any(n => !graph.ContainsKey(n)))
            {
                var next = pn.Single(n => !graph.ContainsKey(n));
                graph[node].Add(next);
                graph.Add(next, new List<(int y, int x)> { node });
                node = next;
            }
            else
            {
                graph[node].Add(start);
                graph[start].Insert(0, node);
                node = start;
            }
        } while (node != start);

        return graph;
    }

}
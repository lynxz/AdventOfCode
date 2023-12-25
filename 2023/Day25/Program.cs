using System.Text.RegularExpressions;
using Tools;

Day25 day = new();
day.OutputFirstStar();

public class Day25 : DayBase
{
    public Day25() : base("25")
    {
    }

    public override string FirstStar()
    {
        var regex = new Regex(@"[a-z]{3}");
        var data = GetRowData();
        var nodes = data.SelectMany(l => regex.Matches(l).Select(m => m.Value)).Distinct().ToArray();
        var graph = new int[nodes.Length, nodes.Length];
        foreach (var line in data)
        {
            var matches = regex.Matches(line);
            var from = Array.IndexOf(nodes, matches[0].Value);
            foreach (var match in matches.Skip(1))
            {
                var to = Array.IndexOf(nodes, match.Value);
                graph[from, to] = 1;
                graph[to, from] = 1;
            }
        }

        int[,] rGraph = new int[graph.GetLength(0),graph.GetLength(1)]; 
        for (int i = 0; i < graph.GetLength(0); i++)
        {
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                rGraph[i, j] = graph[i, j];
            }
        }

        var parent = new int[nodes.Length];
        var s = 0;
        var t = nodes.Length - 1;

        while (Bfs(rGraph, s, t, parent))
        {
            for (var n = t; n != s; n = parent[n])
            {
                rGraph[parent[n], n] -= 1;
                rGraph[n, parent[n]] += 1;
            }
        }

        var visited = new bool[nodes.Length];
        Dfs(rGraph, s, visited);

        for (int i = 0; i < graph.GetLength(0); i++) 
        {
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                if (graph[i, j] > 0 && 
                    visited[i] && !visited[j])
                {
                    Console.WriteLine(nodes[i] + " - " + nodes[j]);
                }
            }
        }

        var l = visited.Count(v => v);

        return (l*(nodes.Length - l)).ToString();
    }

    private void Dfs(int[,] graph, int start, bool[] visited)
    {
        visited[start] = true;

        for (var i = 0; i < graph.GetLength(0); i++)
        {
            if (graph[start, i] > 0 && !visited[i])
            {
                Dfs(graph, i, visited);
            }
        }
    }

    private bool Bfs(int[,] graph, int start, int end, int[] parent)
    {
        var visited = new bool[graph.GetLength(0)];

        var queue = new Queue<int>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            for (var i = 0; i < graph.GetLength(0); i++)
            {
                if (graph[node, i] > 0 && !visited[i])
                {
                    queue.Enqueue(i);
                    visited[i] = true;
                    parent[i] = node;
                }
            }
        }

        return visited[end];
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
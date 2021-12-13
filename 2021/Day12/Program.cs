// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day12("12");
day.PostSecondStar();

public class Day12 : DayBase
{
    public Day12(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData();
        var graph = GetGraph(input);
        var paths = Search(graph, "start", new List<string>());
        return paths.Count().ToString();
    }

    public override string SecondStar()
    {
        var input = GetRowData();
        var graph = GetGraph(input);
        var paths = Search2(graph, "start", new List<string>());
        return paths.Count().ToString();
    }

    private static Dictionary<string, HashSet<string>> GetGraph(string[] input)
    {
        Dictionary<string, HashSet<string>> graph = new();
        foreach (var pair in input)
        {
            var parts = pair.Split('-', StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                if (!graph.ContainsKey(p))
                {
                    graph.Add(p, new HashSet<string>());
                }
                graph[p].Add(parts.Single(d => d != p));
            }
        }

        return graph;
    }

    private static IEnumerable<List<string>> Search(Dictionary<string, HashSet<string>> graph, string currentNode, List<string> currentPath)
    {
        currentPath.Add(currentNode);
        if (currentNode == "end") 
            return Enumerable.Repeat(currentPath, 1);
        
        return graph[currentNode]
            .Where(n => !(IsLower(n) && currentPath.Contains(n)))
            .SelectMany(n => Search(graph, n, new List<string>(currentPath)));
    }

    private static IEnumerable<List<string>> Search2(Dictionary<string, HashSet<string>> graph, string currentNode, List<string> currentPath)
    {
        currentPath.Add(currentNode);
        if (currentNode == "end") 
            return Enumerable.Repeat(currentPath, 1);
        
        return graph[currentNode]
            .Where(n => ValidateNodeSelection(n, currentPath))
            .SelectMany(n => Search2(graph, n, new List<string>(currentPath)));
    }

    private static bool ValidateNodeSelection(string node, List<string> currentPath) {
        if ((node == "start" || node == "end") && currentPath.Contains(node))
            return false;
        if (IsLower(node) && currentPath.Contains(node) && currentPath.GroupBy(x => x).Where(g => IsLower(g.Key)).Any(g => g.Count() > 1))
            return false;
        
        return true;
    }

    private static bool IsLower(string s) => s.All(c => char.IsLower(c));

}
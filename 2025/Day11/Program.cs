using Tools;

Day11 day = new();
day.OutputSecondStar();

public class Day11 : DayBase
{
    public Day11() : base("11")
    {
    }

    public override string FirstStar()
    {
        var lines = GetRowData();
        var graph = new Dictionary<string, string[]>();
        foreach (var line in lines)
        {
            graph[line[..3]] = line[5..].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
        }

        var stack = new Stack<string>();
        stack.Push("you");
        var paths = 0;

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            foreach (var neighbor in graph[current])
            {
                if (neighbor == "out")
                {
                    paths++;
                }
                else if (neighbor != "you")
                {
                    stack.Push(neighbor);
                }
            }
        }

        return paths.ToString();
    }

    public override string SecondStar()
    {
        var lines = GetRowData();
        var graph = new Dictionary<string, string[]>();
        foreach (var line in lines)
        {
            graph[line[..3]] = line[5..].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
        }

        var stack = new Stack<(string Node, List<string> Path)>();
        stack.Push(("svr", new List<string> { "svr" }));
        var paths = 0;

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            foreach (var neighbor in graph[current.Node])
            {
                if (neighbor == "out")
                {
                    System.Console.WriteLine("OUT!");
                    if (current.Path.Contains("dac") && current.Path.Contains("fft"))
                    {
                        System.Console.WriteLine(string.Join(" -> ", current.Path) + " -> out");
                        paths++;
                    }
                        
                }
                else if (!current.Path.Contains(neighbor))
                {
                    var newPath = new List<string>(current.Path) { current.Node };
                    stack.Push((neighbor, newPath));
                }
            }
        }

        return paths.ToString();
    }
}
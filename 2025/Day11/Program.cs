using Tools;

Day11 day = new();
day.OutputFirstStar();
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
                    paths++;
                else if (neighbor != "you")
                    stack.Push(neighbor);
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
        var result = DFS(graph, "svr", new Dictionary<string, long[]>());

        return result[3].ToString();
    }

    private long[] DFS(Dictionary<string, string[]> graph, string current, Dictionary<string, long[]> memo)
    {
        var paths = new long[4]; // [0] = no dac/fft, [1] = dac only, [2] = fft only, [3] = both

        foreach (var neighbor in graph[current])
        {
            if (neighbor == "out")
            {
                if (current == "dac")
                    paths[1]++;
                else if (current == "fft")
                    paths[2]++;
                else
                    paths[0]++;
    
                continue;
            }
            var result = memo.ContainsKey(neighbor) ? memo[neighbor] : DFS(graph, neighbor, memo);

            if (current == "dac")
            {
                paths[1] += result[0];
                paths[3] += result[2] + result[3];
            }
            else if (current == "fft")
            {
                paths[2] += result[0];
                paths[3] += result[1] + result[3];
            } 
            else
            {
                paths[0] += result[0];
                paths[1] += result[1];
                paths[2] += result[2];
                paths[3] += result[3];
            }
        }
        memo[current] = paths;

        return paths;
    }
}
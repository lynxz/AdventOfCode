using Tools;

var day = new Day12();
day.OutputSecondStar();

public class Day12 : DayBase
{
    public Day12() : base("12")
    {
    }

    List<(int x, int y)> Directions = new List<(int x, int y)>()
    {
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1)
    };

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var yMax = data.Length;
        var xMax = data[0].Length;

        var processed = new HashSet<(int x, int y)>();
        var stack = new Stack<(int x, int y)>();

        stack.Push((0, 0));

        var totalSum = 0;
        while (stack.Count > 0)
        {
            var (x, y) = stack.Pop();
            if (processed.Contains((x, y)))
            {
                continue;
            }

            var crop = data[y][x];
            var area = 0;
            var plotPoints = new Stack<(int x, int y)>();
            plotPoints.Push((x, y));

            var boundary = 0;
            while (plotPoints.Count > 0)
            {
                var (px, py) = plotPoints.Pop();
                foreach (var (dx, dy) in Directions)
                {
                    var nx = px + dx;
                    var ny = py + dy;
                    if (nx < 0 || nx >= xMax || ny < 0 || ny >= yMax || data[ny][nx] != crop)
                    {
                        boundary++;
                        if (nx >= 0 && nx < xMax && ny >= 0 && ny < yMax)
                        {
                            stack.Push((nx, ny));
                        }
                        continue;
                    }
                    if (processed.Contains((nx, ny)) || plotPoints.Contains((nx, ny)))
                    {
                        continue;
                    }
                    plotPoints.Push((nx, ny));
                }
                area++;
                processed.Add((px, py));
            }
            System.Console.WriteLine($"Crop: {crop}, Boundary: {boundary}, Area: {area}");
            totalSum += area * boundary;
        }


        return totalSum.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var yMax = data.Length;
        var xMax = data[0].Length;

        var processed = new HashSet<(int x, int y)>();
        var stack = new Stack<(int x, int y)>();

        stack.Push((0, 0));

        var totalSum = 0;
        while (stack.Count > 0)
        {
            var (x, y) = stack.Pop();
            if (processed.Contains((x, y)))
            {
                continue;
            }

            var crop = data[y][x];
            var area = 0;
            var plotPoints = new Stack<(int x, int y)>();
            plotPoints.Push((x, y));

            var boundary = 0;
            while (plotPoints.Count > 0)
            {
                var (px, py) = plotPoints.Pop();
                foreach (var (dx, dy) in Directions)
                {
                    var nx = px + dx;
                    var ny = py + dy;
                    if (nx < 0 || nx >= xMax || ny < 0 || ny >= yMax || data[ny][nx] != crop)
                    {
                        boundary++;
                        if (nx >= 0 && nx < xMax && ny >= 0 && ny < yMax)
                        {
                            stack.Push((nx, ny));
                        }
                        continue;
                    }
                    if (processed.Contains((nx, ny)) || plotPoints.Contains((nx, ny)))
                    {
                        continue;
                    }
                    plotPoints.Push((nx, ny));
                }
                area++;
                processed.Add((px, py));
            }
            System.Console.WriteLine($"Crop: {crop}, Boundary: {boundary}, Area: {area}");
            totalSum += area * boundary;
        }
    }
}
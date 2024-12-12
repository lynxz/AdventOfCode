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
            var perimeter = new List<(int x, int y)>();
            var plotPoints = new Stack<(int x, int y)>();
            plotPoints.Push((x, y));

            while (plotPoints.Count > 0)
            {
                var (px, py) = plotPoints.Pop();
                foreach (var (dx, dy) in Directions)
                {
                    var nx = px + dx;
                    var ny = py + dy;
                    if (nx < 0 || nx >= xMax || ny < 0 || ny >= yMax || data[ny][nx] != crop)
                    {
                        if (!perimeter.Contains((px, py)))
                        {
                            perimeter.Add((px, py));
                        }
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
            var sides = WalkPerimeter(perimeter, data, xMax, yMax);
            System.Console.WriteLine($"Crop: {crop}, Sides: {sides}, Area: {area}");
            totalSum += area * sides;
        }

        return totalSum.ToString();
    }

    List<(int x, int y)> Dirs = new List<(int x, int y)>()
    {
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    };

    private int WalkPerimeter(List<(int x, int y)> perimeter, string[] data, int xMax, int yMax)
    {
        var visited = new HashSet<(int x, int y)>();
        var sides = 0;
        var start = perimeter.OrderBy(c => c.y).ThenBy(c => c.x).First();
        var crop = data[start.y][start.x];

        var outDir = Dirs.First(d => d.x + start.x < 0 || d.x + start.x >= xMax || d.y + start.y < 0 || d.y + start.y >= yMax || data[d.y + start.y][d.x + start.x] != crop);
        var outDirIndex = Dirs.IndexOf(outDir);
        var startDirIndex = (outDirIndex + 1) % Dirs.Count;

        var pos = start;
        var currDirIndex = startDirIndex;
        do
        {
            visited.Add(pos);
            var od = Dirs[outDirIndex];
            var dx = pos.x + od.x;
            var dy = pos.y + od.y;
            if (dx >= 0 && dx < xMax && dy >= 0 && dy < yMax && data[dy][dx] == crop)
            {
                sides++;
                currDirIndex = (currDirIndex + 3) % Dirs.Count; 
                outDirIndex = (outDirIndex + 3) % Dirs.Count; 
                continue;
            }
            od = Dirs[currDirIndex];
            dx = pos.x + od.x;
            dy = pos.y + od.y;
            if (dx < 0 || dx >= xMax || dy < 0 || dy >= yMax && data[dy][dx] != crop)
            {
                sides++;
                currDirIndex = (currDirIndex + 1) % Dirs.Count;
                outDirIndex = (outDirIndex + 1) % Dirs.Count;
                continue;
            }
            pos = (dx, dy);

        } while (!(pos == start && currDirIndex == startDirIndex));

        return sides;
    }
}
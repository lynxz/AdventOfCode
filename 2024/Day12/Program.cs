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
            var perimeter = new List<(int x, int y, int v)>();
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
                        var v = nx < 0 || nx >= xMax || ny < 0 || ny >= yMax ? -1 : data[ny][nx];
                        if (!perimeter.Contains((px, py, v)))
                        {
                            perimeter.Add((px, py, v));
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
            totalSum += area * sides;
        }
        
        return totalSum.ToString();
    }

    private int WalkPerimeter(List<(int x, int y, int v)> perimeter, string[] data, int xMax, int yMax)
    {
        var visited = new HashSet<(int x, int y, int v)>();
        var sides = 0;
        while (!perimeter.All(p => visited.Contains(p)))
        {
            var points = perimeter.Where(p => !visited.Contains(p)).ToList();
            var start = points.OrderBy(c => c.y).ThenBy(c => c.x).First();
            var crop = data[start.y][start.x];

            var outDirIndex = Enumerable.Range(0, Directions.Count).First(i => start.v == GetOutDir((start.x, start.y), i, data, xMax, yMax));
            var startDirIndex = (outDirIndex + 1) % Directions.Count;

            var pos = start;
            var currDirIndex = startDirIndex;
            do
            {
                visited.Add(pos);

                var v = 0;
                var od = Directions[outDirIndex];
                var dx = pos.x + od.x;
                var dy = pos.y + od.y;
                if (dx >= 0 && dx < xMax && dy >= 0 && dy < yMax && data[dy][dx] == crop)
                {
                    sides++;
                    currDirIndex = (currDirIndex + 3) % Directions.Count;
                    outDirIndex = (outDirIndex + 3) % Directions.Count;
                    v = GetOutDir((dx, dy), outDirIndex, data, xMax, yMax);
                    pos = (dx, dy, v);
                    continue;
                }
                od = Directions[currDirIndex];
                dx = pos.x + od.x;
                dy = pos.y + od.y;

                var dia = (x:  Directions[outDirIndex].x + Directions[currDirIndex].x,y:  Directions[outDirIndex].y + Directions[currDirIndex].y);
                var ddx = pos.x + dia.x;
                var ddy = pos.y + dia.y;
                if (dx < 0 || dx >= xMax || dy < 0 || dy >= yMax || (!perimeter.Any(d => d.x == ddx && d.y == ddy) && data[dy][dx] != crop))
                {
                    sides++;
                    currDirIndex = (currDirIndex + 1) % Directions.Count;
                    outDirIndex = (outDirIndex + 1) % Directions.Count;
                    v = GetOutDir((pos.x, pos.y), outDirIndex, data, xMax, yMax);
                    pos = (pos.x, pos.y, v);
                    continue;
                }
                v = GetOutDir((dx, dy), outDirIndex, data, xMax, yMax);
                pos = (dx, dy, v);

            } while (!(pos.x == start.x && pos.y == start.y && currDirIndex == startDirIndex));
        }

        return sides;
    }

    private int GetOutDir((int x, int y) pos, int outDirIndex, string[] data, int xMax, int yMax) {
        var od = Directions[outDirIndex];
        var dx = pos.x + od.x;
        var dy = pos.y + od.y;
        return dx < 0 || dx >= xMax || dy < 0 || dy >= yMax ? -1 : data[dy][dx];
    }
}
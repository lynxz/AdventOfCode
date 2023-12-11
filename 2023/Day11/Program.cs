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
        return GetDistances(2).Sum().ToString();
    }

    public override string SecondStar()
    {
        return GetDistances(1000000).Sum().ToString();
    }

    private List<long> GetDistances(long multiplyer)
    {
        var data = GetRowData();
        var arr = data.ToMultidimensionalArray();
        var galax = Enumerable.Range(0, arr.GetLength(0)).SelectMany(y => Enumerable.Range(0, arr.GetLength(1)).Where(x => arr[y, x] == '#').Select(x => (y, x))).ToList();
        var comb = galax.DifferentCombinations(2).ToList();
        var dists = comb.Select(c => (long)ShortestPath(arr, c.First(), c.Last())).ToList();

        var emptyRows = Enumerable.Range(0, data.Length).Where(i => data[i].All(c => c == '.')).Select(i => i).ToArray();
        var emptyCols = Enumerable.Range(0, data[0].Length).Where(x => Enumerable.Range(0, data.Length).All(y => data[y][x] == '.')).Select(x => x).ToArray();

        for (int i = 0; i < comb.Count; i++)
        {
            var c = comb[i];
            var startX = Math.Min(c.First().x, c.Last().x);
            var startY = Math.Min(c.First().y, c.Last().y);
            var xDist = Math.Abs(c.First().x - c.Last().x);
            var yDist = Math.Abs(c.First().y - c.Last().y);
            var xLines = Enumerable.Range(startY, yDist).Where(x => emptyRows.Contains(x)).Count();
            var yLines = Enumerable.Range(startX, xDist).Where(y => emptyCols.Contains(y)).Count();

            var newDist = dists[i] + (xLines + yLines) * (multiplyer - 1);

            dists[i] = newDist;
        }

        return dists;
    }

    int ShortestPath(char[,] map, (int y, int x) start, (int y, int x) end)
    {
        var visited = new HashSet<(int y, int x)>();
        var queue = new Queue<(int y, int x, int steps, int distance)>();
        var totDist = Distance(start, end);
        queue.Enqueue((start.y, start.x, 0, totDist));
        while (queue.Count > 0)
        {
            var (y, x, steps, distance) = queue.Dequeue();
            if (y == end.y && x == end.x)
                return steps;

            visited.Add((y, x));
            var nextSteps = new[] { (y + 1, x), (y - 1, x), (y, x + 1), (y, x - 1) };
            var min = (y, x, steps, distance);
            foreach (var (ny, nx) in nextSteps)
            {
                var dist = Distance((ny, nx), end);
                if (ny < 0 || ny >= map.GetLength(0) || nx < 0 || nx >= map.GetLength(1) || visited.Contains((ny, nx)) || dist > min.distance)
                    continue;

                min = (ny, nx, steps + 1, dist);
            }
            queue.Enqueue(min);
        }

        return -1;
    }

    int Distance((int y, int x) p1, (int y, int x) p2) =>
        (p1.y - p2.y) * (p1.y - p2.y) + (p1.x - p2.x) * (p1.x - p2.x);


}
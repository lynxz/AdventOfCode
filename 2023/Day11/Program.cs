using Tools;

Day11 day = new();
day.OutputFirstStar();

public class Day11 : DayBase
{
    public Day11() : base("11")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var rows = data.Select(r => r.Contains('#') ? new[] { r.Trim() } : new[] { r.Trim(), new string(Enumerable.Repeat('.', r.Length).ToArray()) }).SelectMany(x => x).ToList();
        var cols = Enumerable.Range(0, rows.Count).Select(x => new List<char>()).ToList();
        for (int x = 0; x < rows[0].Length; x++)
        {
            var ys = Enumerable.Range(0, rows.Count);

            foreach (var y in ys)
            {
                cols[y].Add(rows[y][x]);
                if (ys.All(y => rows[y][x] == '.'))
                {
                    cols[y].Add('.');
                }
            }
        }
        var arr = cols.ToMultidimensionalArray();
        var galax = Enumerable.Range(0, arr.GetLength(0)).SelectMany(y => Enumerable.Range(0, arr.GetLength(1)).Where(x => arr[y, x] == '#').Select(x => (y, x))).ToList();
        var comb = galax.DifferentCombinations(2);
        
        return comb.Sum(c => ShortestPath(arr, c.First(), c.Last())).ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }

    int ShortestPath(char[,] map, (int y, int x) start, (int y, int x) end)
    {
        var visited = new HashSet<(int y, int x)>();
        var queue = new Queue<(int y, int x, int steps)>();
        queue.Enqueue((start.y, start.x, 0));
        while (queue.Count > 0)
        {
            var (y, x, steps) = queue.Dequeue();
            if (y == end.y && x == end.x)
                return steps;

            visited.Add((y, x));
            var nextSteps = new[] { (y + 1, x), (y - 1, x), (y, x + 1), (y, x - 1) };
            foreach (var (ny, nx) in nextSteps)
            {
                if (ny < 0 || ny >= map.GetLength(0) || nx < 0 || nx >= map.GetLength(1) || visited.Contains((ny, nx)))
                    continue;

                queue.Enqueue((ny, nx, steps + 1));
            }
        }

        return -1;
    }


}
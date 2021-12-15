// See https://aka.ms/new-console-template for more information
using Tools;

Day15 day = new("15");
day.OutputSecondStar();

public class Day15 : DayBase
{
    public Day15(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData();
        var array = Enumerable.Range(0, input.Length)
            .Select(y => Enumerable.Range(0, input[0].Length).Select(x => int.Parse(input[y][x].ToString())))
            .ToMultidimensionalArray();

        var (dist, prev) = Djikstra(array);
        var node = (Y: array.GetLength(0) - 1, X: array.GetLength(1) - 1);
        return dist[node].ToString();
    }

    public override string SecondStar()
    {
        var input = GetRowData();
        var array = Enumerable.Range(0, input.Length)
            .Select(y => Enumerable.Range(0, input[0].Length).Select(x => int.Parse(input[y][x].ToString())))
            .ToMultidimensionalArray();
        var yMax = array.GetLength(0);
        var xMax = array.GetLength(1);
        var bigArray = new int[array.GetLength(0) * 5, array.GetLength(1) * 5];

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int y = 0; y <yMax; y++)
                {
                    for (int x = 0; x < xMax; x++)
                    {
                        bigArray[y + yMax * i, x + xMax * j] = (array[y, x] + i + j);
                        if (bigArray[y + yMax * i, x + xMax * j] > 9)
                            bigArray[y + yMax * i, x + xMax * j] = (bigArray[y + yMax * i, x + xMax * j] % 10) + 1;
                    }
                }
            }
        }

        var (dist, prev) = Djikstra(bigArray);
        var node = (Y: bigArray.GetLength(0) - 1, X: bigArray.GetLength(1) - 1);
        return dist[node].ToString();
    }

    (Dictionary<(int Y, int X), int>, Dictionary<(int Y, int X), (int Y, int X)>) Djikstra(int[,] array)
    {
        HashSet<(int Y, int X)> vertex = new();
        Dictionary<(int Y, int X), int> dist = new();
        Dictionary<(int Y, int X), (int Y, int X)> prev = new();
        var offsets = new[] { (Y: -1, X: 0), (Y: 1, X: 0), (Y: 0, X: -1), (Y: 0, X: 1) };
        for (int y = 0; y < array.GetLength(0); y++)
        {
            for (int x = 0; x < array.GetLength(1); x++)
            {
                dist.Add((y, x), int.MaxValue);
                vertex.Add((y, x));
            }
        }
        dist[(0, 0)] = 0;
        while (vertex.Count != 0)
        {
            var node = vertex.MinBy(n => dist[n]);
            vertex.Remove(node);
            if (node.Y == array.GetLength(0) -1 && node.X == array.GetLength(1) -1)
                return (dist, prev);
            foreach (var v in offsets.Where(o => vertex.Contains(((node.Y + o.Y, node.X + o.X)))).Select(o => ((Y: node.Y + o.Y, X: node.X + o.X))))
            {
                var alt = dist[node] + array[v.Y, v.X];
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = node;
                }
            }
        }
        return (dist, prev);
    }
}

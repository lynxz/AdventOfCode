using Tools;

Day08 day = new Day08();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day08 : DayBase
{
    public Day08() : base("8")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(x => x.GetIntegers()).ToArray();
        var distances = CalculateDistances(data);
        var circuits = Enumerable.Range(0, data.Length).ToDictionary(x => x, x => new List<int>());

        foreach (var pair in distances.OrderBy(x => x.Value).Take(1000))
        {
            var box1 = pair.Key.box1;
            var box2 = pair.Key.box2;
            if (!AreConnected(box1, box2, circuits))
            {
                circuits[box1].Add(box2);
                circuits[box2].Add(box1);
            }
        }

        List<int> sizes = GetCircuitSizes(circuits);
        sizes.Sort();
        sizes.Reverse();

        return sizes.Take(3).Aggregate(1, (a, b) => a * b).ToString();
    }

   

    public override string SecondStar()
    {
        var data = GetRowData().Select(x => x.GetIntegers()).ToArray();
        var distances = CalculateDistances(data);
        var circuits = Enumerable.Range(0, data.Length).ToDictionary(x => x, x => new List<int>());

        foreach (var pair in distances.OrderBy(x => x.Value))
        {
            var box1 = pair.Key.box1;
            var box2 = pair.Key.box2;
            if (!AreConnected(box1, box2, circuits))
            {
                circuits[box1].Add(box2);
                circuits[box2].Add(box1);
            }
            if (GetCircuitSizes(circuits).Count == 1)
            {
                return (data[box1][0] * data[box2][0]).ToString();
            }
        }

        throw new Exception("No single circuit found");
    }

    static Dictionary<(int box1, int box2), double> CalculateDistances(int[][] data)
    {
        var distances = new Dictionary<(int box1, int box2), double>();
        for (int i = 0; i < data.Length - 1; i++)
        {
            for (int j = i + 1; j < data.Length; j++)
            {
                long xDist = data[i][0] - data[j][0];
                long yDist = data[i][1] - data[j][1];
                long zDist = data[i][2] - data[j][2];
                var dist = Math.Sqrt(xDist * xDist + yDist * yDist + zDist * zDist);
                distances[(i, j)] = dist;
            }
        }
        return distances;
    }

   static bool AreConnected(int box1, int box2, Dictionary<int, List<int>> circuits)
    {
        var toVisit = new Queue<int>();
        var visited = new HashSet<int>();
        toVisit.Enqueue(box1);
        while (toVisit.Count > 0)
        {
            var current = toVisit.Dequeue();
            if (current == box2)
                return true;
            visited.Add(current);
            foreach (var neighbor in circuits[current])
            {
                if (!visited.Contains(neighbor))
                    toVisit.Enqueue(neighbor);
            }
        }
        return false;
    }

     private static List<int> GetCircuitSizes(Dictionary<int, List<int>> circuits)
    {
        List<int> sizes = new();
        var visited = new HashSet<int>();

        for (int i = 0; i < circuits.Count; i++)
        {
            if (visited.Contains(i))
                continue;
            var toVisit = new Queue<int>();
            toVisit.Enqueue(i);
            int size = 0;
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                if (visited.Contains(current))
                    continue;
                visited.Add(current);
                size++;
                foreach (var neighbor in circuits[current])
                {
                    if (!visited.Contains(neighbor))
                        toVisit.Enqueue(neighbor);
                }
            }
            sizes.Add(size);
        }

        return sizes;
    }

}
// See https://aka.ms/new-console-template for more information
using Tools;

Day19 day = new("19");
day.OutputSecondStar();

public class Day19 : DayBase
{
    public Day19(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData()
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(d => d.First().GetIntegers().Single(), d => d.Skip(1).Select(p => p.GetIntegers()).ToList());
        var transforms = GetTransforms(data);
        var beacons = GetBeaconCoordinates(data, transforms).Distinct().ToList();
        return beacons.Count.ToString();
    }

    private Dictionary<int, Dictionary<int, (int[] Center, int[,] Transform)>> GetTransforms(Dictionary<int, List<int[]>> data)
    {
        Dictionary<int, Dictionary<int, (int[] Center, int[,] Transform)>> transforms = new();
        var distances = data.Select(kvp => CalculateDistances(kvp.Value)).ToList();

        for (int i = 0; i < data.Keys.Count - 1; i++)
        {
            for (int j = i + 1; j < data.Keys.Count; j++)
            {
                var matchingBeacons = FindMatchingBeacons(distances[i], distances[j]).ToList();
                if (matchingBeacons.Count >= 12)
                {
                    if (i == 0)
                    {
                        FindTransform(transforms, matchingBeacons, data, i, j);
                    }
                    else
                    {
                        FindTransform(transforms, matchingBeacons, data, i, j);
                        matchingBeacons = matchingBeacons.Select(pair => (pair.Item2, pair.Item1)).ToList();
                        FindTransform(transforms, matchingBeacons, data, j, i);
                    }
                }
            }
        }

        return transforms;
    }

    private IEnumerable<(int X, int Y, int Z)> GetBeaconCoordinates(Dictionary<int, List<int[]>> data, Dictionary<int, Dictionary<int, (int[] Center, int[,] Transform)>> transforms)
    {

        foreach (var kvp in data)
        {
            var path = FindTransform(kvp.Key, transforms, new List<int>());
            var coords = kvp.Value;
            var currentScanner = kvp.Key;
            while (path.Count > 0)
            {
                var toScanner = path.Pop();
                var trans = transforms[currentScanner][toScanner];
                coords = coords.Select(d => VectorAdd(trans.Center, VectorMultiply(d, trans.Transform))).ToList();
                currentScanner = toScanner;
            }
            foreach (var coord in coords.Select(d => (d[0], d[1], d[2])))
                yield return coord;
        }
    }

    Stack<int> FindTransform(int key, Dictionary<int, Dictionary<int, (int[] Center, int[,] Transform)>> transforms, List<int> visitedKeys)
    {
        visitedKeys.Add(key);
        if (key == 0)
            return new Stack<int>();

        foreach (var kvp in transforms[key])
        {
            if (!visitedKeys.Contains(kvp.Key))
            {
                var list = FindTransform(kvp.Key, transforms, visitedKeys);
                if (list != null)
                {
                    list.Push(kvp.Key);
                    return list;
                }
            }
        }

        return null;
    }

    private void FindTransform(Dictionary<int, Dictionary<int, (int[], int[,])>> transforms, List<(int, int)> matchingBeacons, Dictionary<int, List<int[]>> data, int i, int j)
    {
        var arrays = GenerateArrays();
        foreach (var array in arrays)
        {
            var center = matchingBeacons.Select(m => VectorSubtract(data[i][m.Item1], VectorMultiply(data[j][m.Item2], array))).ToList();
            if (center.All(x => center[0][0] == x[0] && center[0][1] == x[1] && center[0][2] == x[2]))
            {
                if (!transforms.ContainsKey(j))
                    transforms.Add(j, new Dictionary<int, (int[], int[,])>());
                transforms[j].Add(i, (center.First(), array));
                return;
            }
        }
        throw new Exception();
    }

    List<int[,]> GenerateArrays()
    {
        List<int[,]> arrays = new();
        var a = new[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
        var yRot = new[,] { { 0, 0, -1 }, { 0, 1, 0 }, { 1, 0, 0 } };
        var zRot = new[,] { { 0, -1, 0 }, { 1, 0, 0 }, { 0, 0, 1 } };
        var xRot = new[,] { { 1, 0, 0 }, { 0, 0, -1 }, { 0, 1, 0 } };

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    arrays.Add((int[,])a.Clone());
                    a = ArrayMultiply(a, yRot);
                }
                a = ArrayMultiply(a, zRot);
            }
            a = ArrayMultiply(a, xRot);
        }
        return arrays;
    }

    int[,] ArrayMultiply(int[,] a1, int[,] a2)
    {
        var product = new int[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                product[i, j] = a1[i, 0] * a2[0, j] + a1[i, 1] * a2[1, j] + a1[i, 2] * a2[2, j];

        return product;
    }

    int[] VectorMultiply(int[] v, int[,] a) =>
     Enumerable.Range(0, 3).Select(i => a[i, 0] * v[0] + a[i, 1] * v[1] + a[i, 2] * v[2]).ToArray();

    int[] VectorSubtract(int[] v1, int[] v2) =>
       Enumerable.Range(0, 3).Select(i => v1[i] - v2[i]).ToArray();

    int[] VectorAdd(int[] v1, int[] v2) =>
       Enumerable.Range(0, 3).Select(i => v1[i] + v2[i]).ToArray();

    IEnumerable<(int, int)> FindMatchingBeacons(Dictionary<int, List<double>> scanner1, Dictionary<int, List<double>> scanner2)
    {
        List<int> matchedBeacons = new();
        foreach (var beacon1 in scanner1)
        {
            foreach (var beacon2 in scanner2)
            {
                if (!matchedBeacons.Contains(beacon2.Key) && beacon1.Value.Count(d => beacon2.Value.Contains(d)) > 1)
                {
                    matchedBeacons.Add(beacon2.Key);
                    yield return (beacon1.Key, beacon2.Key);
                }
            }
        }
    }

    Dictionary<int, List<double>> CalculateDistances(List<int[]> coords)
    {
        var distDict = Enumerable.Range(0, coords.Count).ToDictionary(i => i, i => new List<double>());
        for (int i = 0; i < coords.Count; i++)
            for (int j = 0; j < coords.Count; j++)
                if (i != j)
                    distDict[i].Add(Distance(coords[i], coords[j]));

        return distDict;
    }

    double Distance(int[] a, int[] b) => Math.Sqrt(Math.Pow(a[0] - b[0], 2) + Math.Pow(a[1] - b[1], 2) + Math.Pow(a[2] - b[2], 2));

    public override string SecondStar()
    {
        var data = GetRawData()
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(d => d.First().GetIntegers().Single(), d => d.Skip(1).Select(p => p.GetIntegers()).ToList());

        var transforms = GetTransforms(data);

        var scanners = GetScannerCoordinates(data, transforms).Distinct().ToList();
        var max = 0;
        for (int i = 0; i < scanners.Count - 1; i++)
        {
            for (int j = i + 1; j < scanners.Count; j++)
            {
                var valX = Math.Max(scanners[i].X, scanners[j].X) - Math.Min(scanners[i].X, scanners[j].X);
                var valY = Math.Max(scanners[i].Y, scanners[j].Y) - Math.Min(scanners[i].Y, scanners[j].Y);
                var valZ = Math.Max(scanners[i].Z, scanners[j].Z) - Math.Min(scanners[i].Z, scanners[j].Z);
                var val = valX+valY+valZ;
                if (val > max) 
                    max = val;
            }
           
        }

        return max.ToString();
    }

    private IEnumerable<(int X, int Y, int Z)> GetScannerCoordinates(Dictionary<int, List<int[]>> data, Dictionary<int, Dictionary<int, (int[] Center, int[,] Transform)>> transforms)
    {
        foreach (var kvp in data)
        {
            var path = FindTransform(kvp.Key, transforms, new List<int>());
            var currentScanner = path.Count >= 1 ? path.Pop() : -1;
            var coord = currentScanner != -1 ? transforms[kvp.Key][currentScanner].Center : new int[] { 0, 0, 0 };
            while (path.Count >= 1)
            {
                var toScanner = path.Pop();
                var trans = transforms[currentScanner][toScanner];
                coord = VectorAdd(trans.Center, VectorMultiply(coord, trans.Transform));
                currentScanner = toScanner;
            }
            yield return (coord[0], coord[1], coord[2]);
        }
    }
}

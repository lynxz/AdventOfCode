using Tools;

Day22 day = new();
day.OutputSecondStar();

public class Day22 : DayBase
{
    public Day22() : base("22")
    {
    }

    public override string FirstStar()
    {
        List<(int[] p1, int[] p2)> landed = new();
        var max = GetData(landed);

        var supported = landed.ToDictionary(l => l, l => new List<(int[] p1, int[] p2)>());
        var supports = landed.ToDictionary(l => l, l => new List<(int[] p1, int[] p2)>());
        for (int layer = 2; layer <= max; layer++)
        {
            foreach (var b in landed.Where(l => l.p1[2] == layer))
            {
                foreach (var s in landed.Where(l => l.p2[2] == layer - 1))
                {
                    if (Overlaps(s, b))
                    {
                        supported[b].Add(s);
                        supports[s].Add(b);
                    }
                }
            }
        }

        return landed.Count(l => supports[l].All(k => supported[k].Count > 1)).ToString();
    }

    private int GetData(List<(int[] p1, int[] p2)> landed)
    {
        int max = 0;
        var data = GetRowData().Select(b => b.Split('~', StringSplitOptions.RemoveEmptyEntries)).Select(v => (p1: v[0].GetIntegers(), p2: v[1].GetIntegers())).ToList();
        data = data.OrderBy(b => b.p1[2]).ToList();


        landed = new List<(int[] p1, int[] p2)>();
        max = int.MinValue;
        foreach (var b in data)
        {
            if (b.p1[2] == 1)
            {
                landed.Add(b);
                continue;
            }

            var handled = false;
            for (int i = landed.Count - 1; i >= 0; i--)
            {
                var l = landed[i];
                if (Overlaps(l, b))
                {
                    handled = true;
                    var diff = b.p1[2] - landed[i].p2[2];
                    b.p1[2] = landed[i].p2[2] + 1;
                    b.p2[2] = b.p2[2] - diff + 1;
                    landed.Add(b);
                    if (max < b.p1[2])
                        max = b.p1[2];
                    break;
                }
            }
            if (!handled)
            {
                var diff = b.p1[2] - 1;
                b.p1[2] = 1;
                b.p2[2] = b.p2[2] - diff;
                landed.Add(b);
            }
            landed = landed.OrderBy(l => l.p2[2]).ToList();
        }
        return max;
    }

    public override string SecondStar()
    {
        List<(int[] p1, int[] p2)> landed = new();
        var max = GetData(landed);

        var supported = landed.ToDictionary(l => l, l => new List<(int[] p1, int[] p2)>());
        var supports = landed.ToDictionary(l => l, l => new List<(int[] p1, int[] p2)>());
        for (int layer = 2; layer <= max; layer++)
        {
            foreach (var b in landed.Where(l => l.p1[2] == layer))
            {
                foreach (var s in landed.Where(l => l.p2[2] == layer - 1))
                {
                    if (Overlaps(s, b))
                    {
                        supported[b].Add(s);
                        supports[s].Add(b);
                    }
                }
            }
        }

        var sum = 0;

        foreach (var l in landed)
        {
            var falling = new HashSet<(int[] p1, int[] p2)> { l };
            CalculateSupport(l, falling, supported, supports);
            sum += falling.Count - 1;
        }

        return sum.ToString();
    }

    void CalculateSupport((int[] p1, int[] p2) dis, HashSet<(int[] p1, int[] p2)> falling, Dictionary<(int[] p1, int[] p2), List<(int[] p1, int[] p2)>> supported, Dictionary<(int[] p1, int[] p2), List<(int[] p1, int[] p2)>> supports)
    {
        var newFalling = supports[dis].Where(b => supported[b].Except(falling).Count() == 0).Select(b => b).ToList();
        newFalling.ForEach(b => falling.Add(b));
        foreach (var nf in newFalling)
        {
            CalculateSupport(nf, falling, supported, supports);
        }
    }

    bool Overlaps((int[] p1, int[] p2) l, (int[] p1, int[] p2) b)
    {
        var lCoords = new HashSet<(int x, int y)>(Enumerable.Range(l.p1[0], l.p2[0] - l.p1[0] + 1).SelectMany(x => Enumerable.Range(l.p1[1], l.p2[1] - l.p1[1] + 1).Select(y => (x, y))));
        return Enumerable.Range(b.p1[0], b.p2[0] - b.p1[0] + 1).Any(x => Enumerable.Range(b.p1[1], b.p2[1] - b.p1[1] + 1).Any(y => lCoords.Contains((x, y))));
    }
}
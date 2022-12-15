using Tools;

Day15 day = new Day15();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day15 : DayBase
{
    public Day15() : base("15")
    {
    }

    public override string FirstStar()
    {
        const int row = 2000000;
        var data = GetRowData();
        var positions = data.Select(l => l.GetIntegers());
        var beaconMap = data
            .Select(l => l.GetIntegers())
            .GroupBy(d => (Y: d[3], X: d[2]))
            .ToDictionary(g => g.Key, g => g.Select(d => (Y: d[1], X: d[0])).ToList());

        var sensors = beaconMap.SelectMany(kvp =>
            kvp.Value.Where(s =>
                s.Y < row ?
                s.Y + Math.Abs(s.Y - kvp.Key.Y) + Math.Abs(s.X - kvp.Key.X) > row :
                s.Y - (Math.Abs(s.Y - kvp.Key.Y) + Math.Abs(s.X - kvp.Key.X)) < row
                ).Select(s => new KeyValuePair<(int Y, int X), (int Y, int X)>(kvp.Key, s))
            ).ToList();

        var xCoords = new HashSet<int>(sensors.SelectMany(kvp => Enumerable.Range(
            kvp.Value.X - (Math.Abs(kvp.Value.Y - kvp.Key.Y) + Math.Abs(kvp.Value.X - kvp.Key.X) - Math.Abs(row - kvp.Value.Y)),
             (2 * (Math.Abs(kvp.Value.Y - kvp.Key.Y) + Math.Abs(kvp.Value.X - kvp.Key.X))) + 1 - (2 * Math.Abs(row - kvp.Value.Y))
            )));

        return (xCoords.Count - 1).ToString();
    }

    public override string SecondStar()
    {
        const int max = 4000000;
        var data = GetRowData();
        var positions = data.Select(l => l.GetIntegers());
        var beaconMap = data
            .Select(l => l.GetIntegers())
            .GroupBy(d => (Y: d[3], X: d[2]))
            .ToDictionary(g => g.Key, g => g.Select(d => (Y: d[1], X: d[0])).ToList());

        for (int row = 0; row < max; row++)
        {
            var sensors = beaconMap.SelectMany(kvp =>
            kvp.Value.Where(s =>
                s.Y < row ?
                s.Y + Math.Abs(s.Y - kvp.Key.Y) + Math.Abs(s.X - kvp.Key.X) > row :
                s.Y - (Math.Abs(s.Y - kvp.Key.Y) + Math.Abs(s.X - kvp.Key.X)) < row
                ).Select(s => new KeyValuePair<(int Y, int X), (int Y, int X)>(kvp.Key, s))
            ).ToList();

            var ranges = sensors.ConvertAll(kvp => (
                kvp.Value.X - (Math.Abs(kvp.Value.Y - kvp.Key.Y) + Math.Abs(kvp.Value.X - kvp.Key.X) - Math.Abs(row - kvp.Value.Y)),
                kvp.Value.X - (Math.Abs(kvp.Value.Y - kvp.Key.Y) + Math.Abs(kvp.Value.X - kvp.Key.X) - Math.Abs(row - kvp.Value.Y)) + (2 * (Math.Abs(kvp.Value.Y - kvp.Key.Y) + Math.Abs(kvp.Value.X - kvp.Key.X))) - (2 * Math.Abs(row - kvp.Value.Y))
                ));

            var mergedRanges = MergeRanges(ranges);
            if (!mergedRanges.Any(r => r.Start <= 0 && r.End >= max)) {
                return (((mergedRanges.First(r => r.End < max).End + 1L)* 4000000L) + row).ToString();
            }
        }

        return "Fail!";
    }

    private List<(int Start, int End)> MergeRanges(List<(int Start, int End)> ranges)
    {
        if (ranges.Count == 1)
            return ranges;

        var newRanges = new List<(int Start, int End)>();
        for (int i = 0; i < ranges.Count; i++)
        {
            bool merged = false;
            int j = i+1;
            for (; j < ranges.Count; j++)
            {
                var count = newRanges.Count;
                if (ranges[i].Start <= ranges[j].Start && ranges[i].End >= ranges[j].End) 
                    newRanges.Add(ranges[i]);
                else if (ranges[i].Start >= ranges[j].Start && ranges[i].End <= ranges[j].End)
                    newRanges.Add(ranges[j]);
                else if (ranges[i].Start <= ranges[j].Start && ranges[i].End >= ranges[j].Start)
                    newRanges.Add((ranges[i].Start, ranges[j].End));
                else if (ranges[i].Start >= ranges[j].Start && ranges[i].Start <= ranges[j].End)
                    newRanges.Add((ranges[j].Start, ranges[i].End));

                if (newRanges.Count > count) {
                    merged = true;
                    break;
                }
            }
            if (!merged)
            {
                newRanges.Add(ranges[i]);
            }
            else
            {
                newRanges.AddRange(Enumerable.Range(i + 1, ranges.Count - i -1).Where(i => i != j).Select(i => ranges[i]));
                break;
            }
        }
        if (newRanges.Count == ranges.Count && newRanges.All(r => ranges.Any(r2 => r2.Start == r.Start && r2.End == r.End)))
            return newRanges;

        return MergeRanges(newRanges);
    }
}
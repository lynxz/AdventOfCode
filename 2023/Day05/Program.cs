using Tools;

Day05 day = new Day05();
day.OutputSecondStar();

public class Day05 : DayBase
{
    public Day05() : base("5")
    {

    }

    public override string FirstStar()
    {
        var data = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var seeds = data[0].GetLongs();
        var min = long.MaxValue;
        foreach (var seed in seeds)
        {
            var val = seed;
            foreach (var map in data.Skip(1))
            {
                foreach (var range in map.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1))
                {
                    var r = range.GetLongs();
                    if (val >= r[1] && val <= r[1] + r[2] - 1)
                    {
                        val = r[0] + (val - r[1]);
                        break;
                    }
                }
            }
            if (val < min)
                min = val;
        }


        return min.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var seeds = data[0].GetLongs().Chunk(2);
        var ranges = new List<List<long[]>>();

        int i = 0;
        foreach (var map in data.Skip(1))
        {
            ranges.Add(new List<long[]>());
            foreach (var range in map.Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                ranges[i].Add(range.GetLongs());
            }
            i++;
        }

        var merge = seeds.SelectMany(s => MergeRanges(ranges, 0, s.ToArray())).ToList();

        return merge.Min(s => s[0]).ToString();
    }

    private List<long[]> MergeRanges(List<List<long[]>> ranges, int index, long[] range)
    {
        if (index == ranges.Count)
            return new List<long[]> { range };

        foreach (var r in ranges[index])
        {
            if (r[1] <= range[0] && r[1] + r[2] >= range[0] + range[1])
            {
                return MergeRanges(ranges, index + 1, new[] { r[0] + (range[0] - r[1]), range[1] });
            }
            else if (r[1] <= range[0] && r[1] + r[2] >= range[0])
            {
                var merged = MergeRanges(ranges, index + 1, new[] { r[0] + (range[0] - r[1]), r[1] + r[2] - range[0] });
                merged.AddRange(MergeRanges(ranges, index, new[] { r[1] + r[2] + 1, range[0] + range[1] - (r[1] + r[2]) }));
                return merged;
            }
            else if (r[1] > range[0] && r[1] < range[0] + range[1])
            {
                var merged = MergeRanges(ranges, index + 1, new[] { r[0], range[0] + range[1] - r[1] });
                merged.AddRange(MergeRanges(ranges, index, new[] { range[0], r[1] - range[0] }));
                return merged;
            }
            else if (r[1] > range[0] && r[1] + r[2] < range[0] + range[1])
            {
                var merged = MergeRanges(ranges, index + 1, new[] { r[0], r[2] });
                merged.AddRange(MergeRanges(ranges, index, new[] { r[1] + r[2] + 1, range[0] + range[1] - (r[1] + r[2]) }));
                merged.AddRange(MergeRanges(ranges, index, new[] { range[0], r[1] - range[0] }));
                return merged;
            }
        }

        return MergeRanges(ranges, index + 1, range);
    }
}
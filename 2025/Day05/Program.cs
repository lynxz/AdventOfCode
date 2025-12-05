using Tools;

Day05 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day05 : DayBase
{
    public Day05() : base("5")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var ranges = GetRanges(data);
        return Enumerable
            .Range(ranges.Count, data.Length - ranges.Count)
            .Select(i => long.Parse(data[i].Trim()))
            .Count(v => ranges.Any(r => v >= r.Start && v <= r.End))
            .ToString();
    }

    private static List<Range> GetRanges(string[] data)
    {
        int i = 0;
        var ranges = new List<Range>();
        while (data[i].Contains("-"))
        {
            var parts = data[i].Trim().Split("-");
            var range = new Range(long.Parse(parts[0]), long.Parse(parts[1]));
            ranges.Add(range);
            i++;
        }

        return ranges;
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var ranges = GetRanges(data);
        MergeRanges(ranges, 0);

        return ranges.Sum(r => r.End - r.Start + 1).ToString();
    }

    public void MergeRanges(List<Range> ranges, int i)
    {
        if (i >= ranges.Count) return;

        var currentRange = ranges[i];
        for (int j = i + 1; j < ranges.Count; j++)
        {
            var range = ranges[j];
            if ((range.Start <= currentRange.End && range.End >= currentRange.End) ||
                (range.Start <= currentRange.Start && range.End >= currentRange.Start) ||
                (range.Start >= currentRange.Start && range.End <= currentRange.End))
            {
                ranges[i] = new Range(Math.Min(currentRange.Start, range.Start), Math.Max(currentRange.End, range.End));
                ranges.RemoveAt(j);
                MergeRanges(ranges, i);
                return;
            }
        }
        MergeRanges(ranges, i + 1);
    }
}

public record Range(long Start, long End);
using Tools;

var day = new Day03();
day.OutputSecondStar();

public class Day03 : DayBase
{
    public Day03() : base("3")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
        var values = new List<long>();

        foreach (var line in data)
        {
            var nums = line.Select(c => int.Parse(c.ToString())).ToArray();
            var (max, position) = ExtractMax(nums);
            if (position == nums.Length - 1)
            {
               var (secondMax, _) = ExtractMax(nums.AsSpan(0, nums.Length - 1));
                values.Add(secondMax*10 + max);
            } else
            {
                var (secondMax, _) = ExtractMax(nums.AsSpan(position + 1));
                values.Add(max*10 + secondMax);
            }
        }
        return values.Sum().ToString();
    }

    private static (int max, int position) ExtractMax(Span<int> nums)
    {
        var max = -1;
        var p = -1;

        for (int pos = 0; pos < nums.Length; pos++)
        {
            if (max < nums[pos])
            {
                max = nums[pos];
                p = pos;
            }
        }

        return (max, p);
    }

    public override string SecondStar()
    {
        var data = GetRowData().Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToList();
        var values = new List<long>();

        foreach (var line in data)
        {
            var nums = line.Select(c => int.Parse(c.ToString())).ToArray();
            var p = 0;
            var total = 0L;
            for (int i = 0; i < 12; i++)
            {
                //System.Console.WriteLine($"Iteration {i}, position {p}");
                var (max, position) = RecurseExtractMax(nums.AsSpan(p), 11 - i);
                //System.Console.WriteLine($"  Found max {max} at position {position}");
                total = total * 10 + max;
                p += position + 1;
            }
            //System.Console.WriteLine($"Total for line {line} is {total}");
            values.Add(total);

        }

        foreach (var v in values)
        {
            Console.WriteLine(v);
        }

        return values.Sum().ToString();
    }

    private static (int max, int position) RecurseExtractMax(Span<int> nums, int digitsLeft)
    {
        var max = -1;
        var p = -1;

        for (int pos = 0; pos < nums.Length; pos++)
        {
            if (max < nums[pos])
            {
                max = nums[pos];
                p = pos;
            }
        }

        //System.Console.WriteLine($"  Considering max {max} at position {p} with {digitsLeft} digits left ");

        if (nums.Length - p - 1 < digitsLeft)
        {
            return RecurseExtractMax(nums.Slice(0, p), digitsLeft - (nums.Length - p));
        }
        

        return (max, p);
    }
}
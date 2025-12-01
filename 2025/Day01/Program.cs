using Tools;

var day = new Day01();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day01 : DayBase
{
    public Day01() : base("1") { }

    public override string FirstStar()
    {
        var data = GetRowData().Select(x => x[0] == 'L' ? -1 * (int.Parse(x.Substring(1).Trim()) % 100) : int.Parse(x.Substring(1).Trim()) % 100).ToArray();
        var vals = data.Aggregate(new List<int> { 50 }, (acc, x) =>
        {
            var prev = acc.Last();
            acc.Add(prev + x < 0 ? 100 + prev + x : (prev + x) % 100);
            return acc;
        });

        return vals.Count(x => x == 0).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(x => x[0] == 'L' ? -1 * int.Parse(x.Substring(1).Trim()) : int.Parse(x.Substring(1).Trim())).ToArray();
        var clicks = 0;
        var vals = data.Aggregate(50, (prev, x) =>
        {
            var next = prev + x;
            if (next > 99)
            {
                clicks += next / 100;
                next = next % 100;
            }
            else if (next < 0)
            {
                clicks += Math.Abs(next / 100) + (prev == 0 ? 0 : 1);
                next = 100 + prev - (Math.Abs(x) % 100);
                if (next > 99) next = next % 100;
            }
            else if (next == 0)
            {
                clicks += 1;
            }
            return next;
        });

        return clicks.ToString();
    }
}
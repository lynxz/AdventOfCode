using Tools;

var day = new Day02();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day02 : DayBase
{
    public Day02() : base("2")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData().Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
        var doubles = new List<long>();
        foreach (var d in data)
        {
            var parts = d.Split('-');
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);

            for (long i = start; i <= end; i++)
            {
                var val = i.ToString();
                if (val.Length % 2 != 0) continue;

                var half = val.Length / 2;
                var left = val.Substring(0, half);
                var right = val.Substring(half, half);
                if (left == right)
                {
                    doubles.Add(Convert.ToInt64(i));
                }
            }
        }

        return doubles.Sum().ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData().Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
        var doubles = new List<long>();
        foreach (var d in data)
        {
            var parts = d.Split('-');
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);

            for (long i = start; i <= end; i++)
            {
                var val = i.ToString();
                var half = val.Length / 2;
                for (int j = 1; j <= half; j++)
                {
                    if (val.Length % j != 0) continue;

                    var s = val.Length / j;
                    var first = val.Substring(0, j);
                    if (Enumerable.Range(0, s).All(x => val.Substring(x * j, j) == first))
                    {
                        doubles.Add(Convert.ToInt64(i));
                        break;
                    }

                }
            }
        }

        return doubles.Sum().ToString();
    }
}
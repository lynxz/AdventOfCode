using Tools;

var day = new Day07();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day07 : DayBase
{

    public Day07() : base("7")
    {
    }

    public override string FirstStar()
    {
        var rows = GetRowData().Select(r => r.GetLongs());
        return rows
            .Where(r => Calc(r.Skip(1), 0, r.First()))
            .Sum(r => r.First())
            .ToString();
    }

    public override string SecondStar()
    {
        var rows = GetRowData().Select(r => r.GetLongs());
        return rows
            .Where(r => Calc(r.Skip(1), 0, r.First(), false))
            .Sum(r => r.First())
            .ToString();
    }

    bool Calc(IEnumerable<long> values, long sum, long match, bool first = true)
    {
        if (sum > match)
        {
            return false;
        }
        if (values.Count() == 0)
        {
            return sum == match;
        }

        var value = values.First();
        return Calc(values.Skip(1), sum + value, match, first)
            || Calc(values.Skip(1), sum == 0 ? value : sum * value, match, first)
            || (!first && Calc(values.Skip(1), sum == 0 ? value : long.Parse($"{sum}{value}"), match, first));
    }
}
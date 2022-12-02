using Tools;

Day02 day = new();
day.OutputSecondStar();

public class Day02 : DayBase
{
    public Day02() : base("2")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(d => d.Split(" ", StringSplitOptions.RemoveEmptyEntries));
        var result = data.Select(d =>  (Elv: d[0][0] - 'A', Me: d[1][0] - 'X'))
            .Sum(d =>  d.Me + 1 + (d.Elv == d.Me ? 3 : d.Me == (d.Elv + 1) % 3 ? 6 : 0) );
        return result.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(d => d.Split(" ", StringSplitOptions.RemoveEmptyEntries));
        var result = data.Select(d =>  (Elv: d[0][0] - 'A', Me: d[1][0] - 'X'))
            .Sum(d => ((d.Elv + ((d.Me + 2) % 3)) % 3) + 1 + (d.Me * 3));
        return result.ToString();
    }
}
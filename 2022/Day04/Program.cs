using Tools;

Day04 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day04 : DayBase
{
    public Day04() : base("4")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.Replace('-', ' ').GetIntegers());
        return data.Count(g => (g[0] <= g[2] && g[1] >= g[3]) || (g[2] <= g[0] && g[3] >= g[1])).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Replace('-', ' ').GetIntegers());
        return data.Count(g => Enumerable.Range(g[0], g[1] - g[0] + 1).Any(i => i >= g[2] && i <= g[3])).ToString();
    }
}
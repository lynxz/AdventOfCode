using Tools;

Day03 day = new();
day.PostSecondStar();

public class Day03 : DayBase
{
    public Day03() : base("3")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var chars = data.Select(d => d[Enumerable.Range(0, d.Length / 2).First(i => d[(d.Length / 2)..].Any(c => c == d[i]))]).ToList();
        return chars.Sum(c => char.IsLower(c) ? c - 'a' + 1 : c - 'A' +27 ).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var groups = Enumerable.Range(0, data.Length/ 3).Select(i => data.Skip(i*3).Take(3).ToArray());
        var chars = groups.Select(g => g[0].First(c => g.All(sg => sg.Contains(c)))).ToList();
        return chars.Sum(c => char.IsLower(c) ? c - 'a' + 1 : c - 'A' +27 ).ToString();
    }
}
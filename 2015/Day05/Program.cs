// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day5();
day.PostSecondStar();

public class Day5 : DayBase
{
    public Day5() : base("5")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var vowels = "aeiou".ToArray();
        var badStrings = "ab cd pq xy".Split(' ');
        return data.Count(l =>
            l.Count(d => vowels.Any(v => v == d)) > 2 &&
            Enumerable.Range(0, l.Length - 1).Any(i => l[i] == l[i + 1]) &&
            !badStrings.Any(bs => l.Contains(bs))
            ).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        return data.Count(l =>
            Enumerable.Range(0, l.Length - 2).Any(i => l[i] == l[i + 2]) &&
            Enumerable.Range(0, l.Length - 1).Where(i => l[i] == l[i+1] ? i + 2 < l.Length - 1 ? l[i] != l[i+2] : true : true).Select(i => (new string(l.Skip(i).Take(2).ToArray()), 1)).GroupBy(x => x.Item1).Any(g => g.Sum(x => x.Item2) > 1)
        ).ToString();
    }
}

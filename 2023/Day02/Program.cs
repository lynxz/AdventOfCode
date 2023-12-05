
using System.Text.RegularExpressions;
using Tools;

Day02 day = new Day02();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day02 : DayBase
{
    public Day02() : base("2")
    {

    }

    public override string FirstStar()
    {

        var data = GetRowData();
        var regex = new Regex(@"(?<value>\d+)\s((?<color>blue)|(?<color>red)|(?<color>green))");
        var limits = new Dictionary<string, int>() { { "red", 12 }, { "green", 13 }, { "blue", 14 } };

        return data
            .Where(r => regex.Matches(r).All(m => int.Parse(m.Groups["value"].Value) <= limits[m.Groups["color"].Value]))
            .Sum(r => r.GetIntegers().First())
            .ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var regex = new Regex(@"(?<value>\d+)\s((?<color>blue)|(?<color>red)|(?<color>green))");

        return data.Select(r => regex.Matches(r).GroupBy(m => m.Groups["color"].Value)
            .Select(g => g.Max(m => int.Parse(m.Groups["value"].Value)))
            .Aggregate(1, (acc, val) => acc * val)).Sum().ToString();
    }
}
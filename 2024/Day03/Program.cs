using System.Text.RegularExpressions;
using Tools;

var day = new Day03();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day03 : DayBase
{
    public Day03() : base("3")
    {
    }

    public override string FirstStar()
    {
        var regEx = new Regex(@"mul\((\d+),(\d+)\)");
        var data = GetRawData();
        return  regEx.Matches(data).Sum(mult).ToString();
    }

    private static long mult(Match m)
    {
        var a = int.Parse(m.Groups[1].Value);
        var b = int.Parse(m.Groups[2].Value);
        return a * b;
    }

    public override string SecondStar()
    {
        var regEx = new Regex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)");
        var data = GetRawData();
        var result = 0L;
        var shouldAdd = true;
        foreach (Match match in regEx.Matches(data))
        {
            if (match.Value == "do()")
            {
                shouldAdd = true;
            }
            else if (match.Value == "don't()")
            {
                shouldAdd = false;
            }
            else
            {
                var m = mult(match);
                if (shouldAdd)
                {
                    result += m;
                }
            }
        }
        return result.ToString();
    }
}
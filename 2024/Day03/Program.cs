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
        // var matches = regEx.Matches(data);
        // var result = 0L;
        // foreach (Match match in matches)
        // {
        //     var a = int.Parse(match.Groups[1].Value);
        //     var b = int.Parse(match.Groups[2].Value);
        //     result += a * b;
        // }
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
        var matches = regEx.Matches(data);
        var result = 0L;
        var shouldAdd = true;
        foreach (Match match in matches)
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
                var a = int.Parse(match.Groups[1].Value);
                var b = int.Parse(match.Groups[2].Value);
                if (shouldAdd)
                {
                    result += a * b;
                }
            }
        }
        return result.ToString();
    }
}
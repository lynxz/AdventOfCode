// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day2();
day.PostSecondStar();

public class Day2 : DayBase
{
    public Day2() : base("2")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(l => l.Split('x').Select(v => int.Parse(v)).ToArray()).ToList();
        var answer = data.Sum(d => 2*d[0]*d[1]+2*d[1]*d[2]+2*d[2]*d[0] + Math.Min(Math.Min(d[0]*d[1], d[0]*d[2]), d[1]*d[2]));
        return answer.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(l => l.Split('x').Select(v => int.Parse(v)).ToArray()).ToList();
        var answer = data.Sum(d => 2*Math.Min(d[0], d[1]) + (d[0] < d[1] ? 2*Math.Min(d[1], d[2]) : 2*Math.Min(d[0], d[2])) + d[0]*d[1]*d[2]);
        return answer.ToString();
    }
}

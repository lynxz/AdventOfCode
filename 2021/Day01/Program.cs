// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day1();
day.OutputFirstStar();

public class Day1 : DayBase
{
    public Day1() : base("1")
    {
        
    }

    public override string FirstStar()
    {
        var input = GetIntRowData();
        return input.Window(2).Count(d => d[0] < d[1]).ToString();
    }

    public override string SecondStar()
    {
        var input = GetIntRowData();
        return input.Window(3).Select(d => d.Sum()).Window(2).Count(d => d[0] < d[1]).ToString();
    }
}

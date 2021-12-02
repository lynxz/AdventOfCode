// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day2("2");
day.PostSecondStar();

public class Day2 : DayBase
{
    public Day2(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData().Select(d => d.Split(' ')).Select(p => (Command: p[0], Step: int.Parse(p[1]))).ToList();
        var depth = 0;
        var pos = 0;
        foreach (var i in input)
        {
            if (i.Command == "forward")
                pos += i.Step;
            else if (i.Command == "up")
                depth -= i.Step;
            else
                depth += i.Step;
        }

        return (depth * pos).ToString();
    }

    public override string SecondStar()
    {
        var input = GetRowData().Select(d => d.Split(' ')).Select(p => (Command: p[0], Step: int.Parse(p[1]))).ToList();
        var depth = 0;
        var pos = 0;
        var aim = 0;
        foreach (var i in input)
        {
            if (i.Command == "forward")
            {
                pos += i.Step;
                depth += aim * i.Step;
            }

            else if (i.Command == "up")
                aim -= i.Step;
            else
                aim += i.Step;
        }

        return (depth * pos).ToString();
    }
}
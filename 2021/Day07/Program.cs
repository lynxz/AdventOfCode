// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day7("7");
day.OutputSecondStar();

public class Day7 : DayBase
{
    public Day7(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRawData().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();
        var max = input.Max();
        var min = input.Min();
        return Enumerable.Range(min, max - min).Min(i => input.Sum(p => Math.Abs(i - p))).ToString();

    }

    public override string SecondStar()
    {
        var input = GetRawData().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();
        var max = input.Max();
        var min = input.Min();
        return Enumerable.Range(min, max - min).Min(i => input.Sum(p => Math.Abs(i - p) * (Math.Abs(i - p) + 1) / 2)).ToString();
    }
}

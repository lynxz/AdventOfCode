using Tools;

var day = new Day1();
day.PostSecondStar();

public class Day1 : DayBase
{
    public Day1() : base("1")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData();
        var elves = data.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var max = elves.Max(e =>
            e.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(d => int.Parse(d))
            .Sum());
        return max.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData();
        var elves = data.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).ToList();
        var order = elves.Select(e => 
            e.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(d => int.Parse(d))
            .Sum())
            .OrderDescending()
            .ToList();
        return order.Take(3).Sum().ToString();
    }
}

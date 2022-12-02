using Tools;

var day = new Day1();
day.OutputSecondStar();

public class Day1 : DayBase
{
    public Day1() : base("1")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData();
        IEnumerable<int> elves = GetElves(data);
        return elves.Max().ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData();
        var elves = GetElves(data);
        return elves.OrderDescending().Take(3).Sum().ToString();
    }

    private static IEnumerable<int> GetElves(string data) =>
        data.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).Select(e =>
            e.Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(d => int.Parse(d))
            .Sum());
}

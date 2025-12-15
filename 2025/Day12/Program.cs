using Tools;

Day12 day = new();
day.OutputFirstStar();

public class Day12 : DayBase
{
    public Day12() : base("12")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData().Replace("\r", "").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        var sum = 0;
        foreach(var row in data[6].Split("\n", StringSplitOptions.RemoveEmptyEntries))
        {
            var colon = row.IndexOf(':');
            var size = row[..colon].Trim().Split('x').Select(int.Parse).ToArray();
            var numberOfPackages = row[(colon + 1)..].Trim().Split(' ').Select(int.Parse).ToArray();
            var amnt = numberOfPackages.Aggregate(size[0] / 3 * (size[1] / 3), (amnt, packages) => amnt - packages);
            if (amnt >= 0)
                sum++;
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
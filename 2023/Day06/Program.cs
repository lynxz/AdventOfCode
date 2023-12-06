using Tools;

Day06 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day06 : DayBase
{
    public Day06() : base("6")
    {
    }

    public override string FirstStar()
    {
        var rows = GetRowData();
        var times = rows.First().GetIntegers();
        var distances = rows.Last().GetIntegers();

        return Enumerable.Range(0, times.Length)
            .Select(i => Enumerable.Range(0, times[i]).Count(j => j * (times[i] -j ) > distances[i]))
            .Aggregate(1, (acc, val) => acc * val)
            .ToString();
    }

    public override string SecondStar()
    {
       var rows = GetRowData();
        var time = long.Parse(string.Join("", rows.First().GetIntegers()));
        var distance = long.Parse(string.Join("", rows.Last().GetIntegers()));
        var start = Convert.ToInt64((time - Math.Sqrt(time * time - 4 * distance)) / 2);
        var end = Convert.ToInt64((time + Math.Sqrt(time * time - 4 * distance)) / 2);

        return (end - start -1).ToString();
    }

    
}
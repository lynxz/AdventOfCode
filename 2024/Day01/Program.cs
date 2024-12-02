using Tools;

var day = new Day01();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day01 : DayBase
{
    public Day01() : base("1") { }

    public override string FirstStar()
    {
        var data = GetRowData();
        var l1 = data.Select(x => x.GetIntegers()[0]).ToList();
        var l2 = data.Select(x => x.GetIntegers()[1]).ToList();
        l1.Sort();
        l2.Sort();
        return l1.Zip(l2, (a, b) => Math.Abs(a - b)).Sum().ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var l1 = data.Select(x => x.GetIntegers()[0]).ToList();
        var l2 = data.Select(x => x.GetIntegers()[1]).ToList();
        return l1.Select(x => l2.Count(y => y == x) * x).Sum().ToString();
    }
}
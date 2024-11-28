using Tools;

var day = new Day01();
day.OutputFirstStar();

public class Day01 : DayBase
{
    public Day01() : base("1") { }

    public override string FirstStar()
    {
        var data = GetRawData();
        return string.Empty;
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }

}
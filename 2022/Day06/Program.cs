using Tools;

Day06 day = new();
day.PostSecondStar();

public class Day06 : DayBase
{
    public Day06() : base("6")
    {
    }

    public override string FirstStar()=>
        GetPacket(4).ToString();

    public override string SecondStar() =>
        GetPacket(14).ToString();
    
    private int GetPacket(int size)
    {
        var data = GetRawData().Window(size).ToList();
        return Enumerable.Range(0, data.Count).First(i => data[i].Distinct().Count() == size) + size;
    }
}
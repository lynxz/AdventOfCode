// See https://aka.ms/new-console-template for more information
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
        return GetRawData().Aggregate(0, (acc, c) => acc += c == '(' ? 1 : -1).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData();
        var count = 0;
        var floor = 0;
        while (floor >= 0) {
            floor += data[count] == '(' ? 1 : -1;
            count++;
        }
        
        return count.ToString();
    }

}

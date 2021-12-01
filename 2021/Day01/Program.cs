// See https://aka.ms/new-console-template for more information
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
        var input = GetIntRowData();
        return Enumerable.Range(0, input.Length-1).Count( i => input[i] < input[i+1]).ToString();
    }

    public override string SecondStar()
    {
        var input = GetIntRowData();
        return Enumerable.Range(0, input.Length-3).Count( i => input.Skip(i).Take(3).Sum() < input.Skip(i+1).Take(3).Sum()).ToString();
    }
}

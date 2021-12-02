// See https://aka.ms/new-console-template for more information
using Tools;

Console.WriteLine("Hello, World!");

var day = new Day3("3");
day.PostFirstStar();

public class Day3 : DayBase
{
    public Day3(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        throw new NotImplementedException();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}

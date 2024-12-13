using Tools;

var day = new Day13();
day.OutputFirstStar();

public class Day13 : DayBase
{
    public Day13() : base("13")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData();
        var machineData = data.Split("\n\n");
        var sum = 0;
        foreach(var machine in machineData)
        {
            var lines = machine.Split("\n");
            var buttonA = lines[0].GetIntegers();
            var buttonB = lines[1].GetIntegers();
            var prize = lines[2].GetIntegers();
            
            var a =0;
            var b = 0;

            
            System.Console.WriteLine(string.Join(",", buttonA));
            System.Console.WriteLine(string.Join(",", buttonB));
            System.Console.WriteLine(string.Join(",", prize));
            System.Console.WriteLine();
            
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
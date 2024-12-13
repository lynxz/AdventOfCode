using System.Numerics;
using Tools;

var day = new Day13();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day13 : DayBase
{
    public Day13() : base("13")
    {
    }

    public override string FirstStar()
    {
        return CalculateTokens(true).ToString();
    }

    public override string SecondStar()
    {
        return CalculateTokens(false).ToString();
    }

    private BigInteger CalculateTokens(bool first)
    {
        var data = GetRawData();
        var machineData = data.Split("\n\n");
        var sum = new BigInteger(0);
        foreach (var machine in machineData)
        {
            var lines = machine.Split("\n");
            var buttonA = lines[0].GetIntegers().Select(i => new BigInteger(i)).ToArray();
            var buttonB = lines[1].GetIntegers().Select(i => new BigInteger(i)).ToArray();
            var prize = lines[2].GetIntegers().Select(i => new BigInteger(first ? i : 10000000000000 + i)).ToArray();


            if (BigInteger.Abs(buttonA[1] * prize[0] - buttonA[0] * prize[1]) % BigInteger.Abs(buttonA[1] * buttonB[0] - buttonA[0] * buttonB[1]) == 0)
            {
                var b = (buttonA[1] * prize[0] - buttonA[0] * prize[1]) / (buttonA[1] * buttonB[0] - buttonA[0] * buttonB[1]);
                if (BigInteger.Abs(prize[1] - buttonB[1] * b) % buttonA[1] == 0)
                {
                    var a = (prize[1] - buttonB[1] * b) / buttonA[1];
                    sum += a * 3 + b;
                }
            }
        }

        return sum;
    }
}
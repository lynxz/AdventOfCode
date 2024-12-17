using System.ComponentModel;
using Tools;

var day = new Day17();
day.OutputFirstStar();

public class Day17 : DayBase
{
    public Day17() : base("17")
    {
    }

    public override string FirstStar()
    {
        var computer = new Computer(25986278, 0, 0);
        var program = "2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0".GetIntegers();

        var result = computer.Run(program);
        return string.Join(",", result);
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}

public class Computer {

    private int a = 0;
    private int b = 0;
    private int c = 0;

    private int p = 0;
    private List<int> outValues = new List<int>();

    private Dictionary<int, Action<int>> ops;

    public Computer(params int[] values) : this()
    {
        a = values[0];
        b = values[1];
        c = values[2];
    }

    public Computer()
    {
        ops = new Dictionary<int, Action<int>>()
        {
            { 0, (int value) => a = Division(GetOperand(value)) },
            { 1, (int value) => b = b ^ GetOperand(value) },
            { 2, (int value) => b = GetOperand(value) % 8 },
            { 3, (int value) => p = a == 0  ? p : GetOperand(value) },
            { 4, (int value) => b = b ^ c },
            { 5, (int value) => outValues.Add(GetOperand(value) % 8) },
            { 6, (int value) => b = Division(GetOperand(value)) },
            { 7, (int value) => c = Division(GetOperand(value)) }
        };
    }

    private int Division(int value) {
        var la = Convert.ToDecimal(a);
        for (int i = 0; i < value; i++)
        {
            la = la / 2m;
            if (la < 1)
            {
                break;
            }
        }
        return Convert.ToInt32(Math.Floor(la));
    }

    public List<int> Run(int[] values)
    {
        while (p < values.Length)
        {
            var opCode = values[p];
            var operand = values[p + 1];
            p += 2;
            ops[opCode](operand);
        }
        return outValues;
    }


    private int GetOperand(int value) =>
        value switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => a, 
            5 => b,
            6 => c,
            _ => throw new ArgumentException()
        };


}
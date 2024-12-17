using Tools;

var day = new Day17();
day.OutputSecondStar();

public class Day17 : DayBase
{
    public Day17() : base("17")
    {
    }

    public override string FirstStar()
    {
        var computer = new Computer(35, 0, 0);
        var program = "2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0".GetIntegers();

        var result = computer.Run(program);
        return string.Join(",", result);
    }

    public override string SecondStar()
    {

        var stack = new Stack<(ulong, ulong)>();
        var program = "2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0";
        var values = program.GetIntegers();
        var output = string.Empty;
        stack.Push((1, 0));
        ulong a = 0;
        ulong i = 0;
        while (program != output)
        {
            var ii = 0;
            (a, i) = stack.Pop();
            output = string.Empty;
            while (program != output)
            {
                var la = i + a;
                var computer = new Computer(la, 0, 0);
                var result = computer.Run(values);
                output = string.Join(",", result);
                i++;
                if (program.EndsWith(output))
                {
                    a = la;
                    stack.Push((a, i + 1));
                    a = a << 1;
                    i = 0;
                }
                if (i > 7)
                {
                    a = a << 1;
                    ii++;
                    i = 0;
                }
                if (ii > 3) {
                    break;
                }
            }
        }


        return a.ToString();
    }
}

public class Computer
{

    private ulong a = 0;
    private ulong b = 0;
    private ulong c = 0;

    private ulong p = 0;
    private List<ulong> outValues = new List<ulong>();

    private Dictionary<ulong, Action<ulong>> ops;

    public Computer(params ulong[] values) : this()
    {
        a = values[0];
        b = values[1];
        c = values[2];
    }

    public Computer()
    {
        ops = new Dictionary<ulong, Action<ulong>>()
        {
            { 0, (ulong value) => a = Division(GetOperand(value)) },
            { 1, (ulong value) => b = b ^ value },
            { 2, (ulong value) => b = GetOperand(value) % 8 },
            { 3, (ulong value) => p = a == 0  ? p : value },
            { 4, (ulong value) => b = b ^ c },
            { 5, (ulong value) => outValues.Add(GetOperand(value) % 8) },
            { 6, (ulong value) => b = Division(GetOperand(value)) },
            { 7, (ulong value) => c = Division(GetOperand(value)) }
        };
    }

    public List<ulong> Run(int[] values)
    {
        var v = values.Select(i => Convert.ToUInt64(i)).ToArray();
        while (p < (ulong)values.Length)
        {
            var opCode = v[p];
            var operand = v[p + 1];
            p += 2;
            ops[opCode](operand);
        }
        return outValues;
    }

    private ulong GetOperand(ulong value) =>
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

    private ulong Division(ulong value)
    {
        ulong acc = 1;
        for (ulong i = 0; i < value; i++)
        {
            acc *= 2;
            if (acc > a)
            {
                return 0;
            }
        }
        return a / acc;
    }

}
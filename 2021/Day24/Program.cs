// See https://aka.ms/new-console-template for more information
using Tools;

Day24 day = new("24");
day.OutputSecondStar();

public class Day24 : DayBase
{
    public Day24(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var instructions = GetRowData().Select(r => r.Trim()).ToList();
        var result = Search(instructions, 0, 0);
        result.Sort();

        return result.Last();
    }

    Dictionary<(int Level, long PrevZ), List<string>> cacheDic = new();

    List<string> Search(List<string> instructions, int level, long prevZ)
    {
        if (cacheDic.ContainsKey((level, prevZ)))
            return cacheDic[(level, prevZ)];

        if (level >= 14)
        {
            if (prevZ == 0) 
                return new() { "" };
            return null;
        }

        List<string> result = new();

        var nextX = instructions[level * 18 + 5].GetIntegers().First() + prevZ % 26;

        if (nextX < 10 && nextX > 0)
        {

            ALU alu = new(() => nextX, prevZ);
            alu.Run(instructions.Skip(18 * level).Take(18).ToList());
            var nextZ = alu["z"];
            var nextStrings = Search(instructions, level + 1, nextZ);
            if (null != nextStrings)
                result.AddRange(nextStrings.Select(s => $"{nextX}{s}"));
        }
        else
        {
            for (int i = 1; i < 10; i++)
            {
                ALU alu = new(() => i, prevZ);
                alu.Run(instructions.Skip(18 * level).Take(18).ToList());
                var nextZ = alu["z"];
                var nextStrings = Search(instructions, level + 1, nextZ);
                if (null != nextStrings)
                    result.AddRange(nextStrings.Select(s => $"{i}{s}"));
            }
        }

        cacheDic.Add((level, prevZ), result);
        return result;
    }

    public override string SecondStar()
    {
        var instructions = GetRowData().Select(r => r.Trim()).ToList();
        var result = Search(instructions, 0, 0);
        result.Sort();

        return result.First();
    }
}

public class ALU
{

    Func<long> _read;
    Dictionary<string, long> _variables;

    public ALU(Func<long> read, long initialZ = 0L)
    {
        _read = read;
        _variables = new Dictionary<string, long> {
            {"w", 0L},
            {"x", 0L},
            {"y", 0L},
            {"z", initialZ}
        };
    }

    public void Run(List<string> instructions)
    {
        var inputCounter = 0;
        foreach (var instr in instructions)
        {
            var op = instr.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (op[0] == "inp")
                inputCounter++;
            long val = 0;
            if (op.Length == 3)
                if (!long.TryParse(op[2], out val))
                    val = _variables[op[2]];

            _variables[op[1]] = GetValue(op[0], _variables[op[1]], val);
        }
    }

    public long this[string var] => _variables[var];

    public bool IsValid() => _variables["z"] == 0;

    long GetValue(string op, long val1, long val2) =>
        op switch
        {
            "inp" => Input(),
            "add" => Add(val1, val2),
            "mul" => Mult(val1, val2),
            "div" => Div(val1, val2),
            "mod" => Mod(val1, val2),
            "eql" => Equal(val1, val2),
            _ => throw new Exception("Invalid op " + op)
        };

    long Input() => _read();

    long Add(long a, long b) => a + b;

    long Mult(long a, long b) => a * b;

    long Div(long a, long b) => a / b;

    long Mod(long a, long b) => a % b;

    long Equal(long a, long b) => a == b ? 1L : 0L;

}
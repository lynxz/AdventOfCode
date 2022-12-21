using System.Text.RegularExpressions;
using Tools;

Day21 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public partial class Day21 : DayBase
{

    public Day21() : base("21")
    {

    }

    public override string FirstStar()
    {
        var data = GetRowData().ToDictionary(r => r[0..4], r => r[6..].Trim());
        var result = RootMonkey("root", data);

        return result.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().ToDictionary(r => r[0..4], r => r[6..].Trim());
        var op = data["root"];
        var m = OpRegex().Match(op);
        var m1 = m.Groups["m1"].Value;
        var m2 = m.Groups["m2"].Value;

        var humn = ContainsHumn(m1, data) ? m1 : m2;
        var val = RootMonkey(humn == m1 ? m2 : m1, data);
        var result = ReverseMonkey(humn, val, data);

        return result.ToString();
    }

    long ReverseMonkey(string name, long num, Dictionary<string, string> monkeys)
    {
        if (name == "humn")
            return num;

        var m = OpRegex().Match(monkeys[name]);
        var m1 = m.Groups["m1"].Value;
        var m2 = m.Groups["m2"].Value;
        bool first = ContainsHumn(m1, monkeys);

        var val = RootMonkey(first ? m2 : m1, monkeys);

        var result = m.Groups["s"].Value switch
        {
            "+" => num - val,
            "-" => first ? num + val : val - num,
            "*" => num / val,
            "/" => first ? val * num : val / num,
            _ => throw new Exception()
        };

        return ReverseMonkey(first ? m1 : m2, result, monkeys);
    }

    bool ContainsHumn(string name, Dictionary<string, string> monkeys)
    {
        if (name == "humn")
            return true;

        var op = monkeys[name];
        if (long.TryParse(op, out _))
            return false;

        var m = OpRegex().Match(op);
        return ContainsHumn(m.Groups["m1"].Value, monkeys) || ContainsHumn(m.Groups["m2"].Value, monkeys);
    }

    long RootMonkey(string name, Dictionary<string, string> monkeys)
    {
        var op = monkeys[name];
        if (long.TryParse(op, out long num))
            return num;

        var m = OpRegex().Match(op);
        var val1 = RootMonkey(m.Groups["m1"].Value, monkeys);
        var val2 = RootMonkey(m.Groups["m2"].Value, monkeys);

        var result = m.Groups["s"].Value switch
        {
            "+" => val1 + val2,
            "-" => val1 - val2,
            "*" => val1 * val2,
            "/" => val1 / val2,
            _ => throw new Exception()
        };

        monkeys[name] = result.ToString();
        return result;
    }

    [GeneratedRegex("(?<m1>\\w{4})\\s(?<s>\\+?-?\\*?/?)\\s(?<m2>\\w{4})")]
    private static partial Regex OpRegex();
}
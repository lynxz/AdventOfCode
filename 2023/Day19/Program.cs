using Microsoft.VisualBasic;
using Tools;

Day19 day = new();
day.OutputFirstStar();

public class Day19 : DayBase
{
    public Day19() : base("19")
    {
    }

    Dictionary<char, int> Map = new Dictionary<char, int> {
        { 'x', 0 },
        { 'm', 1 },
        { 'a', 2 },
        { 's', 3 }
    };

    public override string FirstStar()
    {
        var input = GetRawData().Replace("\r", "");
        var rules = input.Split("\n\n")[0].Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var parts = input.Split("\n\n")[1].Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var functions = new Dictionary<string, Func<int[], string>>();

        foreach (var rule in rules)
        {
            var brack = rule.IndexOf("{");
            var name = rule[..brack];
            var process = rule[(brack + 1)..^1];
            var pps = process.Split(",", StringSplitOptions.RemoveEmptyEntries);

            List<Func<int[], string>> funcs = new();
            foreach (var pp in pps)
            {

                if (pp.Contains("<"))
                {
                    var l = pp[0];
                    var r = pp[(pp.IndexOf(":") + 1)..];
                    var i = pp.GetIntegers().Single();
                    Func<int[], string> cf = f => f[Map[l]] < i ? r : string.Empty;
                    funcs.Add(cf);
                }
                else if (pp.Contains(">"))
                {
                    var l = pp[0];
                    var r = pp[(pp.IndexOf(":") + 1)..];
                    var i = pp.GetIntegers().Single();
                    Func<int[], string> cf = f => f[Map[l]] > i ? r : string.Empty;
                    funcs.Add(cf);
                }
                else
                {
                    funcs.Add(f => pp);
                }
            }

            Func<int[], string> aggFunc = f => {
                foreach (var func in funcs)
                {
                    var res = func(f);
                    if (res != string.Empty)
                    {
                        return res;
                    }
                }
                throw new InvalidOperationException();
            };

            functions.Add(name, aggFunc);

        }

        List<int[]> accepted = new List<int[]>();

        foreach (var part in parts)
        {
            var p = part.GetIntegers();
            var f = "in";
            var done = false;
            while (!done)
            {
                var fun = functions[f];
                f = fun(p);
                if (f == "A")
                {
                    accepted.Add(p);
                    done = true;
                }
                if (f == "R")
                {
                    done = true;
                }
            }
        }

        return accepted.Sum(a => a.Sum()).ToString();

    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
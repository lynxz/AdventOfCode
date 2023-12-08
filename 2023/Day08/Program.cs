using Tools;

Day08 day = new();
day.OutputSecondStar();

public class Day08 : DayBase
{
    public Day08() : base("8") { }

    public override string FirstStar()
    {
        var data = GetRowData();

        var instr = data.First();
        var nodes = data.Skip(1).Select(r => r.Split(" = "))
            .ToDictionary(r => r[0], r => r[1].Trim(new char[] { '(', ')' }).Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).ToList());

        var curr = "AAA";
        var step = 0;


        while (curr != "ZZZ")
        {
            var dir = instr[step % instr.Length] == 'L' ? 0 : 1;
            curr = nodes[curr][dir];
            step++;
        }


        return step.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();

        var instr = data.First();
        var nodes = data.Skip(1).Select(r => r.Split(" = "))
            .ToDictionary(r => r[0], r => r[1].Trim(new char[] { '(', ')' }).Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).ToList());

        var curr = nodes.Where(n => n.Key.Last() == 'A').Select(n => n.Key).ToArray();

        List<int> cycles = new();
        foreach (var n in curr)
        {
            var prevZ = string.Empty;
            var prevS = 0;
            var s = 0;
            var c = n;
            var stab = 0;

            while (c != prevZ && stab < 2)
            {
                if (c.Last() == 'Z')
                {
                    prevZ = c;
                    prevS = s;
                    stab++;
                }

                var dir = instr[s % instr.Length] == 'L' ? 0 : 1;
                s++;
                c = nodes[c][dir];
            }
            cycles.Add(s - prevS);
        }

            return LCM(cycles.Select(c => (long)c).ToArray()).ToString();
        }


        static long LCM(long[] numbers)
        {
            return numbers.Aggregate(lcm);
        }
        static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
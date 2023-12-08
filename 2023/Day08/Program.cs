using System.Diagnostics;
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
        var step = 0;


        var memo = new Dictionary<string, int>();
        var cycles = new Dictionary<string, List<int>>();
        foreach (var c in curr)
        {
            var start = c;
            var b = c;
            var count = 0;
            var currZ = "";
            var currZStep = 0;
            List<int> zeds = new();
            do
            {
                var dir = instr[count % instr.Length] == 'L' ? 0 : 1;
                b = nodes[b][dir];
                count++;
                if (b.Last() == 'Z')
                {
                    zeds.Add(count);
                }
            } while (b != start);
            cycles.Add(c, zeds);
        }

        // while (!curr.All(n => n.Last() == 'Z'))
        // {
        //     var next = new List<string>();
        //     var dir = instr[step % instr.Length] == 'L' ? 0 : 1;
        //     for (int i = 0; i < curr.Length; i++)
        //     {
        //         var c = curr[i];

        //         curr[i] = nodes[c][dir];
        //     }

        //     step++;
        // }

        return step.ToString();
    }
}
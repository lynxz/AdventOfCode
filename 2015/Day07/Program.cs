// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day7("7");
day.PostSecondStar();

public class Day7 : DayBase
{
    public Day7(string day) : base(day)
    {

    }

    public override string FirstStar()
    {
        Dictionary<string, Func<ushort>> sim = GenerateSim();

        return sim["a"]().ToString();
    }

    private Dictionary<string, Func<ushort>> GenerateSim()
    {
        var data = GetRowData();
        var sim = new Dictionary<string, Func<UInt16>>();
        var mem = new Dictionary<string, UInt16>();

        foreach (var row in data)
        {
            var comp = row.Split(' ');
            var input = comp.Last();
            if (comp.Length == 3 && UInt16.TryParse(comp[0], out UInt16 val))
            {
                sim.Add(input, () => val);
            }
            else if (comp.Length == 3)
            {
                sim.Add(input, () => GetValue(sim, mem, comp[0]));
            }
            else if (comp[1] == "AND")
            {
                if (UInt16.TryParse(comp[0], out UInt16 andVal))
                    sim.Add(input, () => (UInt16)(andVal & GetValue(sim, mem, comp[2])));
                else
                    sim.Add(input, () => (UInt16)(GetValue(sim, mem, comp[0]) & GetValue(sim, mem, comp[2])));
            }
            else if (comp[1] == "OR")
            {
                if (UInt16.TryParse(comp[0], out UInt16 orVal))
                    sim.Add(input, () => (UInt16)(orVal | GetValue(sim, mem, comp[2])));
                else
                    sim.Add(input, () => (UInt16)(GetValue(sim, mem, comp[0]) | GetValue(sim, mem, comp[2])));
            }
            else if (comp[0] == "NOT")
            {
                sim.Add(input, () => (UInt16)(~GetValue(sim, mem, comp[1])));
            }
            else if (comp[1] == "LSHIFT")
            {
                var steps = int.Parse(comp[2]);
                sim.Add(input, () => (UInt16)(GetValue(sim, mem, comp[0]) << steps));
            }
            else if (comp[1] == "RSHIFT")
            {
                var steps = int.Parse(comp[2]);
                sim.Add(input, () => (UInt16)(GetValue(sim, mem, comp[0]) >> steps));
            }
        }

        return sim;
    }

    private static ushort GetValue(Dictionary<string, Func<ushort>> sim, Dictionary<string, ushort> mem,  string input)
    {
        if (!mem.ContainsKey(input))
            mem.Add(input, sim[input]());
        return mem[input];
    }

    public override string SecondStar()
    {
        Dictionary<string, Func<ushort>> sim = GenerateSim();

        sim["b"] = () => (UInt16)3176;

        return sim["a"]().ToString();
    }
}
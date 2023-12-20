using System.Security.Cryptography;
using Tools;

Day20 day = new();
day.OutputSecondStar();

public class Day20 : DayBase
{
    public Day20() : base("20")
    {
    }

    public override string FirstStar()
    {
        throw new NotImplementedException();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var mods = new Dictionary<string, List<string>>();
        var flips = new Dictionary<string, bool>();
        var conj = new Dictionary<string, Dictionary<string, int>>();

        foreach (var row in data)
        {
            var parts = row.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var name = parts[0];
            var con = parts[1].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            if (name[0] == '%')
            {
                name = name[1..];
                flips.Add(name, false);
            }
            else if (name[0] == '&')
            {
                name = name[1..];
                conj.Add(name, new Dictionary<string, int>());
            }
            mods.Add(name, con);
        }

        foreach (var c in conj)
        {
            mods.Where(m => m.Value.Contains(c.Key)).ToList().ForEach(m => c.Value.Add(m.Key, 0));
        }

        var queue = new Queue<(string Source, string Reciever, int Signal)>();
        var history = new List<(int Step, string Sender, int Signal)>();

        for (int i = 0; i < 15000; i++)
        {
            queue.Enqueue(("button", "broadcaster", 0));
            while (queue.Count > 0)
            {
                var (source, reciever, signal) = queue.Dequeue();

                if (reciever == "kj")
                    history.Add((i, source, signal));

                var output = -1;
                if (flips.ContainsKey(reciever))
                {
                    if (signal == 0)
                    {
                        flips[reciever] = !flips[reciever];
                        output = flips[reciever] ? 1 : 0;
                    }
                }
                else if (conj.TryGetValue(reciever, out var dict))
                {
                    dict[source] = signal;
                    output = dict.Values.All(i => i == 1) ? 0 : 1;
                }
                else
                {
                    output = signal;
                }

                if (output != -1)
                {
                    if (mods.TryGetValue(reciever, out var cons))
                    {
                        cons.ForEach(c => queue.Enqueue((reciever, c, output)));
                    }
                }
            }
        }
        var groups = history.Where(h => h.Signal != 0).GroupBy(h => h.Sender);
        var cycles = groups.Select(g => g.Skip(1).Take(2).Select(p => p.Step).ToArray()).Select(v => v[1] - v[0]).ToArray();

        return Algo.LCM(cycles.Select(c => (long)c).ToArray()).ToString();
    }
    
}
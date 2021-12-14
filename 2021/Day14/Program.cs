// See https://aka.ms/new-console-template for more information
using System.Text;
using Tools;

Day14 day = new("14");
day.OutputSecondStar();

public class Day14 : DayBase
{
    public Day14(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var parts = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var inserts = parts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split(" -> ", StringSplitOptions.TrimEntries))
            .ToDictionary(p => p[0], p => $"{p[1]}{p[0][1]}");

        var polymer = parts[0];

        for (int i = 0; i < 10; i++)
        {
            StringBuilder sb = new();
            sb.Append(polymer[0]);
            polymer.Window(2).Select(s => new string(s)).ToList().ForEach(s => sb.Append(inserts[s]));
            polymer = sb.ToString();
        }

        var frequency = polymer.GroupBy(c => c);
        return (frequency.Max(g => g.Count()) - frequency.Min(g => g.Count())).ToString();

    }

    Dictionary<string, Dictionary<int, Dictionary<char, ulong>>> stats = new();

    public override string SecondStar()
    {
        var parts = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var inserts = parts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split(" -> ", StringSplitOptions.TrimEntries))
            .ToDictionary(p => p[0], p => $"{p[1]}{p[0][1]}");

        var polymer = parts[0];

        var pairs = inserts.Keys.ToList();

        var lookup = pairs.ToDictionary(p => p, p => Enumerable.Range(0, 10).Aggregate(p, (acc, i) =>
        {
            StringBuilder sb = new();
            sb.Append(acc[0]);
            acc.Window(2).Select(s => new string(s)).ToList().ForEach(s => sb.Append(inserts[s]));
            return sb.ToString();
        }));

        lookup.ToList().ForEach(kvp => stats.Add(kvp.Key, new Dictionary<int, Dictionary<char, ulong>> { { 1, kvp.Value.GroupBy(x => x).ToDictionary(g => g.Key, g => (ulong)g.Count()) } }));
        BuildStats(lookup, polymer, 4);


        var frequency = polymer.Window(2).Select(p => new string(p)).Aggregate(new Dictionary<char, ulong>(), (acc, p) =>
        {
            foreach (var kvp in stats[p][4])
            {
                if (!acc.ContainsKey(kvp.Key))
                    acc.Add(kvp.Key, 4);
                acc[kvp.Key] += kvp.Value;
            }
            return acc;
        });

        polymer.Skip(1).Take(polymer.Length - 2).ToList().ForEach(c => frequency[c]--);

        return (frequency.Max(g => g.Value) - frequency.Min(g => g.Value)).ToString();
    }

    void BuildStats(Dictionary<string, string> lookup, string substring, int level)
    {
        var parts = substring.Window(2).Select(p => new string(p)).ToList();
        for (int i = 0; i < parts.Count; i++)
        {
            var part = parts[i];
            if (!stats[part].ContainsKey(level))
            {
                BuildStats(lookup, lookup[part], level - 1);
                var dic = lookup[part].Window(2).Select(p => new string(p)).Aggregate(new Dictionary<char, ulong>(), (acc, p) =>
                {
                    foreach (var kvp in stats[p][level - 1])
                    {
                        if (!acc.ContainsKey(kvp.Key))
                            acc.Add(kvp.Key, 0);
                        acc[kvp.Key] += kvp.Value;
                    }
                    return acc;
                });
                lookup[part].Skip(1).Take(lookup[part].Length - 2).ToList().ForEach(c => dic[c]--);
                stats[part].Add(level, dic);
            }
        }
    }
}
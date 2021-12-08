// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day8("8");
day.OutputSecondStar();


public class Day8 : DayBase
{
    public Day8(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData();
        var sum = 0;
        foreach (var row in input)
        {
            var i = row.IndexOf('|');
            var p = row.Substring(i + 1).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            sum += p.Where(d => d.Length == 2 || d.Length == 4 || d.Length == 7 || d.Length == 3).Count();
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        var input = GetRowData();
        var sum = 0;
        foreach (var row in input)
        {
            var i = row.IndexOf('|');
            var s = row.Substring(0, i).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var p = row.Substring(i + 1).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var wires = GetDict(s);
            var signals = Solve(wires, s.Where(d => d.Length == 6).ToArray());
            var value = GetValue(signals, p);
            sum += value;
        }

        return sum.ToString();
    }

    private int GetValue(Dictionary<char, int> solution, string[] p)
    {
        var nums = new Dictionary<int, List<int>> {
            {0, new List<int> {0, 1, 2, 4, 5, 6}},
            {1, new List<int> {2, 5}},
            {2, new List<int> {0, 2, 3, 4, 6}},
            {3, new List<int> {0, 2, 3, 5, 6}},
            {4, new List<int> {1, 2, 3, 5}},
            {5, new List<int> {0, 1, 3, 5, 6}},
            {6, new List<int> {0, 1, 3, 4, 5, 6}},
            {7, new List<int> {0, 2, 5}},
            {8, new List<int> {0, 1, 2, 3, 4, 5, 6}},
            {9, new List<int> {0, 1, 2, 3, 5, 6}}
        };

        var result = 0;
        foreach (var s in p)
        {
            result *= 10;
            var segments = s.Select(c => solution[c]).ToList();
            result += nums.Single(n => n.Value.Count == segments.Count && n.Value.All(i => segments.Contains(i))).Key;
        }
        return result;
    }

    public Dictionary<char, int> Solve(Dictionary<char, List<int>> wires, string[] values)
    {
        var w = wires.Where(kvp => kvp.Value.Contains(5)).Single(kvp => values.All(s => s.Contains(kvp.Key)));
        wires[w.Key] = new List<int> { 5 };
        foreach (var notThisKvp in wires.Where(e => e.Key != w.Key))
        {
            notThisKvp.Value.Remove(5);
        }
        w = wires.Where(kvp => kvp.Value.Contains(1)).Single(kvp => values.All(s => s.Contains(kvp.Key)));
        wires[w.Key] = new List<int> { 1 };
        foreach (var notThisKvp in wires.Where(e => e.Key != w.Key))
        {
            notThisKvp.Value.Remove(1);
        }
        w = wires.Where(kvp => kvp.Value.Contains(6)).Single(kvp => values.All(s => s.Contains(kvp.Key)));
        wires[w.Key] = new List<int> { 6 };
        foreach (var notThisKvp in wires.Where(e => e.Key != w.Key))
        {
            notThisKvp.Value.Remove(6);
        }

        return wires.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Single());
    }

    public Dictionary<char, List<int>> GetDict(string[] data)
    {
        var wires = new Dictionary<char, List<int>>();

        data.Single(s => s.Length == 2).ToList().ForEach(c => wires.Add(c, new List<int> { 2, 5 }));
        wires.Add((data.Single(s => s.Length == 3).Single(c => !wires.ContainsKey(c))), new List<int> { 0 });
        data.Single(s => s.Length == 4).Where(c => !wires.ContainsKey(c)).ToList().ForEach(c => wires.Add(c, new List<int> { 1, 3 }));
        data.Where(s => s.Length > 4).SelectMany(s => s).Distinct().Where(c => !wires.ContainsKey(c)).ToList().ForEach(c => wires.Add(c,  new List<int> { 4, 6 }));
        return wires;
    }
}
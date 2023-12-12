using System.Runtime.InteropServices;
using Tools;

Day12 day = new();
//day.OutputFirstStar();
day.OutputSecondStar();

public class Day12 : DayBase
{
    public Day12() : base("12")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var s = data.Select(r => (r.GetIntegers(), r.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0])).ToList();

        Enumerable.Range(0, s.Count).ToList().ForEach(i => System.Console.WriteLine($"{data[i]} {Combinations(s[i], 0, 0, new Dictionary<(int, int), long>())}"));

        return s.Sum(spring => Combinations(spring, 0, 0, new Dictionary<(int, int), long>())).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var s = data.Select(r => (Enumerable.Repeat(r.GetIntegers(), 5).SelectMany(x => x).ToArray(), string.Join('?', Enumerable.Repeat(r.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0], 5)))).ToList();

        Enumerable.Range(0, s.Count).Take(15).ToList().ForEach(i => System.Console.WriteLine($"{data[i]} {Combinations(s[i], 0, 0, new Dictionary<(int, int), long>())}"));

        return s.Sum(spring => (long)Combinations(spring, 0, 0, new Dictionary<(int, int), long>())).ToString();
    }

    long Combinations((int[] i, string s) spring, int index, int pos, Dictionary<(int, int), long> memo)
    {
        var (i, s) = spring;
        if (index >= i.Length && pos < s.Length && Enumerable.Range(pos, s.Length - pos).Any(j => s[j] == '#'))
            return 0;
        if (index >= i.Length)
            return 1;
        if (i[index] > s.Length - pos)
            return 0;
        if (i[index] == s.Length - pos && index == i.Length - 1)
            return Enumerable.Range(pos, s.Length - pos).All(j => s[j] == '#' || s[j] == '?') ? 1 : 0;

        var left = i.Skip(index + 1).Sum();
        var offset = 0;
        var result = 0L;
        do
        {
            if (memo.TryGetValue((pos + offset, index), out var val))
            {
                result += val;
            }
            else
            {
                var s2 = s.Substring(pos + offset, i[index]);
                if (s2.All(c => c == '#' || c == '?'))
                {
                    if (pos + offset + i[index] == s.Length)
                    {
                        var combs = Combinations(spring, index + 1, pos + offset + i[index] + 1, memo);
                        memo.Add((pos + offset, index), combs);
                        result += combs;
                    }
                    else if (pos + offset + i[index] < s.Length && s[pos + offset + i[index]] != '#')
                    {
                        var combs = Combinations(spring, index + 1, pos + offset + i[index] + 1, memo);
                        memo.Add((pos + offset, index), combs);
                        result += combs;
                    }
                }
            }

            offset++;
        } while (offset + pos + left < s.Length && offset + pos + i[index] <= s.Length && Enumerable.Range(pos, offset).All(j => s[j] != '#'));

        return result;
    }
}
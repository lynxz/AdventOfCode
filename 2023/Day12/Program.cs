using Tools;

Day12 day = new();
day.OutputFirstStar();

public class Day12 : DayBase
{
    public Day12() : base("12")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();

        var s = data.Select(r => (r.GetIntegers(), r.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0])).ToList();

        foreach (var b in s)
        {
            Console.WriteLine($"{b.Item2} ({Combinations(b, 0, 0)})");
        }


        return s.Sum(spring => Combinations(spring, 0, 0)).ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }

    int Combinations((int[] i, string s) spring, int index, int pos)
    {
        var (i, s) = spring;
        if (index >= i.Length)
            return 1;
        if (i[index] > s.Length - pos)
            return 0;
        if (i[index] == s.Length - pos && index == i.Length - 1)
            return 1;

        var left = i.Skip(index + 1).Sum();
        var offset = 0;
        var result = 0;
        do
        {

            var s2 = s.Substring(pos + offset, i[index]);
            if (s2.All(c => c == '#' || c == '?'))
            {
                if (pos + offset + i[index] == s.Length)
                {
                    result += Combinations(spring, index + 1, pos + offset + i[index] + 1);
                }
                else if (pos + offset + i[index] < s.Length && s[pos + offset + i[index]] != '#')
                {
                    result += Combinations(spring, index + 1, pos + offset + i[index] + 1);
                }
            }
            offset++;
        } while (offset + pos + left < s.Length - 1 && offset + pos + i[index] < s.Length);

        return result;
    }
}
using Tools;

Day13 day = new();
day.OutputSecondStar();

public class Day13 : DayBase
{
    public Day13() : base("13")
    {

    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var arrays = data.SelectMany(d => d.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(d => ParseList(d))).ToList();
        var pairs = Enumerable.Range(0, arrays.Count / 2).Select(i => arrays.Skip(i * 2).Take(2).ToArray()).ToArray();

        return Enumerable.Range(1, pairs.Length)
            .Where(i => Compare(pairs[i - 1][0], pairs[i - 1][1])!.Value)
            .Select(i => i).Sum()
            .ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var arrays = data.SelectMany(d => d.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(d => ParseList(d))).ToList();
        var div1 = new List<object> { new List<object> { 2 } };
        var div2 = new List<object> { new List<object> { 6 } };
        arrays.Add(div1);
        arrays.Add(div2);

        int index = 0;
        while (index < arrays.Count - 1)
        {
            if (!Compare(arrays[index], arrays[index + 1])!.Value)
            {
                (arrays[index + 1], arrays[index]) = (arrays[index], arrays[index + 1]);
                index = 0;
            } else {
                index++;
            }
        }

        var val1 = Enumerable.Range(1, arrays.Count).First(i => !Compare(arrays[i - 1], div1).HasValue);
        var val2 = Enumerable.Range(1, arrays.Count).First(i => !Compare(arrays[i - 1], div2).HasValue);

        return (val1 * val2).ToString();
    }

    bool? Compare(List<object> left, List<object> right)
    {
        var max = Math.Min(left.Count, right.Count);

        for (int i = 0; i < max; i++)
        {
            if (left[i] is int lval1 && right[i] is int rval1)
            {
                if (lval1 > rval1)
                    return false;
                if (lval1 < rval1)
                    return true;
            }
            else if (left[i] is List<object> lval2 && right[i] is List<object> rval2)
            {
                var comp = Compare(lval2, rval2);
                if (comp.HasValue)
                    return comp;
            }
            else if (left[i] is List<object> lval3 && right[i] is int rval3)
            {
                var comp = Compare(lval3, new List<object> { rval3 });
                if (comp.HasValue)
                    return comp;
            }
            else if (left[i] is int lval4 && right[i] is List<object> rval4)
            {
                var comp = Compare(new List<object> { lval4 }, rval4);
                if (comp.HasValue)
                    return comp;
            }
        }

        return left.Count == right.Count ? null : left.Count < right.Count;
    }

    List<object> ParseList(string data)
    {
        var result = new List<object>();
        var a = data[1..(data.Length - 1)];
        var index = 0;
        var offset = 0;
        while (index + offset < a.Length)
        {
            if (char.IsNumber(a[index + offset]))
            {
                offset++;
            }
            if ((index + offset >= a.Length || a[index + offset] == ',') && offset != 0)
            {
                var end = Math.Min(index + offset, a.Length);
                result.Add(int.Parse(a[index..end]));
                index += offset + 1;
                offset = 0;
            }
            else if (a[index + offset] == '[')
            {
                var lastBracketIndex = FindMatchingBracket(a, index + offset, 0);
                var innerList = ParseList(a[(index + offset)..lastBracketIndex]);
                result.Add(innerList);
                index = lastBracketIndex + 1;
                offset = 0;
            }
        }

        return result;
    }

    int FindMatchingBracket(string data, int index, int bracketCount)
    {
        if (data[index] == ']')
            bracketCount--;
        if (data[index] == '[')
            bracketCount++;
        if (bracketCount == 0)
            return ++index;

        return FindMatchingBracket(data, ++index, bracketCount);
    }
}
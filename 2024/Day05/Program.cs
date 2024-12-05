using Tools;

var day = new Day05();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day05 : DayBase
{
    public Day05() : base("5")
    {
    }

    public override string FirstStar()
    {
        var rowData = GetRowData();

        var rules = rowData.TakeWhile(r => r.GetIntegers().Length < 3).Select(r => r.GetIntegers()).ToList();

        var order = new Dictionary<int, List<int>>();
        foreach (var o in rules)
        {
            if (!order.ContainsKey(o[0]))
            {
                order[o[0]] = new List<int>();
            }
            order[o[0]].Add(o[1]);
        }

        var updates = rowData.Skip(rules.Count).Select(r => r.GetIntegers()).ToList();
        var sum = 0;
        foreach (var update in updates)
        {
            var correct = true;
            for (int i = 0; i < update.Length - 1; i++)
            {
                if (!update.Skip(i + 1).All(j => order.ContainsKey(update[i]) && order[update[i]].Contains(j)))
                {
                    correct = false;
                    break;
                }
            }
            if (correct)
            {
                sum += update[update.Length / 2];
            }
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        var rowData = GetRowData().Select(r => r.Trim()).ToList();

        var rules = rowData.TakeWhile(r => r.GetIntegers().Length == 2).Select(r => r.GetIntegers()).ToList();

        var order = new Dictionary<int, List<int>>();
        foreach (var o in rules)
        {
            if (!order.ContainsKey(o[0]))
            {
                order[o[0]] = new List<int>();
            }
            order[o[0]].Add(o[1]);
        }

        var updates = rowData.Skip(rules.Count).Select(r => r.GetIntegers()).ToList();
        List<int[]> toFix = new List<int[]>();
        var sum = 0;
        foreach (var update in updates)
        {
            for (int i = 0; i < update.Length - 1; i++)
            {
                if (!update.Skip(i + 1).All(j => order.ContainsKey(update[i]) && order[update[i]].Contains(j)))
                {
                    toFix.Add(update);
                    break;
                }
            }
        }

        foreach (var fix in toFix)
        {
            for (int i = 0; i < fix.Length; i++)
            {
                var faulty = fix.Skip(i + 1).FirstOrDefault(j => !order.ContainsKey(fix[i]) || !order[fix[i]].Contains(j));
                if (faulty != default)
                {
                    var k = Enumerable.Range(i, fix.Length - i).First(j => fix[j] == faulty);
                    var temp = fix[k];
                    fix[k] = fix[i];
                    fix[i] = temp;
                    i = -1;
                }
            }

            sum += fix[fix.Length / 2];
        }

        return sum.ToString();
    }
}
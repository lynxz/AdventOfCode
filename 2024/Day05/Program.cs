using Tools;

var day = new Day05();
day.OutputFirstStar();

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
                for (int j = i + 1; j < update.Length; j++)
                {
                    if (!order[update[i]].Contains(update[j]))
                    {
                        correct = false;
                        break;
                    }
                }
                if (!correct)
                {
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
        List<int[]> toFix = new List<int[]>();
        var sum = 0;
        foreach (var update in updates)
        {
            var correct = true;
            for (int i = 0; i < update.Length - 1; i++)
            {
                for (int j = i + 1; j < update.Length; j++)
                {
                    if (!order[update[i]].Contains(update[j]))
                    {
                        toFix.Add(update);
                        correct = false;
                        break;
                    }
                }
                if (!correct)
                {
                    break;
                }
            }
        }

        return sum.ToString();
    }
}
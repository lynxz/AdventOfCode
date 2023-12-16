using Tools;

Day15 day = new();
day.OutputSecondStar();

public class Day15 : DayBase
{
    public Day15() : base("15")
    {

    }

    public override string FirstStar()
    {
        return GetRawData().Trim().Split(',', StringSplitOptions.RemoveEmptyEntries).Sum(s => s.Aggregate(0, (acc, v) => (v + acc) * 17 % 256)).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData().Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
        var boxes = Enumerable.Range(0, 256).Select(i => new List<(string label, int value)>()).ToArray();
        foreach (var s in data)
        {
            if (s.Contains('-'))
            {
                var label = s.Split('-')[0];
                var box = label.Aggregate(0, (acc, v) => (v + acc) * 17 % 256);
                var index = boxes[box].FindIndex(b => b.label == label);
                if (index != -1)
                {
                    boxes[box].RemoveAt(index);
                }
            }
            else
            {
                var p = s.Split('=');
                var label = p[0];
                var f = int.Parse(p[1]);
                var box = label.Aggregate(0, (acc, v) => (v + acc) * 17 % 256);
                var index = boxes[box].FindIndex(b => b.label == label);
                if (index != -1)
                {
                    boxes[box][index] = (label, f);
                }
                else
                {
                    boxes[box].Add((label, f));
                }
            }
        }

        return Enumerable.Range(0, 256).Select(i => Enumerable.Range(0, boxes[i].Count).Sum(j => (i + 1) * (j + 1) * boxes[i][j].value)).Sum().ToString();
    }
}
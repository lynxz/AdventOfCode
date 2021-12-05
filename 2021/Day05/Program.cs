// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day5("5");
day.OutputSecondStar();

public class Day5 : DayBase
{
    public Day5(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData().Select(r => r.GetIntegers()).ToList();
        var straightLines = input.Where(d => d[0] == d[2] || d[1] == d[3]).ToList();
        var points = new HashSet<string>();

        for (int i = 0; i < straightLines.Count - 1; i++)
        {
            var p1 = CalculatePoints(straightLines[i]);
            for (int j = i + 1; j < straightLines.Count; j++)
            {
                var p2 = CalculatePoints(straightLines[j]);
                p1.Where(p => p2.Any(ip => p[0] == ip[0] && p[1] == ip[1])).Select(p => $"({p[0]},{p[1]})").ToList().ForEach(p => points.Add(p));
            }
        }
        return points.Count.ToString();
    }

    public List<int[]> CalculatePoints(int[] line)
    {
        if (line[0] == line[2])
            return Enumerable.Range(0, Math.Abs(line[1] - line[3]) + 1).Select(i => new[] { line[0], Math.Min(line[1], line[3]) + i }).ToList();
        if (line[1] == line[3])
            return Enumerable.Range(0, Math.Abs(line[0] - line[2]) + 1).Select(i => new[] { Math.Min(line[0], line[2]) + i, line[1] }).ToList();

        return Enumerable.Range(0, Math.Abs(line[1] - line[3]) + 1).Select(i => line[0] < line[2] ?
          new[] { line[0] + i, line[1] < line[3] ? line[1] + i : line[1] - i } :
          new[] { line[2] + i, line[1] < line[3] ? line[3] - i : line[3] + i }).ToList();
    }

    public override string SecondStar()
    {
        var input = GetRowData().Select(r => r.GetIntegers()).ToList();
        var points = new HashSet<string>();
        var mem = new Dictionary<string, List<int[]>>();

        for (int i = 0; i < input.Count - 1; i++)
        {
            var lineId = string.Join(",", input[i]);
            var p1 = mem.ContainsKey(lineId) ? mem[lineId] : CalculatePoints(input[i]);

            for (int j = i + 1; j < input.Count; j++)
            {
                lineId = string.Join(",", input[j]);
                if (!mem.ContainsKey(lineId))
                    mem.Add(lineId, CalculatePoints(input[j]));
                var p2 = mem[lineId];
                p1.Where(p => p2.Any(ip => p[0] == ip[0] && p[1] == ip[1])).Select(p => $"({p[0]},{p[1]})").ToList().ForEach(p => points.Add(p));
            }
        }

        return points.Count.ToString();
    }
}
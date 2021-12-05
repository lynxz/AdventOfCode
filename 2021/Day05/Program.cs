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
        var input = GetRowData().Select(r => r.GetIntegers()).Where(d => d[0] == d[2] || d[1] == d[3]).ToList();
        return input.SelectMany(p => GetCoords(p)).GroupBy(c => c).Count(g => g.Count() > 1).ToString();
    }

    public override string SecondStar()
    {
        var input = GetRowData().Select(r => r.GetIntegers()).ToList();
        return input.SelectMany(p => GetCoords(p)).GroupBy(c => c).Count(g => g.Count() > 1).ToString();
    }

    public List<(int, int)> GetCoords(int[] line)
    {
        if (line[0] == line[2])
            return Enumerable.Range(0, Math.Abs(line[1] - line[3]) + 1).Select(i => (line[0], Math.Min(line[1], line[3]) + i)).ToList();
        if (line[1] == line[3])
            return Enumerable.Range(0, Math.Abs(line[0] - line[2]) + 1).Select(i => (Math.Min(line[0], line[2]) + i, line[1])).ToList();

        return Enumerable.Range(0, Math.Abs(line[1] - line[3]) + 1).Select(i => line[0] < line[2] ?
          (line[0] + i, line[1] < line[3] ? line[1] + i : line[1] - i) :
          (line[2] + i, line[1] < line[3] ? line[3] - i : line[3] + i)).ToList();
    }
}
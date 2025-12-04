using System.Runtime.InteropServices.Marshalling;
using Tools;

var day = new Day04();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day04 : DayBase
{
    public Day04() : base("4")
    {
    }

    public override string FirstStar()
    {
        var rows = GetRowData().Where(r => !string.IsNullOrWhiteSpace(r)).Select(s => s.Trim().ToCharArray());
        var xMax = rows.First().Length;
        var all = rows.SelectMany(r => r).ToArray();
        var t = all.Count();
        var yMax = t / xMax;
        (int x, int y)[] offsets = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];

        return Enumerable.Range(0, t).Where(i =>
        {
            var x = i % xMax;
            var y = i / xMax;
            if (all[i] != '@') return false;
            return offsets
                .Where(o => x + o.x < xMax && y + o.y < yMax && y + o.y >= 0 && x + o.x >= 0)
                .Count(o => all[(y + o.y) * xMax + x + o.x] == '@') < 4;
        }).Count()
        .ToString();
    }

    public override string SecondStar()
    {
        var rows = GetRowData().Where(r => !string.IsNullOrWhiteSpace(r)).Select(s => s.Trim().ToCharArray());
        var xMax = rows.First().Length;
        var all = rows.SelectMany(r => r).ToArray();
        var t = all.Length;
        var yMax = t / xMax;
        (int x, int y)[] offsets = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];

        var totalSum = 0;
        var sum = 0;
        do
        {
            var copy = new char[t];
            sum = Enumerable.Range(0, t).Where(i =>
            {
                copy[i] = all[i];
                var x = i % xMax;
                var y = i / xMax;
                if (all[i] != '@') return false;
                var rolls = offsets
                    .Where(o => x + o.x < xMax && y + o.y < yMax && y + o.y >= 0 && x + o.x >= 0)
                    .Count(o => all[(y + o.y) * xMax + x + o.x] == '@') < 4;
                if (rolls)
                    copy[i] = '.';

                return rolls;
            }).Count();
            all = copy;
            totalSum += sum;
        } while (sum > 0);

        return totalSum.ToString();
    }
}
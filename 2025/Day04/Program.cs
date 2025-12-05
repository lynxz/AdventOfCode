using Tools;

var day = new Day04();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day04 : DayBase
{
    (int x, int y)[] _offsets = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];

    public Day04() : base("4")
    {
    }

    public override string FirstStar()
    {
        var set = GetHashSet();
        return set
            .Count(c => _offsets.Count(o => set.Contains((c.x + o.x, c.y + o.y))) < 4)
            .ToString();
    }

    public override string SecondStar()
    {
        var set = GetHashSet();
        var totalSum = 0;
        var toRemove = Array.Empty<(int x, int y)>();
        do
        {
            toRemove = set
                .Where(c => _offsets.Count(o => set.Contains((c.x + o.x, c.y + o.y))) < 4)
                .ToArray();
            totalSum += toRemove.Length;
            set = set.Except(toRemove).ToHashSet();
        } while (toRemove.Length > 0);

        return totalSum.ToString();
    }

    private HashSet<(int x, int y)> GetHashSet()
    {
        var rows = GetRowData().Where(r => !string.IsNullOrWhiteSpace(r)).Select(s => s.Trim().ToCharArray()).ToList();
        var xMax = rows.First().Length;
        var yMax = rows.Count;

        HashSet<(int x, int y)> set = [.. Enumerable.Range(0, xMax).SelectMany(x => Enumerable.Range(0, yMax).Where(y => rows[y][x] == '@').Select(y => (x, y)))];
        return set;
    }
}
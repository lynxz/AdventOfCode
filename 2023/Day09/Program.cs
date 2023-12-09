using Tools;

Day09 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day09 : DayBase
{
    public Day09() : base("9")
    {
    }

    public override string FirstStar()
    {
        var rows = GetRowData().Select(r => r.GetIntegers()).ToList();
        return rows.Sum( r => FindNext(r, false)).ToString();
    }

    public override string SecondStar()
    {
        var rows = GetRowData().Select(r => r.GetIntegers()).ToList();
        return rows.Sum( r => FindNext(r, true)).ToString();
    }

    int FindNext(int[] row, bool first = false) {
        if (row.All(r => r == 0))
            return 0;

        var result = FindNext(row.Window(2).Select(p => p[1] - p[0]).ToArray(), first);
        return first ? row.First() - result : row.Last() + result;
    }

}
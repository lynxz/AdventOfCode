using Tools;

Day13 day = new();
day.OutputFirstStar();

public class Day13 : DayBase
{

    public Day13() : base("13")
    {
    }

    public override string FirstStar()
    {
        var areas = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        var rows = 0;
        var columns = 0;

        foreach (var area in areas)
        {
            rows += GetRows(area);
            columns += GetColumns(area);
        }


        return (rows * 100 + columns).ToString();
    }

    private static int GetColumns(string area, bool fixSumdge = false)
    {
        var lines = area.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var transposed = Enumerable.Range(0, lines[0].Length).Select(x => new string(Enumerable.Range(0, lines.Length).Select(y => lines[lines.Length - y - 1][x]).ToArray())).ToArray();

        return GetMirroredLines(transposed);
    }

    private static int GetRows(string area, bool fixSumdge = false)
    {
        var lines = area.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        return GetMirroredLines(lines);
    }

    private static int GetMirroredLines(string[] lines, bool fixSumdge = false)
    {
        for (int y = 0; y < lines.Length - 1; y++)
        {
            if (lines[y] == lines[y + 1])
            {

                var offset = 1;
                while (y + offset < lines.Length && (y - (offset - 1)) > -1 && lines[y - (offset - 1)] == lines[y + offset])
                {
                    offset++;
                }
                if (y + offset == lines.Length || (y - (offset - 1)) == -1)
                {
                    return y + 1;
                }
            }
        }

        return 0;
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
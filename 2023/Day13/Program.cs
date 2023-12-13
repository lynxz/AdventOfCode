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

    private static int GetColumns(string area)
    {
        var lines = area.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var transposed = Enumerable.Range(0, lines[0].Length).Select(x => new string(Enumerable.Range(0, lines.Length).Select(y => lines[lines.Length - y - 1][x]).ToArray())).ToArray();

        return GetMirroredLines(transposed);
    }

    private static int GetRows(string area)
    {
        var lines = area.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        return GetMirroredLines(lines);
    }

    private static int GetMirroredLines(string[] lines)
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
        var areas = GetRawData().Replace("\r", "").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        var rows = 0;
        var columns = 0;

        foreach (var area in areas)
        {
            (int r, (int, int) smudger) = GetRowsSmudge(area);
            (int c, (int, int) smudgec) = GetColumnsSmudge(area);
            if (smudger != (-1, -1)) {
                rows += r;
            } 
            else if (smudgec != (-1, -1)) {
                columns += c;
            }
        }

        return (rows * 100 + columns).ToString();
    }

    private static (int, (int, int)) GetColumnsSmudge(string area)
    {
        var lines = area.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var transposed = Enumerable.Range(0, lines[0].Length).Select(x => new string(Enumerable.Range(0, lines.Length).Select(y => lines[lines.Length - y - 1][x]).ToArray())).ToArray();

        return GetMirroredLinesSmudge(transposed);
    }

    private static (int, (int, int)) GetRowsSmudge(string area)
    {
        var lines = area.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        return GetMirroredLinesSmudge(lines);
    }

    private static (int, (int, int)) GetMirroredLinesSmudge(string[] lines)
    {
        for (int y = 0; y < lines.Length - 1; y++)
        {
            if (Enumerable.Range(0, lines.First().Length).Count(i => lines[y][i] != lines[y + 1][i]) < 2)
            {

                var offset = 1;
                var smudge = (-1, -1);
                while (y + offset < lines.Length && (y - (offset - 1)) > -1)
                {
                    if (lines[y - (offset - 1)] == lines[y + offset])
                    {
                        offset++;
                    }
                    else if (Enumerable.Range(0, lines.First().Length).Count(i => lines[y - (offset - 1)][i] != lines[y + offset][i]) == 1)
                    {
                        if (smudge != (-1, -1))
                        {
                            break;
                        }
                        var x = Enumerable.Range(0, lines.First().Length).First(i => lines[y - (offset - 1)][i] != lines[y + offset][i]);
                        smudge = (y - (offset - 1), x);
                        offset++;
                    }
                    else
                    {
                        break;
                    }

                }
                if (y + offset == lines.Length || (y - (offset - 1)) == -1 && smudge != (-1, -1))
                {
                    return (y + 1, smudge);
                }
            }
        }

        return (0, (-1, -1));
    }


}
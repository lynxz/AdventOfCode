using Tools;

Day07 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day07 : DayBase
{
    public Day07() : base("7")
    {
    }

    public override string FirstStar()
    {
        var lines = GetRowData();
        var array = Enumerable.Range(0, lines[0].Length).Select(x => Enumerable.Range(0, lines.Length).Select(y => lines[y][x])).ToMultidimensionalArray();
        HashSet<int> beams = [lines[0].IndexOf('S')];
        var splits = 0;
        for(int y = 1; y < array.GetLength(1); y++)
        {
            var splitters = Enumerable.Range(0, array.GetLength(0)).Where(x => array[x, y] == '^').ToList();
            if (splitters.Count == 0)
                continue;

            HashSet<int> newBeams = new();
            foreach(var beam in beams)
            {
                if (splitters.Contains(beam))
                {
                    if (beam - 1 >= 0)
                        newBeams.Add(beam - 1);
                    if (beam + 1 < array.GetLength(0))
                        newBeams.Add(beam + 1);

                    splits++;
                }
                else
                {
                    newBeams.Add(beam);
                }
            }
            beams = newBeams;
        }

        return splits.ToString();
    }

    public override string SecondStar()
    {
        var lines = GetRowData();
        var array = Enumerable.Range(0, lines[0].Length).Select(x => Enumerable.Range(0, lines.Length).Select(y => lines[y][x])).ToMultidimensionalArray();
        Dictionary<int, long> beams = new() { [lines[0].IndexOf('S')] = 1 };
        for(int y = 1; y < array.GetLength(1); y++)
        {
            var splitters = Enumerable.Range(0, array.GetLength(0)).Where(x => array[x, y] == '^').ToList();
            if (splitters.Count == 0)
                continue;

             Dictionary<int, long> newBeams = new();
            foreach(var beam in beams.Keys)
            {
                if (splitters.Contains(beam))
                {
                    if (beam - 1 >= 0)
                    {
                        if (!newBeams.ContainsKey(beam - 1))
                            newBeams[beam - 1] = 0;
                        newBeams[beam - 1] += beams[beam];
                    }
                    if (beam + 1 < array.GetLength(0))
                    {
                        if (!newBeams.ContainsKey(beam + 1))
                            newBeams[beam + 1] = 0;
                        newBeams[beam + 1] += beams[beam];
                    }
                }
                else
                {
                    if (!newBeams.ContainsKey(beam))
                        newBeams[beam] = 0;
                    newBeams[beam] += beams[beam];
                }
            }
            beams = newBeams;
        }

        return beams.Values.Sum().ToString();
    }
}
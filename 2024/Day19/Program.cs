using Tools;

var day = new Day19();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day19 : DayBase
{
    public Day19() : base("19")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData().Replace("\r", "").Split("\n\n");
        var towels = data[0].Split(",").Select(x => x.Trim()).ToArray();
        var designs = data[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        return designs
            .Count(design => CanDesignBeMade(towels, design, string.Empty, new HashSet<string>()))
            .ToString();
    }

    bool CanDesignBeMade(string[] towels, string design, string attempt, HashSet<string> notPossible)
    {
        if (attempt == design)
            return true;
        if (notPossible.Contains(design.Substring(attempt.Length)))
            return false;

        foreach (var towel in towels)
        {
            var newAttempt = attempt + towel;
            if (design.StartsWith(newAttempt))
            {
                if (CanDesignBeMade(towels, design, attempt + towel, notPossible))
                    return true;
            }
        }

        notPossible.Add(design.Substring(attempt.Length));
        return false;
    }

    public override string SecondStar()
    {
        var data = GetRawData().Replace("\r", "").Split("\n\n");
        var towels = data[0].Split(",").Select(x => x.Trim()).ToArray();
        var designs = data[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        return designs
            .Sum(d => CountDesigns(towels, d, string.Empty, new Dictionary<string, long>()))
            .ToString();
    }

    long CountDesigns(string[] towels, string design, string attempt, Dictionary<string, long> combos)
    {
        if (attempt == design)
            return 1L;
        if (combos.ContainsKey(design.Substring(attempt.Length)))
            return combos[design.Substring(attempt.Length)];

        var total = 0L;
        foreach (var towel in towels)
        {
            var newAttempt = attempt + towel;
            if (design.StartsWith(newAttempt))
            {
                total += CountDesigns(towels, design, attempt + towel, combos);
            }
        }

        combos.Add(design.Substring(attempt.Length), total);
        return total;
    }
}
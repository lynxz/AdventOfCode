using Tools;

var day = new Day25();
day.OutputFirstStar();

public class Day25 : DayBase
{
    public Day25() : base("25")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData().Replace("\r", "").Split("\n\n");

        var keys = data
            .Where(d => d.First() == '.')
            .Select(d => d.Replace("\n", ""))
            .Select(d => 
                Enumerable.Range(0,5).Select(x => 
                    Enumerable.Range(0,7).Count(y => d[x+y*5] == '#') - 1
                ).ToArray())
            .ToList();

        var locks = data
            .Where(d => d.First() == '#')
            .Select(d => d.Replace("\n", ""))
            .Select(d => 
                Enumerable.Range(0,5).Select(x => 
                    Enumerable.Range(0,7).Count(y => d[x+y*5] == '#') - 1
                ).ToArray())
            .ToList();

        System.Console.WriteLine("Keys: " + keys.Count);
        System.Console.WriteLine();
        System.Console.WriteLine("Locks: " + locks.Count);

        var sum = 0;
        foreach (var k in keys)
        {
            foreach (var l in locks)
            {
                if (Enumerable.Range(0, 5).All(i => k[i]+l[i] <= 5))
                {
                    sum++;
                }
            }
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
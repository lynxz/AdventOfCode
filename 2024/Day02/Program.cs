using Tools;

var day = new Day02();
day.OutputSecondStar();

public class Day02 : DayBase
{
    public Day02() : base("2")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();

        var safe = 0;
        foreach (var row in data)
        {
            var ints = row.GetIntegers();
            var diffs = ints.Window(2).Select(w => w[1] - w[0]);

            if (diffs.All(d => d < 0) || diffs.All(d => d > 0))
            {
                if (diffs.All(d => Math.Abs(d) < 4))
                {
                    safe++;
                }
            }
        }


        return safe.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();

        var safe = 0;
        foreach (var row in data)
        {
            var ints = row.GetIntegers().ToList();
            var diffs = ints.Window(2).Select(w => w[1] - w[0]);
            var success = false;

            if (diffs.All(d => d < 0) || diffs.All(d => d > 0))
            {
                if (diffs.All(d => Math.Abs(d) < 4))
                {
                   success = true;
                }
            }

            if (!success) {
                var errorLine = FirstError(ints);
                var newInts = ints.ToList();
                newInts.RemoveAt(errorLine);
                diffs = newInts.Window(2).Select(w => w[1] - w[0]);
                if (diffs.All(d => d < 0) || diffs.All(d => d > 0))
                {
                    if (diffs.All(d => Math.Abs(d) < 4))
                    {
                        success = true;
                    }
                }
                if (!success) {
                    newInts = ints.ToList();
                    newInts.RemoveAt(errorLine+1);
                    diffs = newInts.Window(2).Select(w => w[1] - w[0]);
                    if (diffs.All(d => d < 0) || diffs.All(d => d > 0))
                    {
                        if (diffs.All(d => Math.Abs(d) < 4))
                        {
                            success = true;
                        } 
                    }
                }
            }
           
            if (success) {
                safe++;
            } else {
                Console.WriteLine(string.Join(" ", ints));
            }
        }


        return safe.ToString();
    }

    public int FirstError(List<int> ints)
    {
        var diffs = ints.Window(2).Select(w => w[1] - w[0]).ToList();

        if (!(diffs.All(d => d < 0) || diffs.All(d => d > 0)))
        {
            var negative = diffs.Count(d => d < 0);
            var positive = diffs.Count(d => d > 0);
            if (negative > positive)
            {
                return Enumerable.Range(0, diffs.Count).First(i => diffs[i] >= 0);
            }
            else
            {
                return Enumerable.Range(0, diffs.Count).First(i => diffs[i] <= 0);
            }
        }

        if (!diffs.All(d => Math.Abs(d) < 4))
        {
            return Enumerable.Range(0, diffs.Count).First(i => Math.Abs(diffs[i]) > 3);
        }

        return -1;
    }
}
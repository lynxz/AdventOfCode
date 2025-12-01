using Tools;

var day = new Day01();
day.OutputSecondStar();

public class Day01 : DayBase
{
    public Day01() : base("1") { }

    public override string FirstStar()
    {
        var data = GetRowData().Select(x => x[0] == 'L' ? -1 * (int.Parse(x.Substring(1).Trim()) % 100) : int.Parse(x.Substring(1).Trim()) % 100).ToArray();
        var vals = data.Aggregate(new List<int> { 50 }, (acc, x) =>
        {
            var prev = acc.Last();

            var next = prev + x < 0 ? 99 + prev + x + 1 : (prev + x) % 100;
            System.Console.WriteLine(next);
            acc.Add(next);
            return acc;
        });


        return vals.Count(x => x == 0).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(x => x[0] == 'L' ? -1 * int.Parse(x.Substring(1).Trim()) : int.Parse(x.Substring(1).Trim())).ToArray();
        var sum = 0;
        var vals2 = data.Aggregate(new List<int> { 50 }, (acc, x) =>
        {
            var prev = acc.Last();
            x = x < 0 ? -1 * (Math.Abs(x) % 100) : x % 100;
            var next = prev + x < 0 ? 99 + prev + x + 1 : (prev + x) % 100;
            acc.Add(next);
            return acc;
        });
        var i = 1;

        var vals = data.Aggregate(50, (prev, x) =>
        {

            System.Console.WriteLine(x);
            var val = prev + x;
            if (val > 99)
            {
                sum += val / 100;
                val = val % 100;
            }
            else if (val < 0)
            {
                sum += Math.Abs(val / 99) + (prev == 0 ? 0 : 1);
                val = 100 + prev - (Math.Abs(x) % 100);
                if (val > 99) val = val % 100;
            }
            else if (val == 0)
            {
                sum += 1;
            }
            System.Console.WriteLine(val + " " + sum);

            if (val != vals2[i])
            {
                System.Console.WriteLine($"Mismatch at step {i}: {val} != {vals2[i]}");
                throw new Exception("Mismatch!");
            }
            i++;
            return val;
        });

        return sum.ToString();
    }
}
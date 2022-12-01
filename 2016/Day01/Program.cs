
using Tools;

Day01 day = new();
day.PostSecondStar();

public class Day01 : DayBase
{

    public Day01() : base("1")
    {
    }

    public override string FirstStar()
    {
        var multiplier = new[] { 0, 1, 0, -1 };
        var dir = new int[2] { 0, 1 };
        var data = GetRawData().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(d => d.Trim()).ToArray();
        var blocks = data.Aggregate(new[] { 0, 0 }, (d, v) =>
        {
            var mult = int.Parse(new string(v.Skip(1).ToArray()));
            var add = v.StartsWith("R") ? 1 : -1;

            dir[0] = (dir[0] + add) % 4;
            dir[0] = dir[0] < 0 ? 3 : dir[0];
            dir[1] = (dir[1] + add) % 4;
            dir[1] = dir[1] < 0 ? 3 : dir[1];

            d[0] += mult * multiplier[dir[0]];
            d[1] += mult * multiplier[dir[1]];

            return d;
        });

        return (Math.Abs(blocks[0]) + Math.Abs(blocks[1])).ToString();
    }

    public override string SecondStar()
    {
        var multiplier = new[] { 0, 1, 0, -1 };
        var dir = new int[2] { 0, 1 };
        var data = GetRawData().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(d => d.Trim()).ToArray();
        var d = new[] { 0, 0 };
        List<int[]> mem = new();

        foreach (var v in data)
        {
            var mult = int.Parse(new string(v.Skip(1).ToArray()));
            var add = v.StartsWith("R") ? 1 : -1;

            dir[0] = (dir[0] + add) % 4;
            dir[0] = dir[0] < 0 ? 3 : dir[0];
            dir[1] = (dir[1] + add) % 4;
            dir[1] = dir[1] < 0 ? 3 : dir[1];

            var done = false;
            for (int i = 0; i < mult; i++)
            {
                d[0] += multiplier[dir[0]];
                d[1] += multiplier[dir[1]];
                if (mem.Any(b => b[0] == d[0] && b[1] == d[1]))
                {
                    done = true;
                    break;
                }

                mem.Add(new int[] { d[0], d[1] });
            }
            if (done)
                break;
        }

        return (Math.Abs(d[0]) + Math.Abs(d[1])).ToString();
    }
}
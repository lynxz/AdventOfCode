using Tools;

var day = new Day11();
day.OutputSecondStar();

public class Day11 : DayBase
{
    public Day11() : base("11")
    {
    }

    public override string FirstStar()
    {
        var stones = GetRawData().GetIntegers().Select(i => Convert.ToUInt64(i)).ToList();

        for (int i = 0; i < 25; i++)
        {
            var newStones = new List<ulong>();
            foreach (var stone in stones)
            {
                if (stone == 0)
                {
                    newStones.Add(1);
                    continue;
                }
                var digits = CountDigits(stone);
                if (digits % 2 == 0)
                {
                    var n = Enumerable.Repeat((ulong)10, digits / 2).Aggregate((a, b) => a * b);
                    newStones.Add(stone / n);
                    newStones.Add(stone % n);
                }
                else
                {
                    newStones.Add(stone * 2024);
                }
            }
            stones = newStones;

        }


        return stones.Count.ToString();
    }

    static int CountDigits(ulong n)
    {
        if (n == 0)
        {
            return 1;
        }

        var count = 0;
        while (n != 0)
        {
            n /= 10;
            count++;
        }
        return count;
    }

    public override string SecondStar()
    {
        var stones = GetRawData().GetIntegers().Select(i => Convert.ToUInt64(i)).ToList();
        var memo = new Dictionary<ulong, List<ulong>>();
        long sum = 0;
        foreach (var stone in stones) {
            var v = GetNumberOfStones(stone, 0, memo);
            System.Console.WriteLine(stone + " " + v);
            sum += v;
        }

        return sum.ToString();
    }

    static long GetNumberOfStones(ulong stone, int level, Dictionary<ulong, List<ulong>> memo)
    {
        if (!memo.ContainsKey(stone))
        {
            memo.Add(stone, GetStones(stone));
        }
        if (level == 2) {
            return memo[stone].Count;
        }

        return memo[stone].Sum(s => GetNumberOfStones(s, level+1, memo));
    }

    static List<ulong> GetStones(ulong seed)
    {
        var stones = new List<ulong> { seed };
        for (int i = 0; i < 25; i++)
        {
            var newStones = new List<ulong>();
            foreach (var stone in stones)
            {
                if (stone == 0)
                {
                    newStones.Add(1);
                    continue;
                }
                var digits = CountDigits(stone);
                if (digits % 2 == 0)
                {
                    var n = Enumerable.Repeat((ulong)10, digits / 2).Aggregate((a, b) => a * b);
                    newStones.Add(stone / n);
                    newStones.Add(stone % n);
                }
                else
                {
                    newStones.Add(stone * 2024);
                }
            }
            stones = newStones;
        }
        return stones;
    }
}
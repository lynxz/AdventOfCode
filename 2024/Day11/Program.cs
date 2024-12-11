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
                    newStones.Add(stone / n );
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

    int CountDigits(ulong n)
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
        var stones = GetRawData().GetIntegers().Select(i => Convert.ToUInt64(i)).Take(1).ToList();

        for (int i = 0; i < 75; i++)
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
                    newStones.Add(stone / n );
                    newStones.Add(stone % n);
                }
                else
                {
                    newStones.Add(stone * 2024);
                }
            }
            stones = newStones;
            System.Console.WriteLine($"Iteration {i + 1}: {stones.Count}");
        }


        return stones.Count.ToString();
    }
}
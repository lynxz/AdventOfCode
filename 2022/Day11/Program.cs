using System.Numerics;
using Tools;

Day11 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day11 : DayBase
{
    public Day11() : base("11")
    {
    }

    public override string FirstStar()
    {
        var blocks = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var monkeys = new List<Monkey>();
        blocks.ToList().ForEach(b => Monkey.ParseMonkey(b, monkeys));
        for (int i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
                monkey.Turn();
        }

        return monkeys.OrderByDescending(x => x.Inspections).Take(2).Aggregate(1L, (v, m) => v * m.Inspections).ToString();
    }

    public override string SecondStar()
    {
        var blocks = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var monkeys = new List<Monkey>();
        blocks.ToList().ForEach(b => Monkey.ParseMonkey(b, monkeys, false));
        for (int i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
                monkey.Turn();
        }

        return monkeys.OrderByDescending(x => x.Inspections).Take(2).Aggregate(1L, (v, m) => v * m.Inspections).ToString();
    }
}

public class Monkey
{
    private readonly Queue<long> items;
    private readonly List<Monkey> monkeys;
    private readonly Func<long, long> op;
    private readonly int[] receivers;
    private readonly Lazy<long> divisorLimit;
    private readonly bool divideByThree;

    private Monkey(int num, List<Monkey> monkeys, IEnumerable<long> startItems, Func<long, long> op, long divisor, int[] receivers, bool divideByThree)
    {
        Num = num;
        this.monkeys = monkeys;
        items = new Queue<long>(startItems);
        this.op = op;
        this.receivers = receivers;
        Divisor = divisor;
        this.divideByThree = divideByThree;
        divisorLimit = new Lazy<long>(() => monkeys.Aggregate(1L, (v, m) => m.Divisor * v));
    }

    public static Monkey ParseMonkey(string data, List<Monkey> monkeys, bool divideByThree = true)
    {
        var lines = data.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var num = lines[0].GetIntegers()[0];
        var items = lines[1].GetIntegers().Select(i => Convert.ToInt64(i));
        var opValue = lines[2].GetIntegers().Select(i => Convert.ToInt64(i)).ToArray();
        Func<long, long> op = lines[2] switch
        {
            var str when lines[2].Contains('*') && opValue.Length > 0 => x => x * opValue[0],
            var str when lines[2].Contains('+') && opValue.Length > 0 => x => x + opValue[0],
            var str when lines[2].Contains('*') => x => x * x,
            var str when lines[2].Contains('+') => x => x + x,
            _ => throw new InvalidOperationException(data)
        };
        var div = lines[3].GetIntegers()[0];
        var receivers = new int[] { lines[4].GetIntegers()[0], lines[5].GetIntegers()[0] };
        var monkey = new Monkey(num, monkeys, items, op, div, receivers, divideByThree);
        monkeys.Add(monkey);
        return monkey;
    }

    public int Num { get; }

    public long Inspections { get; private set; }

    public long Divisor { get; }

    public void Turn()
    {
        while (items.Count > 0)
        {
            var oldValue = items.Dequeue();
            var newValue = op(oldValue);
            if (divideByThree)
                newValue /= 3;

            newValue %= divisorLimit.Value;
            Throw(receivers[Test(newValue) ? 0 : 1], newValue);
            Inspections++;
        }
    }

    public void Catch(long item) => items.Enqueue(item);

    private void Throw(int monkey, long item)
    {
        var receiver = monkeys.First(m => m.Num == monkey);
        receiver.Catch(item);
    }

    private bool Test(long item) => item % Divisor == 0;
}
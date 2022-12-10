using Tools;

Day10 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day10 : DayBase
{
    public Day10() : base("10")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var computer = new Computer(data);
        var cycles = Enumerable.Range(0, 220).Select(_ => computer.Tick()).ToList();
        return (new [] {20, 60, 100, 140, 180, 220}).Sum(i => i*cycles[i - 1]).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var computer = new Computer(data);
        var cycles = Enumerable.Range(0, 240).Select(_ => computer.Tick()).ToList();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                Console.Write(cycles[j + (i * 40)] + 1 >= j && cycles[j + (i * 40)] - 1 <= j ? "#" : ".");
            }
            Console.WriteLine();
        }
        return string.Empty;
    }
}

public class Computer
{
    private readonly IList<string> _data;
    private readonly Queue<Action> _ops;

    public Computer(IList<string> data)
    {
        _ops = new Queue<Action>();
        Cycle = 1;
        _data = data;
        X = 1;
    }

    public int X { get; private set; }

    public int Cycle { get; private set; }

    public int Tick()
    {
        var temp = X;
        if (_data.Count > Cycle - 1)
        {
            _ops.Enqueue(() => { });
            if (_data[Cycle - 1].StartsWith("addx"))
            {
                var addValue = _data[Cycle - 1].GetIntegers().Single();
                _ops.Enqueue(() =>X += addValue);
            }
        }
        _ops.Dequeue()();
        Cycle++;
        return temp;
    }
}
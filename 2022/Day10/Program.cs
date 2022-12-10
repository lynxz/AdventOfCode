using Tools;

Day10 day = new();
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
        var sum = 0;
        while (computer.Cycle <= 220)
        {
            if (computer.Cycle == 20 || (computer.Cycle - 20) % 40 == 0)
                sum += computer.Cycle * computer.X;

            computer.Tick();
        }
        return sum.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var computer = new Computer(data);
        var screen = Enumerable.Repeat(".", 240).ToArray();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                if (computer.X + 1 >= j && computer.X - 1 <= j)
                    screen[j + (i * 40)] = "#";

                computer.Tick();
            }
        }
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                System.Console.Write(screen[j + (i * 40)]);
            }
            System.Console.WriteLine();
        }
        return string.Empty;
    }
}

public class Computer
{

    private readonly IList<string> _data;
    private Queue<Action> _ops;

    public Computer(IList<string> data)
    {
        _ops = new Queue<Action>();
        Cycle = 1;
        _data = data;
        X = 1;
    }

    public int X { get; private set; }

    public int Cycle { get; private set; }

    public void Tick()
    {
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
    }
}
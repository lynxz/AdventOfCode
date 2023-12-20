using System.Net.Security;
using Tools;

Day20 day = new();
day.OutputFirstStar();

public class Day20 : DayBase
{
    public Day20() : base("20")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var moduels = new Dictionary<string, Module>();

        foreach (var row in data)
        {
            var parts = row.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var module = parts[0];
            var connectedModules = parts[1].Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

            if (module[0] == '%')
                moduels.Add(module[1..], new FLipFlop(connectedModules));
            else if (module[0] == '&')
                moduels.Add(module[1..], new ConjunctionModule(connectedModules));
            else
                moduels.Add(module, new BroadcastModule(connectedModules));
        }

        var conjunctions = moduels.Where(x => x.Value is ConjunctionModule).Select(x => new KeyValuePair<string, ConjunctionModule>(x.Key, x.Value as ConjunctionModule)).ToList();

        foreach (var conjunction in conjunctions)
        {
            moduels.Where(m => m.Value.ConnectedModules.Contains(conjunction.Key)).ToList().ForEach(x => conjunction.Value.AddInput(x.Key));
        }

        var queue = new Queue<(string Module, string Input, int Signal)>();
        var lowCount = 0;
        var highCount = 0;
        for (int i = 0; i < 1000; i++)
        {
            //System.Console.WriteLine("button -low-> broadcaster");
            queue.Enqueue(("broadcaster", "button", 0));
            while (queue.Count > 0)
            {
                var (module, input, signal) = queue.Dequeue();

                if (signal == 0)
                    lowCount++;
                else
                    highCount++;

                if (!moduels.TryGetValue(module, out var m))
                    continue;

                var nextSignal = m.SendSignal(input, signal);
                if (nextSignal != -1)
                {
                    foreach (var connectedModule in moduels[module].ConnectedModules)
                    {
                        // var sig = nextSignal == 0 ? "low" : "high";
                        // System.Console.WriteLine($"{module} -{sig}-> {connectedModule}");
                        queue.Enqueue((connectedModule, module, nextSignal));
                    }


                }
            }
        }


        return (lowCount * highCount).ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}

public abstract class Module
{

    private List<string> _connectedModules;

    public Module(List<string> connectedModules)
    {
        _connectedModules = connectedModules;
    }

    public IEnumerable<string> ConnectedModules { get => _connectedModules; }

    public abstract int SendSignal(string input, int signal);
}

public class FLipFlop : Module
{

    bool _on = false;
    public FLipFlop(List<string> connectedModules) : base(connectedModules)
    {
    }

    public override int SendSignal(string input, int signal)
    {
        if (signal == 0)
        {
            var response = _on ? 0 : 1;
            _on = !_on;
            return response;
        }

        return -1;
    }
}

public class ConjunctionModule : Module
{

    readonly Dictionary<string, int> _inputSignals = new();

    public ConjunctionModule(List<string> connectedModules) : base(connectedModules)
    {
    }

    public void AddInput(string input)
    {
        _inputSignals.Add(input, 0);
    }

    public override int SendSignal(string input, int signal)
    {
        _inputSignals[input] = signal;
        return _inputSignals.Values.All(x => x == 1) ? 0 : 1;
    }
}

public class BroadcastModule : Module
{

    public BroadcastModule(List<string> connectedModules) : base(connectedModules)
    {
    }

    public override int SendSignal(string input, int signal) => signal;
}
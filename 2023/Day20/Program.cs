using Tools;

Day20 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

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
                        queue.Enqueue((connectedModule, module, nextSignal));
                    }
                }
            }
        }


        return (lowCount * highCount).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var mods = new Dictionary<string, List<string>>();
        var flips = new Dictionary<string, bool>();
        var conj = new Dictionary<string, Dictionary<string, int>>();

        foreach (var row in data)
        {
            var parts = row.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var name = parts[0];
            var con = parts[1].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            if (name[0] == '%')
            {
                name = name[1..];
                flips.Add(name, false);
            }
            else if (name[0] == '&')
            {
                name = name[1..];
                conj.Add(name, new Dictionary<string, int>());
            }
            mods.Add(name, con);
        }

        foreach (var c in conj)
        {
            mods.Where(m => m.Value.Contains(c.Key)).ToList().ForEach(m => c.Value.Add(m.Key, 0));
        }

        var queue = new Queue<(string Source, string Reciever, int Signal)>();
        var history = new List<(int Step, string Sender, int Signal)>();

        for (int i = 0; i < 15000; i++)
        {
            queue.Enqueue(("button", "broadcaster", 0));
            while (queue.Count > 0)
            {
                var (source, reciever, signal) = queue.Dequeue();

                if (reciever == "kj")
                    history.Add((i, source, signal));

                var output = -1;
                if (flips.ContainsKey(reciever))
                {
                    if (signal == 0)
                    {
                        flips[reciever] = !flips[reciever];
                        output = flips[reciever] ? 1 : 0;
                    }
                }
                else if (conj.TryGetValue(reciever, out var dict))
                {
                    dict[source] = signal;
                    output = dict.Values.All(i => i == 1) ? 0 : 1;
                }
                else
                {
                    output = signal;
                }

                if (output != -1)
                {
                    if (mods.TryGetValue(reciever, out var cons))
                    {
                        cons.ForEach(c => queue.Enqueue((reciever, c, output)));
                    }
                }
            }
        }
        var groups = history.Where(h => h.Signal != 0).GroupBy(h => h.Sender);
        var cycles = groups.Select(g => g.Skip(1).Take(2).Select(p => p.Step).ToArray()).Select(v => v[1] - v[0]).ToArray();

        return Algo.LCM(cycles.Select(c => (long)c).ToArray()).ToString();
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
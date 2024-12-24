using System.Net.NetworkInformation;
using Tools;

var day = new Day24();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day24 : DayBase
{
    public Day24() : base("24")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData().Replace("\r", "").Split("\n\n");
        var wires = data[0]
            .Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(r => (new string(r.Take(3).ToArray()), r.Last() - '0'))
            .ToDictionary();

        var gates = data[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(r =>
        {
            var parts = r.Split(" -> ");
            var inputs = parts[0].Split(" ");
            return new Gate(inputs[1], inputs[0], inputs[2], parts[1]);
        }).ToList();
        ulong result = CalculateGates(wires, gates);

        return result.ToString();
    }

    private static ulong CalculateGates(Dictionary<string, int> wires, List<Gate> gates)
    {
        while (gates.Count > 0)
        {
            var removeList = new List<Gate>();
            foreach (var gate in gates)
            {
                if (wires.ContainsKey(gate.Input1) && wires.ContainsKey(gate.Input2))
                {
                    var value1 = wires[gate.Input1];
                    var value2 = wires[gate.Input2];
                    switch (gate.Operation)
                    {
                        case "AND":
                            wires[gate.Output] = value1 & value2;
                            break;
                        case "OR":
                            wires[gate.Output] = value1 | value2;
                            break;
                        case "XOR":
                            wires[gate.Output] = value1 ^ value2;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    removeList.Add(gate);
                }
            }
            removeList.ForEach(r => gates.Remove(r));
        }

        var bits = wires.Where(kvp => kvp.Key.StartsWith("z")).OrderByDescending(kvp => kvp.Key).Select(kvp => Convert.ToUInt64(kvp.Value));
        ulong result = 0;
        foreach (var bit in bits)
        {
            result <<= 1;
            result |= bit;
        }

        return result;
    }

    public override string SecondStar()
    {
        var data = GetRawData().Replace("\r", "").Split("\n\n");
        var gates = data[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(r =>
        {
            var parts = r.Split(" -> ");
            var inputs = parts[0].Split(" ");
            return new Gate(inputs[1], inputs[0], inputs[2], parts[1]);
        }).ToList();

        var result = Fix(gates);
        return string.Join(",", result.OrderBy(r => r));
    }

    IEnumerable<string?> Fix(List<Gate> gates)
    {
        var cin = Output(gates, "x00", "AND", "y00");
        for (var i = 1; i < 45; i++)
        {
            var x = $"x{i:D2}";
            var y = $"y{i:D2}";
            var z = $"z{i:D2}";

            var xor1 = Output(gates, x, "XOR", y);
            var and1 = Output(gates, x, "AND", y);
            var xor2 = Output(gates, cin, "XOR", xor1);
            var and2 = Output(gates, cin, "AND", xor1);

            if (xor2 == null && and2 == null)
            {
                return SwapAndFix(gates, xor1, and1);
            }

            var carry = Output(gates, and1, "OR", and2);
            if (xor2 != z)
            {
                return SwapAndFix(gates, z, xor2);
            }
            else
            {
                cin = carry;
            }
        }
        return [];
    }

    IEnumerable<string?> SwapAndFix(List<Gate> gates, string? out1, string? out2)
    {
        var swap1 = gates.Single(g => g.Output == out1);
        var swap2 = gates.Single(g => g.Output == out2);
        var swapGate = new List<Gate>(gates);
        swapGate.Remove(swap1);
        swapGate.Remove(swap2);
        swapGate.Add(new Gate(swap1.Operation, swap1.Input1, swap1.Input2, swap2.Output));
        swapGate.Add(new Gate(swap2.Operation, swap2.Input1, swap2.Input2, swap1.Output));
        return Fix(swapGate).Concat([out1, out2]);
    }

    string? Output(List<Gate> circuit, string? x, string operation, string? y) =>
        circuit.SingleOrDefault(pair =>
            (pair.Input1 == x && pair.Operation == operation && pair.Input2 == y) ||
            (pair.Input1 == y && pair.Operation == operation && pair.Input2 == x)
        )?.Output;


    private static HashSet<Gate> GetDependent(List<Gate> gates, long err)
    {
        var zList = Enumerable.Range(0, 45).Where(i => (err & (1 << i)) > 0).Select(i => "z" + i.ToString("D2")).ToList();
        var stack = new Stack<string>();
        zList.ForEach(z => stack.Push(z));
        var dependent = new HashSet<Gate>();

        while (stack.Count > 0)
        {
            var z = stack.Pop();
            var gate = gates.FirstOrDefault(g => g.Output == z);
            if (gate == null)
            {
                continue;
            }
            stack.Push(gate.Input1);
            stack.Push(gate.Input2);
            dependent.Add(gate);
        }

        return dependent;
    }
}

public record Gate(string Operation, string Input1, string Input2, string Output);
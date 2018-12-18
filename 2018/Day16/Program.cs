using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        public void FirstStar()
        {
            var data = GetMonitorData();
            var computer = new Computer();
            var result = new List<int>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].StartsWith("Before"))
                {
                    var reg = data[i].Substring(9, 10).Split(',').Select(n => int.Parse(n)).ToArray();
                    var input = data[++i].Split(' ').Select(n => int.Parse(n)).ToArray();
                    var post = data[++i].Substring(9, 10).Split(',').Select(n => int.Parse(n)).ToArray();
                    var counter = 0;
                    for (int j = 0; j < computer.Ops.Count; j++)
                    {
                        computer.SetRegister(reg);
                        computer.Ops[j](input[1], input[2], input[3]);
                        if (computer.GetRegister().Zip(post, (n1, n2) => n1 == n2).All(r => r))
                        {
                            counter++;
                        }
                    }
                    result.Add(counter);
                }
            }
            System.Console.WriteLine(result.Where(d => d > 2).Count());
        }

        public void SecondStar()
        {
            var monitor = GetMonitorData();
            var computer = new Computer();
            var result = new List<(int[] Reg, int[] Input, int[] Post)>();

            for (int i = 0; i < monitor.Length; i++)
            {
                if (monitor[i].StartsWith("Before"))
                {
                    var reg = monitor[i].Substring(9, 10).Split(',').Select(n => int.Parse(n)).ToArray();
                    var input = monitor[++i].Split(' ').Select(n => int.Parse(n)).ToArray();
                    var post = monitor[++i].Substring(9, 10).Split(',').Select(n => int.Parse(n)).ToArray();
                    result.Add((reg, input, post));
                }
            }

            var opMap = new Dictionary<int, Action<int, int, int>>();
            var opNameMap = new Dictionary<int, string>();
            var groups = result.GroupBy(d => d.Input[0]);
            var unresolved = new Dictionary<int, List<Action<int, int, int>>>();
            foreach (var group in groups)
            {
                var matchOp = computer.Ops.Where(op => group.All(g =>
                {
                    computer.SetRegister(g.Reg);
                    op(g.Input[1], g.Input[2], g.Input[3]);
                    return computer.GetRegister().Zip(g.Post, (d1, d2) => d1 == d2).All(d => d);
                })).ToList();

                if (matchOp.Count == 1)
                {
                    opMap.Add(group.Key, matchOp.Single());
                    var name = computer.Names[computer.Ops.IndexOf(matchOp.Single())];
                    opNameMap.Add(group.Key, name);
                }
                if (matchOp.Count > 1)
                {
                    if (!unresolved.ContainsKey(group.Key))
                    {
                        unresolved.Add(group.Key, new List<Action<int, int, int>>());
                    }
                    unresolved[group.Key].AddRange(matchOp);
                }
            }

            while (unresolved.Count > 0)
            {
                foreach (var kvp in unresolved.OrderBy(p => p.Value.Count).ToList())
                {
                    var ops = kvp.Value.Where(op => !opMap.Values.Contains(op));
                    if (ops.Count() == 1)
                    {
                        var uniqueOp = ops.First();
                        opMap.Add(kvp.Key, uniqueOp);
                        var name = computer.Names[computer.Ops.IndexOf(uniqueOp)];
                        opNameMap.Add(kvp.Key, name);
                        unresolved.Remove(kvp.Key);
                    }
                }
            }
            
            computer.SetRegister(0, 0, 0, 0);
            System.Console.WriteLine("[0, 0, 0, 0]");
            foreach (var input in GetInputData())
            {
                var r = input.Split(' ').Select(i => int.Parse(i)).ToArray();
                opMap[r[0]](r[1], r[2], r[3]);
                System.Console.WriteLine($"{opNameMap[r[0]]}: {string.Join(" ", r)} ");
                System.Console.WriteLine($"[{string.Join(", ", computer.GetRegister())}]");
            }
            System.Console.WriteLine(computer.GetRegister().First());
        }

        string[] GetMonitorData() => File.ReadAllLines("monitorInput");

        string[] GetInputData() => File.ReadAllLines("dataInput");

    }



    public class Computer
    {

        readonly int[] _register;
        readonly List<Action<int, int, int>> _op;
        readonly List<string> _opName;
        public Computer()
        {
            _register = new int[4];
            _op = new List<Action<int, int, int>> {
                (a, b, c) => _register[c] = _register[a] + _register[b],
                (a, b, c) => _register[c] = _register[a] + b,
                (a, b, c) => _register[c] = _register[a] * _register[b],
                (a, b, c) => _register[c] = _register[a] * b,
                (a, b, c) => _register[c] = _register[a] & _register[b],
                (a, b, c) => _register[c] = _register[a] & b,
                (a, b, c) => _register[c] = _register[a] | _register[b],
                (a, b, c) => _register[c] = _register[a] | b,
                (a, b, c) => _register[c] = _register[a],
                (a, b, c) => _register[c] = a,
                (a, b, c) => _register[c] = a > _register[b] ? 1 : 0,
                (a, b, c) => _register[c] = _register[a] > b ? 1 : 0,
                (a, b, c) => _register[c] = _register[a] > _register[b] ? 1 : 0,
                (a, b, c) => _register[c] = a == _register[b] ? 1 : 0,
                (a, b, c) => _register[c] = _register[a] == b ? 1 : 0,
                (a, b, c) => _register[c] = _register[a] == _register[b] ? 1 : 0
            };
            _opName = new List<string> {
                "addr",
                "addi",
                "mulr",
                "muli",
                "banr",
                "bani",
                "borr",
                "bori",
                "setr",
                "seti",
                "gtir",
                "gtri",
                "gtrr",
                "eqir",
                "eqri",
                "eqrr"
            };
        }

        public void SetRegister(int r1, int r2, int r3, int r4)
        {
            _register[0] = r1;
            _register[1] = r2;
            _register[2] = r3;
            _register[3] = r4;
        }

        public void SetRegister(int[] reg)
        {
            for (int i = 0; i < _register.Length; i++)
            {
                _register[i] = reg[i];
            }
        }

        public IEnumerable<int> GetRegister() => _register.Select(i => i);

        public List<Action<int, int, int>> Ops => _op;

        public List<string> Names => _opName;

    }

}

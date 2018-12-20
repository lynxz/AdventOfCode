using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day19
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
            var data = GetData();
            var ipReg = int.Parse(data.First().Substring(3));
            var instr = data.Skip(1).Select(d => (d.Substring(0, 4), d.Substring(5).Split(' ').Select(i => int.Parse(i)).ToArray())).ToList();
            var computer = new Computer();
            var result = RunInstructions(ipReg, computer, instr);
            System.Console.WriteLine(result);
        }

        public void SecondStar()
        {
            // var data = GetData();
            // var ipReg = int.Parse(data.First().Substring(3));
            // var instr = data.Skip(1).Select(d => (d.Substring(0, 4), d.Substring(5).Split(' ').Select(i => int.Parse(i)).ToArray())).ToList();
            // var computer = new Computer();
            // computer.SetRegister(0, 1);
            // //instr[2].Item2[0] = 10551424;
            // var result = RunInstructions(ipReg, computer, instr);
            // System.Console.WriteLine(result);
            var c = 10551424;
            var sum = 0;
            for (int i = 1; i<= c; i++) {
                if (c % i == 0) {
                    System.Console.WriteLine(i);
                    sum+= i;
                }
            }
            System.Console.WriteLine(sum );
        }

        private static int RunInstructions(int ipReg, Computer computer, List<(string op, int[] values)> instr)
        {
            var ip = 0;
            while (ip < instr.Count)
            {
                computer.SetRegister(ipReg, ip);
                var step = instr[ip];
                computer.Execut(step.op, step.values[0], step.values[1], step.values[2]);
                ip = computer.GetRegister(ipReg);
                ip++;
                if (ip == 7)
                {
                    System.Console.WriteLine($"[{string.Join(" ", computer.GetRegister())}]");
                }
            }

            return computer.GetRegister(0);
        }

        string[] GetData() => File.ReadAllLines("input");
    }


    public class Computer
    {

        readonly int[] _register;
        readonly Dictionary<string, Action<int, int, int>> _op;

        public Computer()
        {
            _register = new int[6];
            _op = new Dictionary<string, Action<int, int, int>> {
                { "addr", (a, b, c) => _register[c] = _register[a] + _register[b] },
                { "addi", (a, b, c) => _register[c] = _register[a] + b },
                { "mulr", (a, b, c) => _register[c] = _register[a] * _register[b] },
                { "muli", (a, b, c) => _register[c] = _register[a] * b },
                { "banr", (a, b, c) => _register[c] = _register[a] & _register[b] },
                { "bani", (a, b, c) => _register[c] = _register[a] & b },
                { "borr", (a, b, c) => _register[c] = _register[a] | _register[b] },
                { "bori", (a, b, c) => _register[c] = _register[a] | b },
                { "setr", (a, b, c) => _register[c] = _register[a] },
                { "seti", (a, b, c) => _register[c] = a },
                { "gtir", (a, b, c) => _register[c] = a > _register[b] ? 1 : 0 },
                { "gtri", (a, b, c) => _register[c] = _register[a] > b ? 1 : 0 },
                { "gtrr", (a, b, c) => _register[c] = _register[a] > _register[b] ? 1 : 0 },
                { "eqir", (a, b, c) => _register[c] = a == _register[b] ? 1 : 0 },
                { "eqri", (a, b, c) => _register[c] = _register[a] == b ? 1 : 0 },
                { "eqrr", (a, b, c) => _register[c] = _register[a] == _register[b] ? 1 : 0 }
            };
        }

        public void SetRegister(int register, int value) => _register[register] = value;

        public void SetRegister(int[] reg)
        {
            for (int i = 0; i < _register.Length; i++)
            {
                _register[i] = reg[i];
            }
        }

        public void Execut(string op, int a, int b, int c) => _op[op](a, b, c);

        public IEnumerable<int> GetRegister() => _register.Select(i => i);

        public int GetRegister(int reg) => _register[reg];

    }
}

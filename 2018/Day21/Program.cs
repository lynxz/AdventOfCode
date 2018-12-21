using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21
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
            var ipReg = int.Parse(data.First().Substring(4));
            var instr = data.Skip(1).Select(d => (d.Substring(0, 4), d.Substring(5).Split(" ").Select(i => uint.Parse(i)).ToArray())).ToList();
            var ip = 0;
            var computer = new Computer(true);
           // computer.SetRegister(2985446, 0);
            while (ip >= 0 && ip < instr.Count)
            {
                computer.SetRegister(ipReg, (uint)ip);
                computer.Execut(instr[ip].Item1, instr[ip].Item2[0], instr[ip].Item2[1], instr[ip].Item2[2] );
                ip = (int)computer.GetRegister(ipReg);
                ip++;
            }
        }

        public void SecondStar()
        {
            var permTable = new HashSet<uint>();
            var data = GetData();
            var ipReg = int.Parse(data.First().Substring(4));
            var instr = data.Skip(1).Select(d => (d.Substring(0, 4), d.Substring(5).Split(" ").Select(i => uint.Parse(i)).ToArray())).ToList();
            var ip = 0;
            var computer = new Computer();
            var prev = 0u;
           // computer.SetRegister(2985446, 0);
            while (ip >= 0 && ip < instr.Count)
            {
                if (ip == 6) {
                    var reg4 = computer.GetRegister(4);
                    if (permTable.Contains(reg4)) {
                        System.Console.WriteLine(prev);
                        return;
                    }
                    permTable.Add(reg4);
                    prev = reg4;
                    if (permTable.Count % 1000 == 0) {
                        System.Console.WriteLine(permTable.Count);
                    }
                }
                computer.SetRegister(ipReg, (uint)ip);
                computer.Execut(instr[ip].Item1, instr[ip].Item2[0], instr[ip].Item2[1], instr[ip].Item2[2] );
                ip = (int)computer.GetRegister(ipReg);
                ip++;
            }

        }

        string[] GetData() => File.ReadAllLines("input");

    }

    public class Computer
    {

        readonly uint[] _register;
        readonly Dictionary<string, Action<uint, uint, uint>> _op;
        readonly bool _printDebug;


        public Computer(bool printDebug = false)
        {
            _printDebug = printDebug;
            _register = new uint[6];
            _op = new Dictionary<string, Action<uint, uint, uint>> {
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
                { "gtir", (a, b, c) => _register[c] = a > _register[b] ? 1u : 0 },
                { "gtri", (a, b, c) => _register[c] = _register[a] > b ? 1u : 0 },
                { "gtrr", (a, b, c) => _register[c] = _register[a] > _register[b] ? 1u : 0 },
                { "eqir", (a, b, c) => _register[c] = a == _register[b] ? 1u : 0 },
                { "eqri", (a, b, c) => _register[c] = _register[a] == b ? 1u : 0 },
                { "eqrr", (a, b, c) => _register[c] = _register[a] == _register[b] ? 1u : 0 }
            };
        }

        public void SetRegister(int register, uint value) => _register[register] = value;

        public void SetRegister(uint[] reg)
        {
            for (int i = 0; i < _register.Length; i++)
            {
                _register[i] = reg[i];
            }
        }

        public void Execut(string op, uint a, uint b, uint c)
        {
            if (_printDebug)
            {
                System.Console.WriteLine($"[ {string.Join(" ", _register)} ]");
                System.Console.WriteLine($"{op}: {a} {b} {c}");
            }
            _op[op](a, b, c);
            if (_printDebug)
            {
                System.Console.WriteLine($"[ {string.Join(" ", _register)} ]");
                System.Console.WriteLine();
            }
        }

        public IEnumerable<uint> GetRegister() => _register.Select(i => i);

        public uint GetRegister(int reg) => _register[reg];

    }
}

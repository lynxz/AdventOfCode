using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
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
            var computer = new IntComputer(GetData().ToList(), () => 1, v => System.Console.WriteLine(v));
            computer.RunProgram();
        }

        public void SecondStar()
        {
            var computer = new IntComputer(GetData().ToList(), () => 2, v => System.Console.WriteLine(v));
            computer.RunProgram();
        }


        IEnumerable<long> GetData()
            => File.ReadAllText("input.txt").Split(",").Select(v => long.Parse(v));

    }

    public class IntComputer
    {

        int _pos = 0;
        List<long> _ops;
        Func<long> _getInput;
        Action<long> _writeOutput;
        Dictionary<int, long> _heap;
        int _relative = 0;

        public IntComputer(List<long> ops, Func<long> getInput, Action<long> wrtieOutput)
        {
            _ops = ops;
            _getInput = getInput;
            _writeOutput = wrtieOutput;
            _heap = new Dictionary<int, long>();
        }

        public bool Halted { get; private set; }

        public void RunProgram()
        {
            while (true)
            {
                var data = Evaluate(_pos, _ops);
                if (data[0] == 99)
                {
                    Halted = true;
                    System.Console.WriteLine("HALT!");
                    return;
                }
                else if (data[0] == 1)
                {
                    Store(data[3], data[1] + data[2]);
                }
                else if (data[0] == 2)
                {
                    Store(data[3], data[1] * data[2]);
                }
                else if (data[0] == 3)
                {
                    Store(data[1], _getInput());
                }
                else if (data[0] == 4)
                {
                    _writeOutput(data[1]);
                }
                else if (data[0] == 5)
                {
                    if (data[1] != 0)
                    {
                        _pos = Convert.ToInt32(data[2]);
                        data = new long[0];
                    }
                }
                else if (data[0] == 6)
                {
                    if (data[1] == 0)
                    {
                        _pos = Convert.ToInt32(data[2]);
                        data = new long[0];
                    }
                }
                else if (data[0] == 7)
                {
                    Store(data[3], data[1] < data[2] ? 1u : 0u);
                }
                else if (data[0] == 8)
                {
                    Store(data[3], data[1] == data[2] ? 1u : 0u);
                }
                else if (data[0] == 9)
                {
                    _relative += Convert.ToInt32(data[1]);
                }
                else
                {
                    throw new InvalidOperationException();
                }
                _pos += data.Length;
            }
        }

        void Store(long pos, long value) => Store(Convert.ToInt32(pos), value);

        void Store(int pos, long value)
        {
            if (pos >= _ops.Count)
            {
                if (!_heap.ContainsKey(pos))
                {
                    _heap.Add(pos, 0);
                }
                _heap[pos] = value;
            }
            else
            {
                _ops[pos] = value;
            }
        }

        long[] Evaluate(int pos, List<long> ops)
        {
            var op = GetData(pos);
            if (op == 99)
            {
                return new[] { op };
            }
            else
            {
                var code = op % 100;
                var a = (op / 100) % 10;
                var b = (op / 1000) % 10;
                var c = (op / 10000) % 10;
                if (code == 3)
                {
                    return new[] {
                        code,
                        GetWriteAddress(pos + 1, a)
                    };
                }
                if (code == 4 || code == 9)
                {
                    return new[] {
                        code,
                        GetData(pos + 1, a),
                    };
                }
                if (code == 5 || code == 6)
                {
                    return new[] {
                        code,
                        GetData(pos + 1, a),
                        GetData(pos + 2, b)
                    };
                }
                return new[] {
                    code,
                    GetData(pos + 1, a),
                    GetData(pos + 2, b),
                    GetWriteAddress(pos + 3, c)
                };
            }
        }

        long GetWriteAddress(int pos, long mode)
            => mode == 2 ? GetData(pos) + _relative : GetData(pos);


        long GetData(int pos, long mode)
        {
            switch (mode)
            {
                case 1:
                    return GetData(pos);
                case 2:
                    return GetDataRelative(pos);
                default:
                    return GetDataPos(pos);
            }
        }

        long GetDataRelative(int pos)
        {
            var position = Convert.ToInt32(GetData(pos) + _relative);
            return GetData(position);
        }

        long GetDataPos(int pos)
        {
            var position = Convert.ToInt32(GetData(pos));
            return GetData(position);
        }

        long GetData(int pos)
        {
            if (pos >= _ops.Count)
            {
                return _heap.ContainsKey(pos) ? _heap[pos] : 0;
            }
            return _ops[pos];
        }

    }

}

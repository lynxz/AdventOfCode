using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
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
            var input = 0;
            var x = 0;
            var y = 0;
            var data = new List<(int x, int y, int tile)>();
            var ops = GetData();
            var computer = new IntComputer(ops, () => 1, i =>
            {
                switch (input)
                {
                    case 0:
                        x = Convert.ToInt32(i);
                        break;
                    case 1:
                        y = Convert.ToInt32(i);
                        break;
                    case 2:
                        data.Add((x, y, Convert.ToInt32(i)));
                        break;
                }
                input = (input + 1) % 3;
            });
            computer.RunProgram();
            System.Console.WriteLine(data.Count(d => d.tile == 2));
        }

        public void SecondStar()
        {
            var memmory = File.ReadAllLines("mem.txt").Select(i => int.Parse(i)).ToList(); ;
            var display = new int[37, 20];
            var score = 0;
            var input = 0;
            var x = 0;
            var y = 0;
            var ops = GetData();
            int step = 0;
            ops[0] = 2;
            var computer = new IntComputer(ops, () =>
            {
                if (step < memmory.Count)
                {
                    Console.Write('.');
                    return memmory[step++];
                }
                DrawDisplay(display);
                step = int.MaxValue;
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    memmory.Add(-1);
                    return -1;
                }
                if (key.Key == ConsoleKey.RightArrow)
                {
                    memmory.Add(1);
                    return 1;
                }
                memmory.Add(0);
                return 0;
            }
            , i =>
             {
                 switch (input)
                 {
                     case 0:
                         x = Convert.ToInt32(i);
                         break;
                     case 1:
                         y = Convert.ToInt32(i);
                         break;
                     case 2:
                         var tile = Convert.ToInt32(i);
                         if (x == -1)
                         {
                             score = tile;
                         }
                         else
                         {
                             display[x, y] = tile;
                         }
                         break;
                 }
                 input = (input + 1) % 3;
             });
            computer.RunProgram();
            using (var fs = new StreamWriter(new FileStream("mem.txt", FileMode.Truncate)))
            {
                foreach (var mem in memmory)
                {
                    fs.WriteLine(mem);
                }
            }
            System.Console.WriteLine(score);
        }

        void DrawDisplay(int[,] display)
        {
            System.Console.WriteLine();
            var output = new Dictionary<int, string> { { 0, " " }, { 1, "|" }, { 2, "#" }, { 3, "—" }, { 4, "o" } };
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 37; x++)
                {
                    Console.Write(output[display[x, y]]);
                }
                System.Console.WriteLine();
            }
        }

        List<long> GetData() =>
            File.ReadAllText("input.txt").Split(",").Select(i => long.Parse(i)).ToList();

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        enum Direction
        {
            Up,
            Left,
            Right,
            Down
        }

        public void FirstStar()
        {
            var ops = GetData();
            var color = true;
            var currentCoordinate = (x: 0, y: 0);
            var currentDirection = Direction.Up;
            var coordinates = new Dictionary<(int x, int y), long>();
            coordinates.Add(currentCoordinate, 0);
            var computer = new IntComputer(
                ops,
                () => coordinates.ContainsKey(currentCoordinate) ? coordinates[currentCoordinate] : 0,
                output =>
                {
                    if (color)
                    {
                        if (!coordinates.ContainsKey(currentCoordinate))
                        {
                            coordinates.Add(currentCoordinate, 0);
                        }
                        coordinates[currentCoordinate] = output;
                    }
                    else
                    {
                        currentDirection = GetNextDirection(currentDirection, output);
                        switch (currentDirection)
                        {
                            case Direction.Up:
                                currentCoordinate = (currentCoordinate.x, currentCoordinate.y + 1);
                                break;
                            case Direction.Down:
                                currentCoordinate = (currentCoordinate.x, currentCoordinate.y - 1);
                                break;
                            case Direction.Left:
                                currentCoordinate = (currentCoordinate.x - 1, currentCoordinate.y);
                                break;
                            case Direction.Right:
                                currentCoordinate = (currentCoordinate.x + 1, currentCoordinate.y);
                                break;
                        }
                    }
                    color = !color;
                });
            computer.RunProgram();
            System.Console.WriteLine(coordinates.Count);
        }

        public void SecondStar()
        {
            var ops = GetData();
            var color = true;
            var currentCoordinate = (x: 0, y: 0);
            var currentDirection = Direction.Up;
            var coordinates = new Dictionary<(int x, int y), long>();
            coordinates.Add(currentCoordinate, 1);
            var computer = new IntComputer(
                ops,
                () => coordinates.ContainsKey(currentCoordinate) ? coordinates[currentCoordinate] : 0,
                output =>
                {
                    if (color)
                    {
                        if (!coordinates.ContainsKey(currentCoordinate))
                        {
                            coordinates.Add(currentCoordinate, 0);
                        }
                        coordinates[currentCoordinate] = output;
                    }
                    else
                    {
                        currentDirection = GetNextDirection(currentDirection, output);
                        switch (currentDirection)
                        {
                            case Direction.Up:
                                currentCoordinate = (currentCoordinate.x, currentCoordinate.y + 1);
                                break;
                            case Direction.Down:
                                currentCoordinate = (currentCoordinate.x, currentCoordinate.y - 1);
                                break;
                            case Direction.Left:
                                currentCoordinate = (currentCoordinate.x - 1, currentCoordinate.y);
                                break;
                            case Direction.Right:
                                currentCoordinate = (currentCoordinate.x + 1, currentCoordinate.y);
                                break;
                        }
                    }
                    color = !color;
                });
            computer.RunProgram();
            var xOffset = Math.Abs(coordinates.Min(c => c.Key.x));
            var yOffset = Math.Abs(coordinates.Min(c => c.Key.y));
            var xMax = xOffset + coordinates.Keys.Max(c => c.x) + 1;
            var yMax = yOffset + coordinates.Keys.Max(c => c.y) + 1;
            var array = new bool[xMax, yMax];
            foreach (var coord in coordinates.Keys)
            {
                array[xOffset + coord.x, yMax - (yOffset + coord.y) - 1] = coordinates[coord] == 1;
            }
            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    System.Console.Write(array[x, y] ? '8' : ' ');
                }
                System.Console.WriteLine();
            }
        }

        Direction GetNextDirection(Direction currentDirection, long mode)
        {
            switch (currentDirection)
            {
                case Direction.Up:
                    return mode == 0 ? Direction.Left : Direction.Right;
                case Direction.Left:
                    return mode == 0 ? Direction.Down : Direction.Up;
                case Direction.Right:
                    return mode == 0 ? Direction.Up : Direction.Down;
                case Direction.Down:
                    return mode == 0 ? Direction.Right : Direction.Left;
            }
            throw new InvalidOperationException();
        }

        List<long> GetData() =>
            File.ReadAllText("input.txt").Split(',').Select(i => long.Parse(i)).ToList();

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

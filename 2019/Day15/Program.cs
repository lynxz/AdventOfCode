using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        void FirstStar()
        {
            var nodes = new List<Node>();
            var oxygen = GenerateGraph(nodes);
            Draw(nodes);
            System.Console.WriteLine(oxygen);
            System.Console.WriteLine(Djikstra(nodes, nodes[0], oxygen));
        }

        private (int, int) GenerateGraph(List<Node> nodes)
        {
            var oxygen = (0, 0);
            var steps = new Stack<int>();
            var blocks = new HashSet<(int X, int Y)>();
            var rando = new Random(12);
            var backtracking = false;
            var move = 2;
            var currentCoordinate = (x: 0, y: 0);
            nodes.Add(new Node { X = 0, Y = 0 });
            var ops = GetData();
            var computer = new IntComputer(
                ops,
                () =>
                {
                    return move;
                },
                i =>
                {
                    if (i == 0)
                    {
                        var coord = GetCoordinateFromMove(currentCoordinate, move);
                        if (!nodes.Any(n => n.X == coord.Item1 && n.Y == coord.Item2))
                        {
                            nodes.Add(new Node { X = coord.Item1, Y = coord.Item2, Type = 0 });
                        }
                        var queueSize = steps.Count;
                        move = Step(steps, currentCoordinate, nodes);
                        backtracking = queueSize > steps.Count;
                    }
                    if (i == 1 || i == 2)
                    {
                        if (!backtracking)
                        {
                            steps.Push(move);
                        }
                        currentCoordinate = GetCoordinateFromMove(currentCoordinate, move);
                        if (i == 2)
                        {
                            oxygen = currentCoordinate;
                        }
                        AddNode(currentCoordinate, nodes, 1);
                        var queueSize = steps.Count;
                        move = Step(steps, currentCoordinate, nodes);
                        backtracking = queueSize > steps.Count;
                    }
                });
            try
            {
                computer.RunProgram();
            }
            catch (Exception)
            {
                System.Console.WriteLine("Done mapping");
            }
            return oxygen;
        }

        void SecondStar()
        {
            var nodes = new List<Node>();
            var oxygen = GenerateGraph(nodes);
            var startNode = nodes.First(n => n.X == oxygen.Item1 && n.Y == oxygen.Item2);
            startNode.Type = 3;
            var minutes = 0;
            var next = new List<Node> {startNode};
            while (nodes.Any(n => n.Type == 1)) {
                var newNext = next.SelectMany(n => n.Nodes).Where(n => n.Type != 3).Distinct().ToList();
                newNext.ForEach(n => n.Type = 3);
                next = newNext;
                minutes++;
            }
            System.Console.WriteLine(minutes);
        }

        int Djikstra(List<Node> nodes, Node source, (int x, int y) target)
        {
            var dist = nodes.ToDictionary(n => n, n => int.MaxValue);
            var prev = nodes.ToDictionary(n => n, n => (Node)null);
            dist[source] = 0;
            Node targetNode = null;

            while (nodes.Count > 0)
            {
                var minDist = int.MaxValue;
                Node u = null;
                foreach (var node in nodes)
                {
                    if (minDist > dist[node])
                    {
                        minDist = dist[node];
                        u = node;
                    }
                }
                if (u.X == target.x && u.Y == target.y)
                {
                    targetNode = u;
                    break;
                }
                nodes.Remove(u);
                foreach (var v in u.Nodes)
                {
                    var alt = dist[u] + 1;
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }

            var q = targetNode;
            var counter = 0;
            while (q != null)
            {
                q = prev[q];
                counter++;
            }
            return counter - 1;
        }

        private int Step(Stack<int> steps, (int x, int y) currentCoordinate, List<Node> nodes)
        {
            var reversStep = new Dictionary<int, int> { { 1, 2 }, { 2, 1 }, { 3, 4 }, { 4, 3 } };
            int nextMove;
            var below = GetCoordinateFromMove(currentCoordinate, 2);
            var left = GetCoordinateFromMove(currentCoordinate, 3);
            var right = GetCoordinateFromMove(currentCoordinate, 4);
            var up = GetCoordinateFromMove(currentCoordinate, 1);
            if (!nodes.Any(n => n.X == below.Item1 && n.Y == below.Item2))
            {
                nextMove = 2;
            }
            else if (!nodes.Any(n => n.X == left.Item1 && n.Y == left.Item2))
            {
                nextMove = 3;
            }
            else if (!nodes.Any(n => n.X == right.Item1 && n.Y == right.Item2))
            {
                nextMove = 4;
            }
            else if (!nodes.Any(n => n.X == up.Item1 && n.Y == up.Item2))
            {
                nextMove = 1;
            }
            else
            {
                nextMove = reversStep[steps.Pop()];
            }
            return nextMove;
        }

        void Draw(List<Node> nodes)
        {
            var minX = nodes.Min(n => n.X);
            var minY = nodes.Min(n => n.Y);
            var maxX = nodes.Max(n => n.X);
            var maxY = nodes.Max(n => n.Y);
            var offsetX = Math.Abs(minX);
            var offsetY = Math.Abs(minY);
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var tile = " ";
                    var node = nodes.FirstOrDefault(n => n.X == x && n.Y == y);
                    if (node != null)
                    {
                        tile = node.Type == 0 ? "#" : ".";
                    }
                    Console.Write(tile);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        void AddNode((int x, int y) currentCoordinate, List<Node> nodes, int type)
        {
            if (!nodes.Any(n => n.X == currentCoordinate.x && n.Y == currentCoordinate.y))
            {
                var node = new Node { X = currentCoordinate.x, Y = currentCoordinate.y, Type = type };
                foreach (var offset in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
                {
                    var neighbor = nodes.FirstOrDefault(n => n.X == node.X + offset.Item1 && n.Y == node.Y + offset.Item2);
                    if (neighbor != null)
                    {
                        neighbor.Nodes.Add(node);
                        node.Nodes.Add(neighbor);
                    }
                }
                nodes.Add(node);
            }
        }

        (int, int) GetCoordinateFromMove((int x, int y) currentCoordinate, int direction)
        {
            switch (direction)
            {
                case 1:
                    return (currentCoordinate.x, currentCoordinate.y + 1);
                case 2:
                    return (currentCoordinate.x, currentCoordinate.y - 1);
                case 3:
                    return (currentCoordinate.x - 1, currentCoordinate.y);
                case 4:
                    return (currentCoordinate.x + 1, currentCoordinate.y);
            }
            throw new InvalidOperationException();
        }

        List<long> GetData() =>
            File.ReadAllText("input.txt").Split(",").Select(i => long.Parse(i)).ToList();

    }

    public class Node
    {

        public Node()
        {
            Nodes = new List<Node>();
        }

        public int Type { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public List<Node> Nodes { get; }

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
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
            var highest = -1;
            var highestPerm = new int[4];
            var permutations = GeneratePermuations(new int[] { 0, 1, 2, 3, 4 }, 0, 4);
            foreach (var permutation in permutations)
            {
                var input = 0;
                var phases = permutation;
                for (int i = 0; i < phases.Length; i++)
                {
                    var ops = GetData().ToList();
                    input = RunProgram(ops, phases[i], input);
                }
                if (input > highest)
                {
                    highest = input;
                    highestPerm = permutation;
                }
            }
            System.Console.WriteLine($"[{string.Join(',', highestPerm)}]");
            System.Console.WriteLine(highest);
        }

        void SecondStar()
        {
            var highest = -1;
            var highestPerm = new int[4];
            var permutations = GeneratePermuations(new int[] { 5, 6, 7, 8, 9 }, 0, 4);
            foreach (var permutation in permutations)
            {
                var signal = 0;
                int step = 0;
                var phase = permutation;
                var readPhase = Enumerable.Repeat(true, 5).ToArray();

                var amps = Enumerable.Range(0, 5).Select(c => new IntComputer(GetData().ToList(), () => { var result = readPhase[c] ? phase[c] : signal; readPhase[c] = false; return result; }, i => signal = i)).ToArray();

                while (true)
                {
                    amps[step].RunProgram();
                    if (amps[step].Halted)
                    {
                        break;
                    }
                    step = (step + 1) % 5;
                }
                if (signal > highest) {
                    highest = signal;
                    highestPerm = permutation;
                }
            }
            System.Console.WriteLine($"[{string.Join(',', highestPerm)}]");
            System.Console.WriteLine(highest);
        }

        IEnumerable<int[]> GeneratePermuations(int[] array, int l, int r)
        {
            var localArray = array.ToArray();
            if (l == r)
            {
                return Enumerable.Repeat(localArray, 1);
            }
            else
            {
                var result = new List<int[]>();
                for (int i = l; i <= r; i++)
                {
                    Swap(localArray, l, i);
                    result.AddRange(GeneratePermuations(localArray, l + 1, r));
                    Swap(localArray, l, i);
                }
                return result;
            }
        }

        void Swap(int[] array, int r, int l)
        {
            var temp = array[r];
            array[r] = array[l];
            array[l] = temp;
        }

        private int RunProgram(List<int> ops, int phase, int input)
        {
            var first = true;
            var pos = 0;
            while (true)
            {
                var data = Evaluate(pos, ops);
                if (data[0] == 99)
                {
                    System.Console.WriteLine("HALT!");
                    return -1;
                }
                else if (data[0] == 1)
                {
                    ops[data[3]] = data[1] + data[2];
                }
                else if (data[0] == 2)
                {
                    ops[data[3]] = data[1] * data[2];
                }
                else if (data[0] == 3)
                {
                    if (first)
                    {
                        ops[data[1]] = phase;
                        first = false;
                    }
                    else
                    {
                        ops[data[1]] = input;
                    }

                }
                else if (data[0] == 4)
                {
                    return data[1];
                }
                else if (data[0] == 5)
                {
                    if (data[1] != 0)
                    {
                        pos = data[2];
                        data = new int[0];
                    }
                }
                else if (data[0] == 6)
                {
                    if (data[1] == 0)
                    {
                        pos = data[2];
                        data = new int[0];
                    }
                }
                else if (data[0] == 7)
                {
                    ops[data[3]] = data[1] < data[2] ? 1 : 0;
                }
                else if (data[0] == 8)
                {
                    ops[data[3]] = data[1] == data[2] ? 1 : 0;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                pos += data.Length;
            }
        }

        int[] Evaluate(int pos, List<int> ops)
        {
            if (ops[pos] == 99)
            {
                return new[] { ops[pos] };
            }
            else if (ops[pos] == 1 || ops[pos] == 2 || ops[pos] == 7 || ops[pos] == 8)
            {
                return new[] { ops[pos], ops[ops[pos + 1]], ops[ops[pos + 2]], ops[pos + 3] };
            }
            else if (ops[pos] == 3)
            {
                return new[] { ops[pos], ops[pos + 1] };
            }
            else if (ops[pos] == 4)
            {
                return new[] { ops[pos], ops[ops[pos + 1]] };
            }
            else if (ops[pos] == 5 || ops[pos] == 6)
            {
                return new[] { ops[pos], ops[ops[pos + 1]], ops[ops[pos + 2]] };
            }
            else if (ops[pos] == 6)
            {
                return new[] { ops[pos], ops[ops[pos + 1]] };
            }
            else if (ops[pos] > 99)
            {
                var code = ops[pos] % 100;
                var a = (ops[pos] / 100) % 10;
                var b = (ops[pos] / 1000) % 10;
                var c = (ops[pos] / 10000) % 10;
                if (code == 3 || code == 4)
                {
                    return new[] {
                        code,
                        a == 1 ? ops[pos + 1] : ops[ops[pos + 1]],
                    };
                }
                if (code == 5 || code == 6)
                {
                    return new[] {
                    code,
                    a == 1 ? ops[pos + 1] : ops[ops[pos + 1]],
                    b == 1 ? ops[pos + 2] : ops[ops[pos + 2]]
                };
                }
                return new[] {
                    code,
                    a == 1 ? ops[pos + 1] : ops[ops[pos + 1]],
                    b == 1 ? ops[pos + 2] : ops[ops[pos + 2]],
                    ops[pos + 3]
                };
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        IEnumerable<int> GetData()
            => File.ReadAllLines("input.txt").First().Split(',').Select(i => int.Parse(i));

    }

    public class IntComputer
    {

        int _pos = 0;
        List<int> _ops;
        Func<int> _getInput;
        Action<int> _writeOutput;


        public IntComputer(List<int> ops, Func<int> getInput, Action<int> wrtieOutput)
        {
            _ops = ops;
            _getInput = getInput;
            _writeOutput = wrtieOutput;
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
                    _ops[data[3]] = data[1] + data[2];
                }
                else if (data[0] == 2)
                {
                    _ops[data[3]] = data[1] * data[2];
                }
                else if (data[0] == 3)
                {
                    _ops[data[1]] = _getInput();
                }
                else if (data[0] == 4)
                {
                    _writeOutput(data[1]);
                    _pos += data.Length;
                    return;
                }
                else if (data[0] == 5)
                {
                    if (data[1] != 0)
                    {
                        _pos = data[2];
                        data = new int[0];
                    }
                }
                else if (data[0] == 6)
                {
                    if (data[1] == 0)
                    {
                        _pos = data[2];
                        data = new int[0];
                    }
                }
                else if (data[0] == 7)
                {
                    _ops[data[3]] = data[1] < data[2] ? 1 : 0;
                }
                else if (data[0] == 8)
                {
                    _ops[data[3]] = data[1] == data[2] ? 1 : 0;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                _pos += data.Length;
            }
        }

        int[] Evaluate(int pos, List<int> ops)
        {
            if (ops[pos] == 99)
            {
                return new[] { ops[pos] };
            }
            else if (ops[pos] == 1 || ops[pos] == 2 || ops[pos] == 7 || ops[pos] == 8)
            {
                return new[] { ops[pos], ops[ops[pos + 1]], ops[ops[pos + 2]], ops[pos + 3] };
            }
            else if (ops[pos] == 3)
            {
                return new[] { ops[pos], ops[pos + 1] };
            }
            else if (ops[pos] == 4)
            {
                return new[] { ops[pos], ops[ops[pos + 1]] };
            }
            else if (ops[pos] == 5 || ops[pos] == 6)
            {
                return new[] { ops[pos], ops[ops[pos + 1]], ops[ops[pos + 2]] };
            }
            else if (ops[pos] == 6)
            {
                return new[] { ops[pos], ops[ops[pos + 1]] };
            }
            else if (ops[pos] > 99)
            {
                var code = ops[pos] % 100;
                var a = (ops[pos] / 100) % 10;
                var b = (ops[pos] / 1000) % 10;
                var c = (ops[pos] / 10000) % 10;
                if (code == 3 || code == 4)
                {
                    return new[] {
                        code,
                        a == 1 ? ops[pos + 1] : ops[ops[pos + 1]],
                    };
                }
                if (code == 5 || code == 6)
                {
                    return new[] {
                    code,
                    a == 1 ? ops[pos + 1] : ops[ops[pos + 1]],
                    b == 1 ? ops[pos + 2] : ops[ops[pos + 2]]
                };
                }
                return new[] {
                    code,
                    a == 1 ? ops[pos + 1] : ops[ops[pos + 1]],
                    b == 1 ? ops[pos + 2] : ops[ops[pos + 2]],
                    ops[pos + 3]
                };
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5
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
            var pos = 0;
            var ops = GetData().ToList();
            while (true)
            {
                var data = Evaluate(pos, ops);
                if (data[0] == 99)
                {
                    System.Console.WriteLine("HALT!");
                    return;
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
                    ops[data[1]] = 1;
                }
                else if (data[0] == 4)
                {
                    System.Console.WriteLine(data[1]);
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
                if (code == 5 || code == 6) {
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

        public void SecondStar()
        {
            var pos = 0;
            var ops = GetData().ToList();
            while (true)
            {
                var data = Evaluate(pos, ops);
                if (data[0] == 99)
                {
                    System.Console.WriteLine("HALT!");
                    return;
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
                    ops[data[1]] = 5;
                }
                else if (data[0] == 4)
                {
                    System.Console.WriteLine(data[1]);
                }
                else if (data[0] == 5)
                {
                    if (data[1] != 0) {
                        pos = data[2];
                        data = new int[0];
                    }
                }
                else if (data[0] == 6)
                {
                    if (data[1] == 0) {
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


        IEnumerable<int> GetData()
            => File.ReadAllLines("input.txt").First().Split(',').Select(i => int.Parse(i));

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2
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
            var ops = GetData().First().Split(',').Select(s => int.Parse(s)).ToList();
            ops[1] = 12;
            ops[2] = 2;
            while (true)
            {
                var data = ops.Skip(pos).Take(4).ToArray();
                if (data[0] == 99)
                {
                    System.Console.WriteLine(ops[0]);
                    return;
                }
                else if (data[0] == 1)
                {
                    ops[data[3]] = ops[data[1]] + ops[data[2]];
                }
                else if (data[0] == 2)
                {
                    ops[data[3]] = ops[data[1]] * ops[data[2]];
                }
                else
                {
                    throw new InvalidOperationException();
                }
                pos += 4;
            }
        }

        public void SecondStar()
        {
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    if (RunComputer(noun, verb) == 19690720)
                    {
                        System.Console.WriteLine(100 * noun + verb);
                        return;
                    }
                }
            }
        }

        int RunComputer(int noun, int verb)
        {
            var pos = 0;
            var ops = GetData().First().Split(',').Select(s => int.Parse(s)).ToList();
            ops[1] = noun;
            ops[2] = verb;
            while (true)
            {
                var data = ops.Skip(pos).Take(4).ToArray();
                if (data[0] == 99)
                {
                    return ops[0];
                }
                else if (data[0] == 1)
                {
                    ops[data[3]] = ops[data[1]] + ops[data[2]];
                }
                else if (data[0] == 2)
                {
                    ops[data[3]] = ops[data[1]] * ops[data[2]];
                }
                else
                {
                    throw new InvalidOperationException();
                }
                pos += 4;
            }
        }

        IEnumerable<string> GetData()
            => File.ReadAllLines("input.txt");


    }
}

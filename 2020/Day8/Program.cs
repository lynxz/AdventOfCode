using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {

        static void Main(string[] args)
        {
            var prg = new Program();
            prg.FirstStar();
            prg.SecondStar();
        }

        void FirstStar()
        {
            var data = GetData();
            var acc = Computer.RunProgram(data);
            System.Console.WriteLine(acc.Accumulator);
        }

        void SecondStar()
        {
            var data = GetData();

            for (int i = 0; i < data.Count; i++)
            {
                var program = new List<Op>(data);

                if (program[i].Code == "jmp")
                    program[i] = new Op("nop", program[i].Value);
                else if (program[i].Code == "nop")
                    program[i] = new Op("jmp", program[i].Value);
                else
                    continue;

                var result = Computer.RunProgram(program);

                if (result.Completion)
                {
                    System.Console.WriteLine(result.Accumulator);
                    break;
                }
            }
        }

        List<Op> GetData() => File.ReadAllLines("data.txt").Select(d => new Op(d.Split(' ')[0], int.Parse(d.Split(' ')[1]))).ToList();

    }

    class Computer
    {
        public static (int Accumulator, bool Completion) RunProgram(List<Op> program)
        {
            var acc = 0;
            var line = 0;
            var set = new HashSet<int>();

            while (!set.Contains(line) && line < program.Count)
            {
                var op = program[line];
                set.Add(line);
                switch (op.Code)
                {
                    case "nop":
                        line++;
                        break;
                    case "acc":
                        acc += op.Value;
                        line++;
                        break;
                    case "jmp":
                        line += op.Value;
                        break;
                }

            }
            return (acc, line >= program.Count);
        }

    }

    record Op(string Code, int Value);

}

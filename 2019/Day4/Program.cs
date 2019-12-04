using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4
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
            var start = 284639;
            var stop = 748759;
            var d = new[] { 1, 10, 100, 1000, 10000, 100_000, 1_000_000 };
            var count = 0;

            for (; start < stop; start++)
            {
                var parts = Enumerable.Range(0, d.Length - 1).Select(i =>  (start / d[i]) % 10 ).ToArray();
                var correct = true;
                var anyDouble = false;
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    if (parts[i] < parts[i + 1])
                    {
                        correct = false;
                    }
                    if (parts[i] == parts[i + 1])
                    {
                        anyDouble = true;
                    }
                }
                if (correct && anyDouble)
                {
                    count++;
                }
            }
            System.Console.WriteLine(count);
        }

        public void SecondStar()
        {
            var start = 284639;
            var stop = 748759;
            var d = new[] { 1, 10, 100, 1000, 10000, 100_000, 1_000_000 };
            var count = 0;

            for (; start < stop; start++)
            {
                var parts = Enumerable.Range(0, d.Length - 1).Select(i =>  (start / d[i]) % 10 ).ToArray();
                var correct = true;
                var anyDouble = false;
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    if (parts[i] < parts[i + 1])
                    {
                        correct = false;
                    }
                    if (parts[i] == parts[i + 1])
                    {
                        if ((i + 2 == parts.Length || parts[i+2] != parts[i]) && (i -1 == -1 || parts[i-1] != parts[i])) {
                            anyDouble = true;
                        }
                        
                    }
                }
                if (correct && anyDouble)
                {
                    count++;
                }
            }
            System.Console.WriteLine(count);
        }

        IEnumerable<string> GetData()
            => File.ReadAllLines("input.txt");

    }
}

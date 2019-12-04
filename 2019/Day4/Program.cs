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
            var count = 0;

            for (; start < stop; start++)
            {
                var parts = start.ToString().Select(c => int.Parse(c.ToString())).ToArray();
                var anyDouble = Enumerable.Range(0, parts.Length - 1).Any(i => parts[i] == parts[i + 1]);
                var correct = Enumerable.Range(0, parts.Length - 1).All(i => parts[i] <= parts[i + 1]);
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
            var count = 0;

            for (; start < stop; start++)
            {
                var parts = start.ToString().Select(c => int.Parse(c.ToString())).ToArray();
                var anyDouble = Enumerable.Range(0, parts.Length - 1).Any(i => (parts[i] == parts[i + 1]) && ((i + 2 == parts.Length || parts[i + 2] != parts[i]) && (i - 1 == -1 || parts[i - 1] != parts[i])));
                var correct = Enumerable.Range(0, parts.Length - 1).All(i => parts[i] <= parts[i + 1]);
                if (correct && anyDouble)
                {
                    count++;
                }
            }
            System.Console.WriteLine(count);
        }


    }
}

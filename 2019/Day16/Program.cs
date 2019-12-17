using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day16
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
            var basePattern = new[] { 0, 1, 0, -1 };
            var numbers = GetData();
            for (int j = 0; j < 100; j++)
            {
                var newSeries = new List<int>();
                for (int i = 1; i <= numbers.Count; i++)
                {
                    var pattern = Enumerable.Range(0, 4).SelectMany(n => Enumerable.Repeat(basePattern[n], i)).ToList();
                    var repeat = numbers.Count / pattern.Count;
                    repeat++;
                    var repeatPattern = Enumerable.Repeat(pattern, repeat).SelectMany(p => p).Skip(1).ToArray();
                    var sum = Enumerable.Range(0, numbers.Count).Sum(n => numbers[n] * repeatPattern[n]);
                    newSeries.Add(Math.Abs(sum) % 10);
                }
                numbers = newSeries;
                System.Console.WriteLine(j);
            }
            System.Console.WriteLine(string.Join("", numbers.Take(8)));
        }

        void SecondStar()
        {
            var numbers = Enumerable.Repeat(GetData(), 10000).SelectMany(i => i).ToArray();
            var newSeries = new int[numbers.Length];
            var offset = Int32.Parse(string.Join("", numbers.Take(7)));
            for (int j = 0; j < 100; j++)
            {
                var sum = numbers.Skip(offset).Sum();
                for (int i = offset; i < numbers.Length; i++)
                {
                    newSeries[i] = (sum % 10);
                    sum -= numbers[i];
                }
                Array.Copy(newSeries, numbers, numbers.Length);
            }
            System.Console.WriteLine(string.Join("", numbers.Skip(offset).Take(8)));
        }

        List<int> GetData() =>
            File.ReadAllText("input.txt").Select(c => int.Parse(c.ToString())).ToList();

    }
}

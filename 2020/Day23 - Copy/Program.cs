using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            //prog.FirstStar();
            prog.SecondStar();
        }

        private void FirstStar()
        {
            var data = GetData();
            var blah = Iterate2(data.ToArray(), 100);
            var startIndex = Array.IndexOf(blah, 1);
            Enumerable.Range(1, 8);
            System.Console.WriteLine(string.Join("", Enumerable.Range(1, 8).Select(i => blah[(startIndex + i) % blah.Length])));
        }

        private static void Iterate(List<int> data, int iterations)
        {
            
            var currentValue = data.First();
            var tmp = new int[3];
            var stopwatch = new Stopwatch();
            var max = data.Max();
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                var currentIndex = data.IndexOf(currentValue);
                for (int j = 1; j < 4; j++)
                    tmp[j - 1] = data[(currentIndex + j) % data.Count];

                var dest = currentValue - 1;
                var done = false;
                while (!done)
                {
                    done = true;
                    if (dest <= 0)
                        dest = max;
                    if (tmp.Contains(dest))
                    {
                        dest--;
                        done = false;
                    }
                }
                for (int j = 0; j < 3; j++)
                    data.Remove(tmp[j]);
                var destIndex = data.IndexOf(dest);
                data.InsertRange(destIndex + 1, tmp);

                currentIndex = data.IndexOf(currentValue) + 1;
                if (currentIndex >= data.Count)
                    currentIndex = 0;
                currentValue = data[currentIndex];
                if (i % 100_000 == 0)
                {
                    System.Console.WriteLine(stopwatch.Elapsed);
                    stopwatch.Restart();
                }
            }
        }

        private static int[] Iterate2(int[] data, int iterations)
        {
            var tmp = new int[3];
            var stopwatch = new Stopwatch();
            var max = data.Max();
            stopwatch.Start();
            var currentIndex = 0;
            for (int i = 0; i < iterations; i++)
            {
                var currentValue = data[currentIndex];
                for (int j = 1; j < 4; j++)
                    tmp[j - 1] = data[(currentIndex + j) % data.Length];

                var dest = currentValue - 1;
                var done = false;
                while (!done)
                {
                    done = true;
                    if (dest <= 0)
                        dest = max;
                    if (tmp.Contains(dest))
                    {
                        dest--;
                        done = false;
                    }
                }

                var start = (currentIndex + 1) % data.Length;
                do {
                    data[start] = data[(start + 3)% data.Length];
                    start = (start + 1) % data.Length;
                } while (data[(start + 2)% data.Length] != dest);

                for (int j = 0; j < 3; j++) 
                    data[(start + j) % data.Length] = tmp[j];

                currentIndex = (currentIndex + 1) % data.Length;

                if (i % 100_000 == 0)
                {
                    System.Console.WriteLine(stopwatch.Elapsed);
                    stopwatch.Restart();
                }
                System.Console.WriteLine(i + ": " + string.Join(" ", data));
                var indexOfOne = Array.IndexOf(data, 1);
                System.Console.WriteLine("Index of one: " + indexOfOne);
            }
            return data;
        }

        private void SecondStar()
        {
            var data = GetData();
            for (int i = 10; i <= 30; i++)
            {
                data.Add(i);
            }
            Iterate2(data.ToArray(), 300);
            var index = data.IndexOf(1);
            System.Console.WriteLine(Convert.ToUInt64(data[(index + 1) % data.Count]) * Convert.ToUInt64(data[(index + 2) % data.Count]));
        }

        List<int> GetData() => "198753462".Select(c => int.Parse(c.ToString())).ToList();

    }
}

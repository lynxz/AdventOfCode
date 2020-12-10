using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
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
            var max = data.Max() + 3;
            data.Add(max);
            var last = 0L;
            var three = 0;
            var one = 0;
            while (data.Any())
            {
                var s = data.Where(d => d <= last + 3 && d > last).Min();
                data.Remove(s);
                if (s - last == 3)
                    three++;
                else if (s - last == 1)
                    one++;

                last = s;
            }
            System.Console.WriteLine(one * three);
        }

        void SecondStar()
        {
            var data = GetData();
            data.Add(data.Max() + 3);
            data.Add(0);
            data.Sort();
            var result = Print(data, 0, new Dictionary<int, long> { { data.Count - 2, 1 } }, 0L);
            System.Console.WriteLine(result);
        }

        long Print(List<long> data, int index, Dictionary<int, long> lookup, long counter)
        {
            if (lookup.ContainsKey(index))
            {
                return lookup[index];
            }
            var count = 0L;
            foreach (var value in data.Where(d => d <= data[index] + 3 && d > data[index]))
            {
                count += Print(data, data.IndexOf(value), lookup, counter);
            }
            lookup[index] = count;
            return count;
        }

        List<long> GetData() => File.ReadAllLines("data.txt").Select(v => long.Parse(v)).ToList();
    }
}

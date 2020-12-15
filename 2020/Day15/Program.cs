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
            var prog = new Program();
            prog.FirstStar();
            prog.SecondStar();
        }


        void FirstStar()
        {
            var data = GetData();
            var counter = data.Count;
            while (data.Count != 2020)
            {
                var n = data.Last();
                var newNum = 0;
                if (data.IndexOf(n) != data.Count - 1)
                {
                    var indexes = Enumerable.Range(0, data.Count).Where(i => data[i] == n).Reverse().Take(2);
                    newNum = indexes.First() - indexes.Last();
                }
                data.Add(newNum);
            }
            System.Console.WriteLine(data.Last());
        }

        void SecondStar()
        {
            var data = GetData();
            var dict = Enumerable.Range(0, data.Count).ToDictionary(i => data[i], i => new[] { 0, 1 + i });
            var counter = data.Count;
            var lastNumber = data.Last();
            while (counter != 30000000)
            {
                if (dict[lastNumber][0] != 0)
                {
                    var newNum = dict[lastNumber][1] - dict[lastNumber][0];
                    if (dict.ContainsKey(newNum))
                    {
                        dict[newNum][0] = dict[newNum][1];
                        dict[newNum][1] = counter + 1;
                    }
                    else
                    {
                        dict[newNum] = new int[] { 0, counter + 1 };
                    }
                    lastNumber = newNum;
                }
                else
                {
                    if (!dict.ContainsKey(0))
                    {
                        dict[0] = new[] { 0, 0 };
                    }
                    dict[0][0] = dict[0][1];
                    dict[0][1] = counter + 1;
                    lastNumber = 0;
                }
                if (lastNumber < 0)
                {
                    throw new Exception();
                }
                counter++;
            }
            System.Console.WriteLine(lastNumber);
        }

        List<int> GetData() => "6,3,15,13,1,0".Split(',').Select(i => int.Parse(i)).ToList();
    }
}

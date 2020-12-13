using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            //prog.FirstStar();
            prog.SecondStar();
        }

        void FirstStar()
        {
            var data = GetData();
            var timestamp = long.Parse(data[0]);
            var lines = data[1].Split(',').Where(s => s != "x").Select(x => long.Parse(x)).ToList();
            var busTime = lines.ToDictionary(k => k, v => 0L);

            while (lines.Any(l => busTime[l] < timestamp))
            {
                foreach (var line in lines)
                {
                    if (busTime[line] < timestamp)
                    {
                        busTime[line] += line;
                    }
                }
            }

            var minLine = 0L;
            var minTime = long.MaxValue;
            foreach (var line in lines)
            {
                var waitTime = busTime[line] - timestamp;
                if (waitTime < minTime)
                {
                    minTime = waitTime;
                    minLine = line;
                }
            }
            System.Console.WriteLine(minLine * minTime);
        }

        void SecondStar()
        {
            var data = GetData()[1].Split(',');
            var lines = Enumerable.Range(0, data.Length).Where(i => data[i] != "x").Select(i => (Line: ulong.Parse(data[i]), Index: Convert.ToUInt64(i))).ToList();

            var counter = 1UL;
            var increment = 1UL;
            foreach (var l in lines.Skip(1))
            {

                while (((lines[0].Line * counter) + l.Index) % l.Line != 0)
                    counter += increment;

                increment *= l.Line;
            }

            System.Console.WriteLine(counter * lines[0].Line);
        }


        List<string> GetData() => File.ReadAllLines("data.txt").ToList();
    }
}

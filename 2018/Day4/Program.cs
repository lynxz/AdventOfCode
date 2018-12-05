using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            SecondStar();
        }

        private static void FirstStar()
        {
            Dictionary<int, int[]> sleep = GetSleepStatistics();

            var id = 0;
            var minute = 0;
            var highSum = 0;

            foreach (var kvp in sleep)
            {
                var sum = kvp.Value.Sum();
                if (sum > highSum)
                {
                    id = kvp.Key;
                    highSum = sum;
                }
            }
            highSum = 0;
            for (int i = 0; i < sleep[id].Length; i++)
            {
                if (sleep[id][i] > highSum)
                {
                    minute = i;
                    highSum = sleep[id][i];
                }
            }
            System.Console.WriteLine(id * minute);
        }

        private static void SecondStar() {
            var sleep = GetSleepStatistics();

            var id = 0;
            var minute = 0;
            var highest = 0;

            foreach (var kvp in sleep)
            {
                var high = kvp.Value.Max();
                if (high > highest)
                {
                    id = kvp.Key;
                    highest = high;
                    minute = Enumerable.Range(0, 60).Last(n => kvp.Value[n] == high);
                }
            }

            System.Console.WriteLine(id * minute);
        }

        private static Dictionary<int, int[]> GetSleepStatistics()
        {
            var data = File.ReadAllLines("input")
                        .OrderBy(i => i)
                        .Select(s => (time: DateTime.Parse(s.Substring(1, 16)), evnt: s.Substring(19)))
                        .ToArray();

            var currentGuard = 0;
            var sleep = new Dictionary<int, int[]>();
            for (int i = 0; i < data.Length; i++)
            {
                var match = Regex.Match(data[i].evnt, @"\d+");
                if (match.Success)
                {
                    currentGuard = int.Parse(match.Value);
                    if (!sleep.ContainsKey(currentGuard))
                    {
                        sleep.Add(currentGuard, new int[60]);
                    }
                }
                else if (data[i].evnt.Equals("falls asleep"))
                {
                    var start = data[i].time;
                    var stop = data[++i].time;
                    if (!data[i].evnt.StartsWith("wakes up"))
                    {
                        throw new Exception("Bajs");
                    }
                    var elapsed = stop - start;
                    var startIndex = start.Minute;
                    for (int j = 0; j < Convert.ToInt32(elapsed.TotalMinutes); j++)
                    {
                        sleep[currentGuard][startIndex + j] += 1;
                    }
                }
            }

            return sleep;
        }
    }
}

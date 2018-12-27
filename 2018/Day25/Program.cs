using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.FirstStar();
        }

        public void FirstStar()
        {
            var data = GetData();
            var set = data.Select(d => new List<int[]> { d.Split(',').Select(i => int.Parse(i)).ToArray() }).ToList();
            var prevSize = set.Count + 1;
            while (set.Count < prevSize)
            {
                prevSize = set.Count;
                var removedSet = new HashSet<List<int[]>>();
                for (int i = 0; i < set.Count - 1; i++)
                {
                    for (int j = i + 1; j < set.Count; j++)
                    {
                        if (!removedSet.Contains(set[i]) && !removedSet.Contains(set[j]))
                        {
                            if (set[i].Any(s => set[j].Any(n => Enumerable.Range(0, 4).Sum(k => Math.Abs(s[k] - n[k])) <= 3)))
                            {
                                set[i].AddRange(set[j]);
                                removedSet.Add(set[j]);
                            }
                        }
                    }
                }
                foreach (var removed in removedSet)
                {
                    set.Remove(removed);
                }
            }
            System.Console.WriteLine(set.Count);
        }

        string[] GetData() => File.ReadAllLines("input");

    }
}

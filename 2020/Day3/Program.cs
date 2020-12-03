using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
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
            var data = GetData().ToList();
            var trees = GetSlopes(data, 3, 1);
            System.Console.WriteLine(trees);
        }

        void SecondStar()
        {
            var data = GetData().ToList();
            var trees = new List<int>();
            trees.Add(GetSlopes(data, 1, 1));
            trees.Add(GetSlopes(data, 3, 1));
            trees.Add(GetSlopes(data, 5, 1));
            trees.Add(GetSlopes(data, 7, 1));
            trees.Add(GetSlopes(data, 1, 2));

            System.Console.WriteLine(trees.Aggregate((ulong)1, (val, next) => val*((ulong)next)));
        }

        int GetSlopes(List<char[]> data, int right, int down) {
            var trees = 0;
            var rightCounter = 0;
            for (int i = down; i < data.Count; i += down)
            {
                rightCounter += right;
                rightCounter %= data[i].Length;

                if (data[i][rightCounter] == '#') 
                    trees++;  
            }
            return trees;
        }

        IEnumerable<char[]> GetData() => File.ReadAllLines("data.txt").Select(s => s.ToCharArray());

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            prog.FirstStar();
            prog.SecondStar();
        }

        public void FirstStar()
        {
            var data = GetData().ToList();
            for (int i = 0; i < data.Count - 1; i++)
            {
                for (int j = i + 1; j < data.Count; j++)
                {
                    if (data[i] + data[j] == 2020)
                    {
                        System.Console.WriteLine(data[i] * data[j]);
                    }
                }
            }
        }

        public void SecondStar()
        {
            var data = GetData().ToList();
            for (int i = 0; i < data.Count - 2; i++)
            {
                for (int j = i + 1; j < data.Count - 1; j++)
                {
                    for (int k = j + 1; k < data.Count; k++)
                    {
                        if (data[i] + data[j] + data[k] == 2020)
                        {
                            System.Console.WriteLine(data[i] * data[j] * data[k]);
                        }
                    }
                }
            }
        }

        static IEnumerable<int> GetData()
        {
            return File.ReadAllLines("data.txt").Select(l => int.Parse(l));
        }
    }
}

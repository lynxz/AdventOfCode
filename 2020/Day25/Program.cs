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
            var prog = new Program();
            prog.FirstStar();
        }

        private void FirstStar()
        {
            var data = GetData();
            var loopSizes = data.Select(l => GetLoop(l)).ToList();
            var value = 1L;
            for (int i = 0; i < loopSizes.Last(); i++) {
                value *= data[0];
                value = value % 20201227L;
            }
            System.Console.WriteLine(value);
        }

        private  int GetLoop( long crypt)
        {
            var loop = 1;
            var value = 7L;
            while (value != crypt)
            {
                value *= 7L;
                value = value % 20201227L;
                loop++;
            }
            return loop;
        }

        List<long> GetData() => File.ReadAllLines("data.txt").Select(i => long.Parse(i)).ToList();

    }
}

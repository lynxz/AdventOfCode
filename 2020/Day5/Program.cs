using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day5
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

            var max = 0;
            foreach (var l in data)
            {
                var columnIndex = l.IndexOf(l.First(c => c == 'R' || c == 'L'));
                var row = Finder(l, 0, 0, 127);
                var column = Finder(l, columnIndex, 0, 7);

                var seat = row * 8 + column;
                if (seat > max)
                {
                    max = seat;
                }
            }

            System.Console.WriteLine(max);
        }

        

        void SecondStar()
        {
            var data = GetData().ToList();

            var seats = new List<int>();
            foreach (var l in data)
            {
                var columnIndex = l.IndexOf(l.First(c => c == 'R' || c == 'L'));
                var row = Finder(l, 0, 0, 127);
                var column = Finder(l, columnIndex, 0, 7);
                seats.Add(row * 8 + column);
            }
            seats.Sort();
            for (int i = 0; i < seats.Count - 1; i++) {
                if (seats[i] + 1 != seats[i+1]) {
                    System.Console.WriteLine(seats[i] + 1);
                    break;
                }
            }
        }

        int Finder(string d, int pos, int l, int u)
        {
            if (l == u)
            {
                return l;
            }
            var diff = (u + l) / 2;
            switch (d[pos])
            {
                case 'F':
                    return Finder(d, ++pos, l, diff);
                case 'B':
                    return Finder(d, ++pos, diff + 1, u);
                case 'L':
                    return Finder(d, ++pos, l, diff);
                case 'R':
                    return Finder(d, ++pos, diff + 1, u);
            }
            throw new Exception();
        }

        IEnumerable<string> GetData() => File.ReadAllLines("data.txt");

    }
}

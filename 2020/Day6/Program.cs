using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
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
            var result = data.Sum(l => l.Where(c => char.IsLetter(c)).Distinct().Count());

            System.Console.WriteLine(result);
        }

        void SecondStar()
        {
            var data = GetData().ToList();
            var result = data.Sum(l => Enumerable.Range('a', 26).Select(c => l.Count(v => v == c)).Count(i => i == l.Count(c => c == '\n') + 1));

            System.Console.WriteLine(result);
        }

        IEnumerable<string> GetData() => File.ReadAllText("data.txt").Split("\r\n\r\n");
    }
}

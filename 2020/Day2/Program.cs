using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Day2
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
            var regEx = new Regex(@"(\d+)-(\d+)\s(\w):\s(\w*)");

            var result = 0;
            foreach (var l in data)
            {
                var match = regEx.Match(l);
                if (!match.Success)
                    System.Console.WriteLine("Error");

                var min = int.Parse(match.Groups[1].Value);
                var max = int.Parse(match.Groups[2].Value);
                var letter = char.Parse(match.Groups[3].Value);
                var word = match.Groups[4].Value;

                var count = word.Count(c => c == letter);
                if (count >= min && count <= max)
                {
                    result++;
                }
            }
            System.Console.WriteLine(result);
        }

        void SecondStar()
        {
            var data = GetData();
            var regEx = new Regex(@"(\d+)-(\d+)\s(\w):\s(\w*)");

            var result = 0;
            foreach (var l in data)
            {
                var match = regEx.Match(l);
                if (!match.Success)
                    System.Console.WriteLine("Error");

                var min = int.Parse(match.Groups[1].Value);
                var max = int.Parse(match.Groups[2].Value);
                var letter = char.Parse(match.Groups[3].Value);
                var word = match.Groups[4].Value;

                if ((word[min - 1] == letter || word[max - 1] == letter) && (word[min - 1] != word[max - 1]))
                {
                    result++;
                }
            }
            System.Console.WriteLine(result);
        }

        IEnumerable<string> GetData() => File.ReadAllLines("data.txt");
    }
}

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
            //prg.FirstStar();
            prg.SecondStar();
        }

        void FirstStar()
        {
            var data = GetData().ToList();
            var result = 0;
            foreach (var l in data)
            {
                var set = new HashSet<char>();
                foreach (char c in Enumerable.Range('a', 'z' - 'a' + 1))
                {
                    if (l.Contains(c)) 
                        set.Add(c);
                }
                result += set.Count;
            }
            System.Console.WriteLine(result);
        }

        void SecondStar()
        {
            var data = GetData().ToList();
            var result = 0;
            foreach (var l in data)
            {
                var set = new int['z' - 'a' + 1];
                foreach (char c in Enumerable.Range('a', 'z' - 'a' + 1))
                {
                    set[c-'a'] = l.Count(v => v == c);
                }
                result += set.Count(i => i == l.Count(c => c == '\n') + 1);
            }
            System.Console.WriteLine(result);
        }

        IEnumerable<string> GetData() => File.ReadAllText("data.txt").Split("\r\n\r\n");
    }
}

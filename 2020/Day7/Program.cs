using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day7
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
            var dict = new Dictionary<string, List<string>>();
            var regEx = new Regex(@"(\d+)\s(\w+\s\w+)\sbag");
            foreach (var l in data)
            {
                var bag = l.Substring(0, l.IndexOf("bags") - 1);
                dict.Add(bag, new List<string>());
                var subBags = regEx.Matches(l.Split("contain")[1]);
                foreach (Match match in subBags)
                {
                    dict[bag].Add(match.Groups[2].Value);
                }
            }
            var set = new HashSet<string>();
            Selection("shiny gold", dict, set);
            System.Console.WriteLine(set.Count);
        }

        void Selection(string bag, Dictionary<string, List<string>> dict, HashSet<string> set)
        {
            foreach (var key in dict.Keys)
            {
                if (dict[key].Any(v => v == bag)) {
                    set.Add(key);
                    Selection(key, dict, set);
                }
            }
            return;
        }

        void SecondStar()
        {
            var data = GetData().ToList();
            var dict = new Dictionary<string, List<(string Bag,int Count)>>();
            var regEx = new Regex(@"(\d+)\s(\w+\s\w+)\sbag");
            foreach (var l in data)
            {
                var bag = l.Substring(0, l.IndexOf("bags") - 1);
                dict.Add(bag, new List<(string Bag, int Count)>());
                var subBags = regEx.Matches(l.Split("contain")[1]);
                foreach (Match match in subBags)
                {
                    dict[bag].Add((match.Groups[2].Value, int.Parse(match.Groups[1].Value)));
                }
            }
            
            CalcBags("shiny gold", dict);
            System.Console.WriteLine(CalcBags("shiny gold", dict));
        }

        int CalcBags(string bag, Dictionary<string, List<(string Bag,int Count)>> dict) {
            var subBags = dict[bag];
            return subBags.Sum(b => b.Count + b.Count*CalcBags(b.Bag, dict));
        }

        IEnumerable<string> GetData() => File.ReadAllLines("data.txt");
    }
}

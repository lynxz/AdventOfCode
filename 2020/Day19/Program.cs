using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            prog.FirstStar();
            prog.SecondStar();
        }

        private void FirstStar()
        {
            var data = GetData();
            var dict = GenerateRules(data);
            var count = 0;
            for (int i = data.IndexOf(string.Empty) + 1; i < data.Count; i++)
            {
                var results = dict[0](data[i].ToCharArray(), new List<int> { 0 });
                if (results.Any(r => r.Item2 && r.Item1 ==  data[i].Length))
                    count++;
            }
            System.Console.WriteLine(count);
        }

        private Dictionary<int, Func<char[], List<int>, List<(int, bool)>>> GenerateRules(List<string> data)
        {
            Dictionary<int, Func<char[], List<int>, List<(int, bool)>>> dict = new();
            for (int i = 0; i < data.IndexOf(string.Empty); i++)
            {
                var parts = data[i].Split(":");
                var ruleNr = int.Parse(parts[0]);
                var match = Regex.Match(parts[1], "\"(\\w)\"");

                if (match.Success)
                {
                    var value = match.Groups[1].Value[0];
                    dict.Add(ruleNr, (s, pos) => pos.Select(p => p >= s.Length ? (0, false) : (p + 1, s[p] == value)).ToList());
                }
                else
                {
                    if (parts[1].Contains("|"))
                    {
                        var subParts = parts[1].Split("|");
                        var rules1 = ValidateRules(dict, subParts[0]);
                        var rules2 = ValidateRules(dict, subParts[1]);
                        dict.Add(ruleNr, (s, pos) =>
                        {
                            var resultList = new List<(int, bool)>(rules1(s, pos).Where(r => r.Item2));
                            resultList.AddRange(rules2(s, pos).Where(r => r.Item2));
                            return resultList.Count == 0 ? new List<(int, bool)> { (s.Length, false)} : resultList;
                        });
                    }
                    else
                    {
                        dict.Add(ruleNr, ValidateRules(dict, parts[1]));
                    }
                }
            }
            return dict;
        }

        Func<char[], List<int>, List<(int, bool)>> ValidateRules(Dictionary<int, Func<char[], List<int>, List<(int, bool)>>> dict, string stringRule)
        {
            var rules = Regex.Matches(stringRule, @"\d+").Select(m => int.Parse(m.Value)).ToArray();
            return (s, pos) =>
            {
                List<(int, bool)> results = new();
                foreach (var rule in rules)
                {
                    results = dict[rule](s, pos);
                    if (!results.Any(r => r.Item2))
                        return new List<(int, bool)> { (0, false) };
                    pos = results.Where(r => r.Item2).Select(r => r.Item1).ToList();
                }
                return results;
            };
        }

        private void SecondStar()
        {
            var data = GetData();
            var dict = GenerateRules(data);
            dict[8] = (s, pos) => {
                var result = dict[42](s, pos).Where(r => r.Item2).ToList();
                if (result.Count == 0) 
                    return new List<(int, bool)> { (s.Length, false) };
                result.AddRange(dict[8](s, result.Select(r => r.Item1).ToList()));
                return result;
            };
            dict[11] = (s, pos) => {
               var result = dict[42](s, pos).Where(r => r.Item2).ToList();
                if (result.Count == 0) 
                    return new List<(int, bool)> { (s.Length, false) };
                result.AddRange(dict[11](s, result.Where(r => r.Item2).Select(r => r.Item1).ToList()));
                result = dict[31](s, result.Where(r => r.Item2).Select(r => r.Item1).ToList());
                return result;
            };
            var count = 0;
            for (int i = data.IndexOf(string.Empty) + 1; i < data.Count; i++)
            {
                var results = dict[0](data[i].ToCharArray(), new List<int> { 0 });
                if (results.Any(r => r.Item2 && r.Item1 ==  data[i].Length))
                    count++;
            }
            System.Console.WriteLine(count);
        }

        List<string> GetData() => File.ReadAllLines("data.txt").ToList();
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
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
            var (rules, myTicket, otherTickets) = GetData();
            var scanningErrorRate = otherTickets.Sum(t => t.Where(v => !rules.Any(r => r.Valid(v))).Sum());
            System.Console.WriteLine(scanningErrorRate);
        }

        void SecondStar()
        {
            var (rules, myTicket, otherTickets) = GetData();

            // May any deity forgive me for this monstrosity
            List<List<(int index, List<Rule> rules)>> matchings = otherTickets
                .Where(t => !t.Any(v => !rules.Any(r => r.Valid(v))))
                .Select(t => t.Select(v => (t.IndexOf(v), rules.Where(r => r.Valid(v)).ToList())).ToList()).ToList();


            var fields = new List<(int index, Rule rule)>();
            while (rules.Count > 0)
            {
                for (int i = 0; i < otherTickets.Count; i++)
                {
                    var positionMatch = matchings.SelectMany(m => m.Where(v => v.index == i).Select(p => p.rules)).ToList();
                    var rulesmatching = rules.Where(r => positionMatch.All(p => p.Any(pr => pr == r)));
                    if (rulesmatching.Count() == 1)
                    {
                        var rule = rulesmatching.Single();
                        fields.Add((i, rule));
                        rules.Remove(rule);
                    }
                }
            }

            var result = fields.Where(f => f.rule.Name.StartsWith("departure")).Select(f => myTicket[f.index]).Aggregate(1L, (v, acc) => v * acc);
            System.Console.WriteLine(result);
        }

        (List<Rule> Rules, List<int> MyTicket, List<List<int>> OtherTickets) GetData()
        {
            var data = File.ReadAllLines("data.txt").ToList();
            var rules = new List<Rule>();
            var index = 0;
            for (; index < data.Count; index++)
            {
                if (data[index] == string.Empty)
                {
                    break;
                }
                var name = data[index].Split(':')[0];
                var ranges = GetInts(data[index]);
                rules.Add(new Rule(name, new Range(ranges[0], ranges[1]), new Range(ranges[2], ranges[3])));
            }
            index += 2;
            var myTicket = GetInts(data[index]);

            index += 3;
            var otherTickets = new List<List<int>>();
            for (; index < data.Count; index++)
            {
                otherTickets.Add(GetInts(data[index]));
            }
            return (rules, myTicket, otherTickets);
        }

        List<int> GetInts(string line) => Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToList();

    }

    record Rule(string Name, Range FirstRange, Range SecondRange)
    {

        public bool Valid(int value) => FirstRange.InRange(value) || SecondRange.InRange(value);

    }

    record Range(int Start, int End)
    {

        public bool InRange(int value) => value >= Start && value <= End;

    }

}

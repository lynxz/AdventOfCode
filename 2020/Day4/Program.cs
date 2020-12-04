using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
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
            int validCounter = 0;
            foreach (var pass in data) {
                var validationFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
                if (validationFields.All(f => pass.ContainsKey(f)))
                {
                    validCounter++;
                }
            }
            System.Console.WriteLine(validCounter);
        }

        void SecondStar()
        {
            var data = GetData().ToList();
            int validCounter = 0;
            foreach (var pass in data) {
                var validationFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
                if (validationFields.All(f => pass.ContainsKey(f) && Validator(f, pass[f])))
                {
                    validCounter++;
                }
            }
            System.Console.WriteLine(validCounter);
        }

        bool Validator(string field, string value)
        {
            switch (field)
            {
                case "byr":
                    var v = int.Parse(value);
                    return v >= 1920 && v <= 2002;
                case "iyr":
                    var v2 = int.Parse(value);
                    return v2 >= 2010 && v2 <= 2020;
                case "eyr":
                    var v3 = int.Parse(value);
                    return v3 >= 2020 && v3 <= 2030;
                case "hgt":
                    var h = int.Parse(new string(value.Where(c => char.IsNumber(c)).ToArray()));
                    if (value.Contains("in"))
                        return h >= 59 && h <= 76;
                    return h >= 150 && h <= 193;
                case "hcl":
                    return value.Length == 7 && Regex.Match(value, "#[a-f0-9]*").Success;
                case "ecl":
                    return (new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }).Any(c => c == value);
                case "pid":
                    return value.All(c => char.IsNumber(c)) && value.Count(c => char.IsNumber(c)) == 9;
                case "cid":
                    return true;

            }

            throw new Exception();
        }

        IEnumerable<Dictionary<string, string>> GetData()
        {
            var lines = File.ReadAllLines("data.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                var dict = new Dictionary<string, string>();
                while (i < lines.Length && !string.IsNullOrEmpty(lines[i]))
                {
                    var fieldsAndValues = lines[i].Split(" ").Select(d => d.Split(":"));
                    foreach (var kvp in fieldsAndValues)
                    {
                        dict.Add(kvp[0], kvp[1]);
                    }
                    i++;
                }
                yield return dict;
            }
        }

    }
}

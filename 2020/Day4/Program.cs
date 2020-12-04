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
            //prg.FirstStar();
            prg.SecondStar();
        }


        void FirstStar()
        {
            var fields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
            var data = GetData().ToList();
            int validCounter = 0;
            for (int i = 0; i < data.Count; i++)
            {
                var dict = fields.ToDictionary(f => f, f => false);
                while (i < data.Count && !string.IsNullOrEmpty(data[i]))
                {
                    var v = data[i].Split(" ").Select(d => d.Split(":")[0]);
                    foreach (var a in v)
                    {
                        dict[a] = true;
                    }
                    i++;
                }

                var fields2 = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
                if (fields2.All(f => dict[f]))
                {
                    validCounter++;
                }
            }
            System.Console.WriteLine(validCounter);
        }

        void SecondStar()
        {
            var fields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
            var data = GetData().ToList();
            int validCounter = 0;
            for (int i = 0; i < data.Count; i++)
            {
                var dict = fields.ToDictionary(f => f, f => false);
                while (i < data.Count && !string.IsNullOrEmpty(data[i]))
                {
                    var v = data[i].Split(" ").ToDictionary(d => d.Split(":")[0], d => d.Split(":")[1]);
                    foreach (var a in v)
                    {
                        dict[a.Key] = Validator(a.Key, a.Value);
                    }
                    i++;
                }

                var fields2 = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
                if (fields2.All(f => dict[f]))
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
                    return (new [] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}).Any(c => c == value);
                case "pid":
                    return value.All(c =>  char.IsNumber(c)) && value.Count(c => char.IsNumber(c)) == 9;
                case "cid":
                    return true;

            }

            throw new Exception();
        }

        IEnumerable<string> GetData() => File.ReadAllLines("data.txt");

    }
}

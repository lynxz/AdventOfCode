using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21
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
            Dictionary<string, Dictionary<string, int>> dict = new();
            Dictionary<string, int> occurrence = new();
            List<string> allergies = new();
            foreach (var l in data)
            {
                var index = l.IndexOf("(");
                var ingredients = l.Substring(0, index).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var allergens = l.Substring(index + 1, l.Length - index - 2).Replace("contains", "").Split(',').Select(w => w.Trim()).ToArray();

                foreach (var ingredient in ingredients)
                {
                    if (!dict.ContainsKey(ingredient))
                    {
                        dict.Add(ingredient, new Dictionary<string, int>());
                        occurrence.Add(ingredient, 0);
                    }
                    occurrence[ingredient]++;
                    foreach (var allergen in allergens)
                    {
                        if (!dict[ingredient].ContainsKey(allergen))
                        {
                            dict[ingredient].Add(allergen, 0);
                        }
                        dict[ingredient][allergen]++;
                        if (!allergies.Contains(allergen))
                            allergies.Add(allergen);
                    }
                }
            }

            List<(string ingredient, string allergen)> match = new ();
            while (allergies.Count != 0)
            {
                foreach (var allergen in allergies.ToList())
                {
                    var possible = dict.Where(kvp => kvp.Value.ContainsKey(allergen)).Select(kvp => (ingredient: kvp.Key, count: kvp.Value[allergen]));
                    var max = possible.Max(p => p.count);
                    var maxValues = possible.Where(p => p.count == max).ToList();
                    if (maxValues.Count == 1)
                    {
                        var ingredient = maxValues.Single().ingredient;
                        dict.Remove(ingredient);
                        allergies.Remove(allergen);
                        match.Add((ingredient, allergen));
                    }
                }
            }

            System.Console.WriteLine(dict.Keys.Sum(i => occurrence[i])); 
            System.Console.WriteLine(string.Join(",", match.OrderBy(m => m.allergen).Select(m => m.ingredient))); 
        }

        List<string> GetData() => File.ReadAllLines("data.txt").ToList();

    }
}

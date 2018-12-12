using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        public void FirstStar()
        {
            var pots = GetPots(400);
            var map = GetMap();

            for (int i = 0; i < 20; i++)
            {
                var newPots = Enumerable.Repeat('.', pots.Length).ToArray();
                for (int cp = 2; cp < pots.Length - 2; cp++)
                {
                    var pattern = new string(Enumerable.Range(-2, 5).Select(offset => pots[cp + offset]).ToArray());
                    newPots[cp] = map.ContainsKey(pattern) ? map[pattern] : '.';
                }
                pots = newPots;
            }
            System.Console.WriteLine(Enumerable.Range(-400, pots.Length).Zip(pots, (i, p) => p == '#' ? i : 0).Sum());
        }

        public void SecondStar()
        {
            var pots = GetPots(400);
            var map = GetMap();
            var value = 0;
            for (long i = 0; i < 100; i++)
            {
                var newPots = Enumerable.Repeat('.', pots.Length).ToArray();
                for (int cp = 2; cp < pots.Length - 2; cp++)
                {
                    var pattern = new string(Enumerable.Range(-2, 5).Select(offset => pots[cp + offset]).ToArray());
                    newPots[cp] = map.ContainsKey(pattern) ? map[pattern] : '.';
                }
                pots = newPots;

                value = Enumerable.Range(-400, pots.Length).Zip(pots, (j, p) => p == '#' ? j : 0).Sum();
            }
            System.Console.WriteLine((50000000000 - 100)* 50 + value );
        }

        char[] GetPots(int start)
        {
            var data = File.ReadAllLines("input");
            var initState = data[0].Substring(15);
            var result = Enumerable.Repeat('.', 2 * start + initState.Length).ToArray();
            for (int i = 0; i < initState.Length; i++)
            {
                result[i + start] = initState[i];
            }
            return result;
        }

        Dictionary<string, char> GetMap()
        {
            var data = File.ReadAllLines("input");
            var map = new Dictionary<string, char>();
            for (int i = 2; i < data.Length; i++)
            {
                var parts = data[i].Split(" => ");
                map.Add(parts[0], parts[1][0]);
            }
            return map;
        }
    }
}

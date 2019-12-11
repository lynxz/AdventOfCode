using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        void FirstStar()
        {

            var coords = GetCoords().ToList();
            var score = coords.ToDictionary(c => c, c => 0);
            foreach (var coord1 in coords)
            {
                foreach (var coord2 in coords)
                {
                    if (coord1 != coord2)
                    {
                        var between = CoordsInBetween(coord1, coord2);
                        if (coords.All(c => !between.Any(b => b == c)))
                        {
                            score[coord1]++;
                        }
                    }
                }
            }

            var max = 0;
            var maxKvp = new KeyValuePair<(int, int), int>();
            foreach (var kvp in score)
            {
                if (kvp.Value > max)
                {
                    max = kvp.Value;
                    maxKvp = kvp;
                }
            }
            System.Console.WriteLine($"({maxKvp.Key.Item1}, {maxKvp.Key.Item2}) {maxKvp.Value}");
        }

        void SecondStar()
        {
            var data = GetData().ToList();
            var home = (31, 20);
            var frame = new List<(int, int)>();
            var coords = GetCoords().ToList();
            var queues = coords
                .Where(c => c != home)
                .Select(c =>
                {
                    var xDist = c.Item1 - home.Item1;
                    var yDist = c.Item2 - home.Item2;
                    var angle = Math.Atan2(xDist, yDist);
                    return (c.Item1, c.Item2, angle, dist: Math.Sqrt(xDist * xDist + yDist * yDist));
                })
                .ToLookup(a => a.angle)
                .OrderByDescending(a => a.Key)
                .Select(a => new Queue<(int x, int y, double angle, double dist)>(a.OrderBy(b => b.dist)))
                .ToList();

            int i = 0;
            while (i < 200)
            {
                foreach (var queue in queues)
                {
                    if (queue.Count > 0)
                    {
                        var hit = queues[i].Dequeue();
                        i++;
                        System.Console.WriteLine($"The {i}th asteroid to be vaporized is at {hit.x},{hit.y}.");
                        if (i == 200)
                        {
                            System.Console.WriteLine(hit.x * 100 + hit.y);
                        }
                    }
                }
            }
        }

        List<(int, int)> CoordsInBetween((int, int) a, (int, int) b)
        {
            if (a.Item1 == b.Item1)
            {
                var d = a.Item2 > b.Item2 ? -1 : 1;
                return Enumerable.Range(1, Math.Abs(a.Item2 - b.Item2) - 1).Select(i => (a.Item1, a.Item2 + i * d))
                    .Where(c => c.Item1 >= 0 && c.Item1 < 40 && c.Item2 >= 0 && c.Item2 < 40)
                    .ToList();
            }
            if (a.Item2 == b.Item2)
            {
                var d = a.Item1 > b.Item1 ? -1 : 1;
                return Enumerable.Range(1, Math.Abs(a.Item1 - b.Item1) - 1).Select(i => (a.Item1 + i * d, a.Item2))
                    .Where(c => c.Item1 >= 0 && c.Item1 < 40 && c.Item2 >= 0 && c.Item2 < 40)
                    .ToList();
            }
            if (Math.Abs(a.Item1 - b.Item1) == Math.Abs(a.Item2 - b.Item2))
            {
                var dx = a.Item1 > b.Item1 ? -1 : 1;
                var dy = a.Item2 > b.Item2 ? -1 : 1;
                return Enumerable.Range(1, Math.Abs(a.Item1 - b.Item1) - 1).Select(i => (a.Item1 + i * dx, a.Item2 + i * dy))
                    .Where(c => c.Item1 >= 0 && c.Item1 < 40 && c.Item2 >= 0 && c.Item2 < 40)
                    .ToList();
            }
            var gcd = GCD(Math.Abs(a.Item2 - b.Item2), Math.Abs(a.Item1 - b.Item1));
            var dix = (b.Item1 - a.Item1) / gcd;
            var diy = (b.Item2 - a.Item2) / gcd;
            return Enumerable.Range(1, gcd - 1).Select(i => (a.Item1 + i * dix, a.Item2 + i * diy))
                .Where(c => c.Item1 >= 0 && c.Item1 < 40 && c.Item2 >= 0 && c.Item2 < 40)
                .ToList();
        }

        int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }
            return a == 0 ? b : a;
        }

        IEnumerable<(int, int)> GetCoords()
        {
            var data = GetData().ToList();
            for (int y = 0; y < data.Count; y++)
            {
                for (int x = 0; x < data[y].Length; x++)
                {
                    if (data[y][x] == '#')
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        IEnumerable<string> GetData()
            => File.ReadAllLines("input.txt");

    }
}

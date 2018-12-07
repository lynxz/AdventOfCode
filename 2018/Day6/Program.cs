using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day6
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
            var c = GetData();
            var maxX = c.Max(v => v.Item1) + 1;
            var maxY = c.Max(v => v.Item2) + 1;
            var grid = Enumerable.Repeat(int.MaxValue, maxX * maxY).ToArray();
            var invalids = new HashSet<int>();
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    var closest = int.MaxValue;
                    for (int index = 0; index < c.Length; index++)
                    {
                        var dist = CalculateDistance(c[index], (i, j));
                        if (dist < closest)
                        {
                            closest = dist;
                            grid[maxX * j + i] = index;
                        }
                    }
                    if (i == 0 || j == 0 || i == maxX - 1 || j == maxY - 1)
                    {
                        if (!invalids.Contains(grid[maxX * j + i]))
                        {
                            invalids.Add(grid[maxX * j + i]);
                        }
                    }
                }
            }

            var sizes = new Dictionary<int, int>();
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    if (!invalids.Contains(grid[maxX * j + i]))
                    {
                        if (!sizes.ContainsKey(grid[maxX * j + i]))
                        {
                            sizes.Add(grid[maxX * j + i], 0);
                        }
                        sizes[grid[maxX * j + i]]++;
                    }
                }
            }

            System.Console.WriteLine(sizes.Max(s => s.Value));
        }

        static int CalculateDistance((int, int) c1, (int, int) c2)
        {
            var result = Math.Abs(c1.Item1 - c2.Item1) + Math.Abs(c1.Item2 - c2.Item2);
            return result >= 0 ? result : int.MaxValue;
        }



        public void SecondStar()
        {
            var c = GetData();
            var maxX = c.Max(v => v.Item1) + 1;
            var maxY = c.Max(v => v.Item2) + 1;
            var grid = Enumerable.Repeat('.', maxX * maxY).ToArray();
            var area = 0;
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    var distSum = c.Sum(p => CalculateDistance(p, (i, j)));
                    if (distSum < 10000)
                    {
                        area++;
                        grid[j * maxX + i] = '#';
                    }
                }
            }
            var sb = new StringBuilder();
            for (int i = 0; i < maxY; i++)
            {
                sb.AppendLine(string.Join("", grid.Skip(i * maxX).Take(maxX)));
            }
            File.WriteAllText(@"C:\Temp\result.txt", sb.ToString());
            System.Console.WriteLine(area);
        }

        (int, int)[] GetData() => File.ReadAllLines("input")
            .Select(l => (int.Parse(l.Split(',')[0]), int.Parse(l.Split(',')[1])))
            .ToArray();

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day17
{

    public enum Element { Sand, Water, Clay, Flow, Void }

    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        public bool Flow(char[,] map, int x, int y, int dir)
        {
            if (y < 1 || y >= map.GetLength(0) || x < 0 || x >= map.GetLength(1))
            {
                return true;
            }

            if (map[y, x] != '.')
            {
                return map[y, x] != '#' && map[y, x] != '~';
            }

            map[y, x] = '|';

            var leaky = Flow(map, x, y + 1, 0);
            if (leaky)
            {
                return true;
            }

            leaky = (dir <= 0) && Flow(map, x - 1, y, -1);
            leaky |= (dir >= 0) && Flow(map, x + 1, y, 1);
            if (leaky)
            {
                return true;
            }

            if (dir == 0)
            {
                Fill(map, x, y, -1);
                Fill(map, x + 1, y, 1);
            }

            return false;
        }


        void Fill(char[,] map, int x, int y, int dir)
        {
            if (y < 1 || y >= map.GetLength(0) || x < 0 || x >= map.GetLength(1))
            {
                return;
            }
            if (map[y, x] == '|')
            {
                Fill(map, x + dir, y, dir);
                map[y, x] = '~';
            }
        }

        public void FirstStar()
        {
            var map = GetMap();
            try
            {
                Flow(map, 500 - 460, 1, 0);
            }
            finally
            {
                PrintMap(map);
            }
            var sum = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == '~' || map[y, x] == '|')
                    {
                        sum++;
                    }
                }
            }
            System.Console.WriteLine(sum);
        }

        public void SecondStar()
        {
            var map = GetMap();
            Flow(map, 500 - 460, 1, 0);
            var sum = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == '~')
                    {
                        sum++;
                    }
                }
            }
            System.Console.WriteLine(sum);
        }

        public static void PrintMap(char[,] map)
        {
            using (var fw = new StreamWriter("output"))
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    var sb = new StringBuilder();
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        sb.Append(map[y, x]);
                    }
                    fw.WriteLine(sb.ToString());
                }
            }
        }

        private static char[,] GetMap()
        {
            var xOffset = 460;
            var yOffset = 5;
            var map = new char[1896 - yOffset, 700 - xOffset];
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    map[y, x] = '.';
                }
            }
            foreach (var line in File.ReadAllLines("input"))
            {
                var m = Regex.Match(line, @"(\w)=(\d+), \w=(\d+)..(\d+)");
                var pos = int.Parse(m.Groups[2].Value);
                var start = int.Parse(m.Groups[3].Value);
                var end = int.Parse(m.Groups[4].Value);
                if (m.Groups[1].Value == "x")
                {
                    for (int i = start; i <= end; i++)
                    {
                        map[i - yOffset, pos - xOffset] = '#';
                    }
                }
                else
                {
                    for (int i = start; i <= end; i++)
                    {
                        map[pos - yOffset, i - xOffset] = '#';
                    }
                }
            }

            return map;
        }
    }

}

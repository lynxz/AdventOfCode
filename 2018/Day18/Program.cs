using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.FirstStar();
        }

        public void SecondStar()
        {
            var map = GetData();
            var memoization = new Dictionary<int, int>();
            for (int i = 0; i < 1000_000_000; i++)
            {
                var newMap = new char[map.GetLength(0), map.GetLength(1)];
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        var surround = GetSurronding(map, y, x);
                        switch (map[y, x])
                        {
                            case '.':
                                newMap[y, x] = surround.Count(c => c == '|') >= 3 ? '|' : '.';
                                break;
                            case '|':
                                newMap[y, x] = surround.Count(c => c == '#') >= 3 ? '#' : '|';
                                break;
                            case '#':
                                newMap[y, x] = surround.Count(c => c == '#') >= 1 &&
                                            surround.Count(c => c == '|') >= 1 ? '#' : '.';
                                break;
                        }
                    }
                }
                map = newMap;
                var sb = new StringBuilder();
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        sb.Append(map[y,x]);
                    }
                }
                var hash = sb.ToString().GetHashCode();
                if (!memoization.ContainsKey(hash)) {
                    memoization.Add(hash, i);
                }
                else {
                    var last = memoization[hash];
                    System.Console.WriteLine($"Last: {last}, Current: {i}");
                    var diff = i - last;
                    System.Console.WriteLine((1000_000_000 - 476) % diff);
                    return;
                }
            }
        }

        public void FirstStar()
        {
            var map = GetData();
            PrintMap(map);
            for (int i = 0; i < 496; i++)
            {
                var newMap = new char[map.GetLength(0), map.GetLength(1)];
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        var surround = GetSurronding(map, y, x);
                        switch (map[y, x])
                        {
                            case '.':
                                newMap[y, x] = surround.Count(c => c == '|') >= 3 ? '|' : '.';
                                break;
                            case '|':
                                newMap[y, x] = surround.Count(c => c == '#') >= 3 ? '#' : '|';
                                break;
                            case '#':
                                newMap[y, x] = surround.Count(c => c == '#') >= 1 &&
                                            surround.Count(c => c == '|') >= 1 ? '#' : '.';
                                break;
                        }
                    }
                }
                map = newMap;
                PrintMap(map);
            }
            var wood = 0;
            var lumberyard = 0;

            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == '|') wood++;
                    else if (map[y, x] == '#') lumberyard++;
                }
            }
            System.Console.WriteLine(wood * lumberyard);
        }

        void PrintMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    sb.Append(map[y, x]);
                }
                System.Console.WriteLine(sb.ToString());
            }
            System.Console.WriteLine();
        }

        IEnumerable<char> GetSurronding(char[,] map, int y, int x)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!(j == 0 && i == 0))
                    {
                        if (ValidateCoords(map, y + i, x + j))
                        {
                            yield return map[y + i, x + j];
                        }
                    }
                }
            }
        }

        bool ValidateCoords(char[,] map, int y, int x) => y >= 0 && x >= 0 && y < map.GetLength(0) && x < map.GetLength(1);

        char[,] GetData()
        {
            var data = File.ReadAllLines("input");
            var result = new char[data.Length, data.First().Length];
            for (int y = 0; y < data.Length; y++)
            {
                for (int x = 0; x < data[y].Length; x++)
                {
                    result[y, x] = data[y][x];
                }
            }

            return result;
        }

    }
}

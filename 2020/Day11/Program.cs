using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            prog.FirstStar();
            prog.SecondStar();
        }

        public void FirstStar()
        {
            var data = GetData();

            var same = false;
            while (!same)
            {
                var newData = new List<char[]>();
                for (int y = 0; y < data.Count; y++)
                {
                    newData.Add(new char[data[y].Length]);
                    for (int x = 0; x < data[y].Length; x++)
                    {
                        if (data[y][x] == 'L')
                            newData[y][x] = NumberOfAdajcentOccupied(data, x, y) == 0 ? '#' : 'L';
                        else if (data[y][x] == '#')
                            newData[y][x] = NumberOfAdajcentOccupied(data, x, y) >= 4 ? 'L' : '#';
                        else
                            newData[y][x] = '.';

                        if (x == 0 && y == 0)
                            same = newData[y][x] == data[y][x];
                        else
                            same = same && newData[y][x] == data[y][x];
                    }
                }
                data = newData;
            }
            var result = data.Sum(l => l.Count(c => c == '#'));
            System.Console.WriteLine(result);
        }

        public void SecondStar()
        {
            var data = GetData();

            var lookup = GetLookup(data);

            var same = false;
            while (!same)
            {
                var newData = new List<char[]>();
                for (int y = 0; y < data.Count; y++)
                {
                    newData.Add(new char[data[y].Length]);
                    for (int x = 0; x < data[y].Length; x++)
                    {
                        if (data[y][x] == 'L')
                            newData[y][x] = lookup[(x,y)].Count(c => data[c.y][c.x] == '#') == 0 ? '#' : 'L';
                        else if (data[y][x] == '#')
                            newData[y][x] = lookup[(x,y)].Count(c => data[c.y][c.x] == '#') >= 5 ? 'L' : '#';
                        else
                            newData[y][x] = '.';

                        if (x == 0 && y == 0)
                            same = newData[y][x] == data[y][x];
                        else
                            same = same && newData[y][x] == data[y][x];
                    }
                }
                data = newData;
            }
            var result = data.Sum(l => l.Count(c => c == '#'));
            System.Console.WriteLine(result);
        }

        int NumberOfAdajcentOccupied(List<char[]> seats, int x, int y)
        {
            var occupied = 0;
            if (x > 0)
            {
                if (y > 0)
                {
                    occupied += seats[y - 1][x - 1] == '#' ? 1 : 0;
                    occupied += seats[y - 1][x] == '#' ? 1 : 0;
                }
                if (y < seats.Count - 1)
                {
                    occupied += seats[y + 1][x - 1] == '#' ? 1 : 0;
                    occupied += seats[y + 1][x] == '#' ? 1 : 0;
                }
                occupied += seats[y][x - 1] == '#' ? 1 : 0;
            }
            if (x < seats[y].Length - 1)
            {
                if (y > 0)
                {
                    occupied += seats[y - 1][x + 1] == '#' ? 1 : 0;
                    if (x == 0)
                        occupied += seats[y - 1][x] == '#' ? 1 : 0;
                }
                if (y < seats.Count - 1)
                {
                    occupied += seats[y + 1][x + 1] == '#' ? 1 : 0;
                    if (x == 0)
                        occupied += seats[y + 1][x] == '#' ? 1 : 0;
                }
                occupied += seats[y][x + 1] == '#' ? 1 : 0;
            }

            return occupied;
        }

        Dictionary<(int x, int y), List<(int x, int y)>> GetLookup(List<char[]> seats)
        {
            var coords = GetCoords(seats).ToList();
            var lookup = new Dictionary<(int, int), List<(int, int)>>();
            for (int y = 0; y < seats.Count; y++)
            {
                for (int x = 0; x < seats[y].Length; x++)
                {
                    if (seats[y][x] == '.')
                        continue;

                    var home = (x, y);
                    
                    var lists = coords
                        .Where(c => c != home)
                        .Select(c =>
                        {
                            var xDist = c.Item1 - home.Item1;
                            var yDist = c.Item2 - home.Item2;
                            var angle = Math.Atan2(xDist, yDist);
                            return (c.Item1, c.Item2, angle, dist: Math.Sqrt(xDist * xDist + yDist * yDist));
                        })
                        .Where(a =>
                            Math.Abs(a.angle) < 0.001 ||
                            Math.Abs(Math.PI - a.angle) < 0.001 ||
                            Math.Abs(Math.PI / 2 - a.angle) < 0.001 ||
                            Math.Abs(Math.PI / 2 + a.angle) < 0.001 ||
                            Math.Abs(Math.PI / 4 - a.angle) < 0.001 ||
                            Math.Abs(Math.PI / 4 + a.angle) < 0.001 ||
                            Math.Abs((3 * Math.PI) / 4 - a.angle) < 0.001 ||
                            Math.Abs((3 * Math.PI) / 4 + a.angle) < 0.001 
                            )
                        .ToLookup(a => a.angle)
                        .OrderByDescending(a => a.Key)
                        .Select(a => new List<(int x, int y, double angle, double dist)>(a.OrderBy(b => b.dist)))
                        .ToList();

                    lookup.Add(home, lists.Where(l => l.Count > 0).Select(l => (l[0].x, l[0].y)).ToList());
                }
            }

            return lookup;
        }

        IEnumerable<(int, int)> GetCoords(List<char[]> data)
        {
            for (int y = 0; y < data.Count; y++)
            {
                for (int x = 0; x < data[y].Length; x++)
                {
                    if (data[y][x] == 'L' || data[y][x] == '#')
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        List<char[]> GetData() => File.ReadAllLines("data.txt").Select(s => s.ToCharArray()).ToList();
    }
}

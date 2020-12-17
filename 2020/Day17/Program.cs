using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            //prog.FirstStar();
            prog.SecondStar();
        }

        private void FirstStar()
        {
            var data = GetData();
            var side = 20;

            var grid = new Dictionary<Coord, bool>();

            for (int y = -side; y < side; y++)
                for (int x = -side; x < side; x++)
                    for (int z = -side; z < side; z++)
                        grid.Add(new Coord(x, y, z), false);

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    var coord = new Coord(j, i, 0);
                    grid[coord] = data[i][j] == '#';
                }
            }

            Print(grid, 0);

            for (int i = 0; i < 6; i++)
            {
                var newGrid = new Dictionary<Coord, bool>();
                foreach (var coord in grid.Keys)
                {
                    var n = ActiveNeighbors(grid, coord);
                    if (grid[coord])
                    {
                        newGrid[coord] = n >= 2 && n <= 3;
                    }
                    else
                    {
                        newGrid[coord] = n == 3;
                    }
                }
                grid = newGrid;
                Print(grid, 0);
            }

            System.Console.WriteLine(grid.Values.Count(v => v));

        }

        void Print(Dictionary<Coord, bool> grid, int z, int w = 0)
        {
            for (int y = -10; y < 10; y++)
            {
                for (int x = -10; x < 10; x++)
                {
                    var coord = new Coord(x, y, z, w);
                    System.Console.Write(grid[coord] ? "#" : ".");
                }
                System.Console.WriteLine();
            }
        }


        int ActiveNeighbors(Dictionary<Coord, bool> grid, Coord coord)
        {
            var counter = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        if (!(x == 0 && y == 0 && z == 0))
                        {
                            var pos = new Coord(coord.X + x, coord.Y + y, coord.Z + z);
                            if (grid.ContainsKey(pos) && grid[pos])
                                counter += grid[pos] ? 1 : 0;
                        }
                    }
                }
            }
            return counter;
        }

        int ActiveNeighbors2(Dictionary<Coord, bool> grid, Coord coord)
        {
            var counter = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        for (int w = -1; w < 2; w++)
                            if (!(x == 0 && y == 0 && z == 0 && w == 0))
                            {
                                var pos = new Coord(coord.X + x, coord.Y + y, coord.Z + z, coord.W + w);
                                if (grid.ContainsKey(pos) && grid[pos])
                                    counter += grid[pos] ? 1 : 0;
                            }
                    }
                }
            }
            return counter;
        }



        private void SecondStar()
        {
            var data = GetData();
            var side = 15;

            var grid = new Dictionary<Coord, bool>();

            for (int y = -side; y < side; y++)
                for (int x = -side; x < side; x++)
                    for (int z = -side; z < side; z++)
                        for (int w = -side; w < side; w++)
                            grid.Add(new Coord(x, y, z, w), false);

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    var coord = new Coord(j, i, 0);
                    grid[coord] = data[i][j] == '#';
                }
            }

            //Print(grid, 0);

            for (int i = 0; i < 6; i++)
            {
                var newGrid = new Dictionary<Coord, bool>();
                foreach (var coord in grid.Keys)
                {
                    var n = ActiveNeighbors2(grid, coord);
                    if (grid[coord])
                    {
                        newGrid[coord] = n >= 2 && n <= 3;
                    }
                    else
                    {
                        newGrid[coord] = n == 3;
                    }
                }
                grid = newGrid;
                //Print(grid, 0);
                System.Console.WriteLine(".");
            }
            System.Console.WriteLine(grid.Values.Count(v => v));
        }

        List<string> GetData() => File.ReadAllLines("data.txt").ToList();

    }

    public struct Coord
    {

        public Coord(int x, int y, int z, int w = 0)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public int W { get; }

        public override int GetHashCode()
        {
            return (W * 40 * 40 * 40) + (Y * 40 * 40) + (X * 40) + Z;
        }

    }
}

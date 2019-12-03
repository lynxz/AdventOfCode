using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
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
            var firstWire = GetData().First().Split(',');
            var secondWire = GetData().Last().Split(',');
            var firstWireVec = GetVectors(firstWire).ToList();
            var secondWireVec = GetVectors(secondWire).ToList();

            var closest = int.MaxValue;
            int i = 0;
            foreach (var wire1 in firstWireVec)
            {
                System.Console.WriteLine($"{i} of {firstWireVec.Count()}");
                foreach (var wire2 in secondWireVec)
                {
                    foreach (var coord1 in wire1)
                    {
                        foreach (var coord2 in wire2)
                        {
                            if (coord1.Item1 == 0 && coord1.Item2 == 0)
                            {
                                continue;
                            }
                            if (coord1.Item1 == coord2.Item1 && coord1.Item2 == coord2.Item2)
                            {
                                if (Math.Abs(coord1.Item1) + Math.Abs(coord1.Item2) < closest)
                                {
                                    closest = Math.Abs(coord1.Item1) + Math.Abs(coord1.Item2);
                                }
                            }
                        }
                    }
                }
                i++;
            }
            System.Console.WriteLine(closest);
        }

        IEnumerable<IEnumerable<(int, int)>> GetVectors(IEnumerable<string> moves)
        {
            var x = 0;
            var y = 0;
            foreach (var move in moves)
            {
                var direction = move.First();
                var steps = int.Parse(new String(move.Skip(1).ToArray()));
                IEnumerable<(int, int)> coords = null;
                switch (direction)
                {
                    case 'R':
                        coords = Enumerable.Range(0, steps).Select(i => (x + i, y)).ToList();
                        x += steps;
                        break;
                    case 'U':
                        coords = Enumerable.Range(0, steps).Select(i => (x, y + i)).ToList();
                        y += steps;
                        break;
                    case 'L':
                        coords = Enumerable.Range(0, steps).Select(i => (x - i, y)).ToList();
                        x -= steps;
                        break;
                    case 'D':
                        coords = Enumerable.Range(0, steps).Select(i => (x, y - i)).ToList();
                        y -= steps;
                        break;
                }
                yield return coords;
            }
        }

        void SecondStar()
        {
            var firstWire = GetData().First().Split(',');
            var secondWire = GetData().Last().Split(',');
            var firstWireVec = GetVectors(firstWire).SelectMany(x => x).ToList();
            var secondWireVec = GetVectors(secondWire).SelectMany(x => x).ToList();

            var leastSteps = int.MaxValue;
            for (int i = 1; i < firstWireVec.Count; i++)
            {
                System.Console.WriteLine($"{i} of {firstWireVec.Count}");
                for (int j = 1; j < secondWireVec.Count; j++)
                {
                    if (firstWireVec[i].Item1 == secondWireVec[j].Item1 && firstWireVec[i].Item2 == secondWireVec[j].Item2)
                    {
                        if (i + j < leastSteps)
                        {
                            leastSteps = i + j;
                        }
                    }
                }
            }
            System.Console.WriteLine(leastSteps);
        }


        IEnumerable<string> GetData()
            => File.ReadAllLines("input.txt");

    }
}

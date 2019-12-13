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
            var moons = new int[4, 6]  {
                {-3, 15, -11, 0, 0 ,0},
                {3, 13, -19, 0, 0 ,0},
                {-13, 18, -2, 0, 0 ,0},
                {6, 0, -1, 0, 0 ,0},
            };

            for (int r = 0; r < 1000; r++)
            {
                MoveMoons(moons);
            }

            var energy = 0;
            for (int i = 0; i < 4; i++)
            {
                var pot = Enumerable.Range(0, 3).Sum(j => Math.Abs(moons[i, j]));
                var kin = Enumerable.Range(3, 3).Sum(j => Math.Abs(moons[i, j]));
                energy += pot * kin;
            }
            System.Console.WriteLine(energy);
        }

        private static void MoveMoons(int[,] moons)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (moons[i, k] < moons[j, k])
                        {
                            moons[i, k + 3]++;
                            moons[j, k + 3]--;
                        }
                        else if (moons[i, k] > moons[j, k])
                        {
                            moons[j, k + 3]++;
                            moons[i, k + 3]--;
                        }
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    moons[i, j] += moons[i, j + 3];
                }
            }
        }

        public void SecondStar()
        {

            var xStart = new [] {-3, 3, -13, 6};
            var yStart = new [] {15, 13, 18, 0};
            var zStart = new [] {-11, -19, -2, -1};

            var moons = new int[4, 6]  {
                {-3, 15, -11, 0, 0 ,0},
                {3, 13, -19, 0, 0 ,0},
                {-13, 18, -2, 0, 0 ,0},
                {6, 0, -1, 0, 0 ,0},
            };

            var xCoordinates = new Dictionary<(int, int, int, int), List<int>>();
            var yCoordinates = new Dictionary<(int, int, int, int), List<int>>();
            var zCoordinates = new Dictionary<(int, int, int, int), List<int>>();
            for (int r = 0; r < 1000000; r++)
            {
                var xs = (moons[0,0], moons[1,0], moons[2,0],moons[3,0]); 
                if (!xCoordinates.ContainsKey(xs)) {
                    xCoordinates.Add(xs, new List<int>());
                } 
                xCoordinates[xs].Add(r);

                var ys = (moons[0,1], moons[1,1], moons[2,1],moons[3,1]); 
                if (!yCoordinates.ContainsKey(ys)) {
                    yCoordinates.Add(ys, new List<int>());
                } 
                yCoordinates[ys].Add(r);

                var zs = (moons[0,2], moons[1,2], moons[2,2],moons[3,2]); 
                if (!zCoordinates.ContainsKey(zs)) {
                    zCoordinates.Add(zs, new List<int>());  
                } 
                zCoordinates[zs].Add(r);
                
                MoveMoons(moons);
            }

            var xInterval = xCoordinates.Where(c => c.Value.Count > 4);
            var yInterval = yCoordinates.Where(c => c.Value.Count > 4);
            var zInterval = zCoordinates.Where(c => c.Value.Count > 4);
            foreach(var interval in yInterval.Take(40)) {
                System.Console.WriteLine($"coord: {interval.Key} First: {interval.Value[0]} Int1: {interval.Value[2]-interval.Value[0]} Int2: {interval.Value[4]-interval.Value[2]} {string.Join(",", interval.Value)}");
            }
            //ymin: 231614
            //xmin: 286332 
            //zmin: 60424
            // got lcm form wolfram-alpha 500903629351944
            System.Console.WriteLine(500903629351944);
        }

        private static void PrintMoons(int[,] moons)
        {
            for (int i = 0; i < 4; i++)
            {
                System.Console.WriteLine(String.Format("pos=<x={0,3}, y={1,3}, z={2,3}>, vel=<x={3,3}, y={4,3}, z={5,3}>", moons[i, 0], moons[i, 1], moons[i, 2], moons[i, 3], moons[i, 4], moons[i, 5]));
            }
        }

        IEnumerable<string> GetData() =>
            File.ReadAllLines("input.txt");

    }
}

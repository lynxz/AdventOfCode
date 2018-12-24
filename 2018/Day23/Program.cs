using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
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
            var bots = GetBots().ToList();
            var max = int.MinValue;
            Bot strongestBot = null;
            foreach (var bot in bots)
            {
                if (bot.Signal > max)
                {
                    max = bot.Signal;
                    strongestBot = bot;
                }
            }
            int botsInRange = 0;
            foreach (var bot in bots)
            {
                var dist = Math.Abs(bot.X - strongestBot.X) + Math.Abs(bot.Y - strongestBot.Y) + Math.Abs(bot.Z - strongestBot.Z);
                if (dist <= strongestBot.Signal)
                {
                    botsInRange++;
                }
            }
            System.Console.WriteLine(botsInRange);
        }

        public void SecondStar()
        {
            var bots = GetBots().ToList();
            var xMin = bots.Min(b => b.X);
            var xMax = bots.Max(b => b.X);
            var yMin = bots.Min(b => b.Y);
            var yMax = bots.Max(b => b.Y);
            var zMin = bots.Min(b => b.Z);
            var zMax = bots.Max(b => b.Z);

            var minDist = xMax - xMin;
            var dist = 1;
            while (dist < minDist)
            {
                dist *= 2;
            }

            while (true)
            {
                var target_count = 0;
                var bestVal = 0;
                var best = default((int x, int y, int z));
                foreach (var x in Enumerable.Range(0, ((xMax - xMin) / dist) + 1).Select(i => xMin + i * dist))
                {
                    foreach (var y in Enumerable.Range(0, ((yMax - yMin) / dist) + 1).Select(i => yMin + i * dist))
                    {
                        foreach (var z in Enumerable.Range(0, ((zMax - zMin) / dist) + 1).Select(i => zMin + i * dist))
                        {
                            var count = 0;
                            foreach (var bot in bots)
                            {
                                var calc = Math.Abs(x - bot.X) + Math.Abs(y - bot.Y) + Math.Abs(z - bot.Z);
                                if ((calc - bot.Signal) / dist <= 0)
                                {
                                    count++;
                                }
                            }
                            if (count > target_count)
                            {
                                target_count = count;
                                bestVal = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
                                best = (x, y, z);
                            }
                            else if (count == target_count)
                            {
                                if (Math.Abs(x) + Math.Abs(y) + Math.Abs(z) < bestVal)
                                {
                                    bestVal = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
                                    best = (x, y, z);
                                }
                            }
                        }
                    }
                }
                if (dist == 1)
                {
                    System.Console.WriteLine(bestVal);
                    return;
                }
                else
                {
                    xMin = best.x - dist;
                    xMax = best.x + dist;
                    yMin = best.y - dist;
                    yMax = best.y + dist;
                    zMin = best.z - dist;
                    zMax = best.z + dist;
                    dist /= 2;
                }
            }

        }

        IEnumerable<Bot> GetBots()
        {
            var data = File.ReadAllLines("input");
            foreach (var line in data)
            {
                var coords = line.Substring(5, line.LastIndexOf(',') - 6).Split(',').Select(i => int.Parse(i)).ToArray();
                var signal = int.Parse(line.Substring(line.LastIndexOf(',') + 4));
                yield return new Bot
                {
                    X = coords[0],
                    Y = coords[1],
                    Z = coords[2],
                    Signal = signal
                };
            }
        }

    }


    class Bot
    {

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public int Signal { get; set; }

        public override string ToString()
        {
            return $"pos=<{X},{Y},{Z}>, r={Signal}";
        }

    }
}

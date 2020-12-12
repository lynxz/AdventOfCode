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
            var prg = new Program();
            prg.FirstStar();
            prg.SecondStar();
        }

        public void FirstStar()
        {
            var data = GetData().Select(s => (s.Substring(0, 1), int.Parse(s.Substring(1))));
            var x = 0;
            var y = 0;
            var direction = "E";
            foreach (var d in data)
            {
                var result = Move(d.Item1, d.Item2, direction);
                x += result.x;
                y += result.y;
                direction = result.heading;
            }

            System.Console.WriteLine(Math.Abs(x) + Math.Abs(y));
        }

        (int x, int y, string heading) Move(string direction, int distance, string heading)
        {
            var headings = new[] { "E", "N", "W", "S" };
            var x = 0;
            var y = 0;
            switch (direction)
            {
                case "N":
                    y += distance;
                    break;
                case "S":
                    y -= distance;
                    break;
                case "W":
                    x -= distance;
                    break;
                case "E":
                    x += distance;
                    break;
                case "L":
                    var steps = distance / 90;
                    var index = Enumerable.Range(0, 4).First(c => headings[c] == heading);
                    heading = headings[(index + steps) % 4];
                    break;
                case "R":
                    var steps2 = distance / 90;
                    var index2 = Enumerable.Range(0, 4).First(c => headings[c] == heading);
                    heading = headings[(4 + index2 - steps2) % 4];
                    break;
                case "F":
                    return Move(heading, distance, heading);
            }
            return (x, y, heading);
        }

        public void SecondStar()
        {
            var data = GetData().Select(s => (s.Substring(0, 1), int.Parse(s.Substring(1))));
            var x = 10;
            var y = 1;
            var direction = "E";
            var shipX = 0;
            var shipY = 0;

            foreach (var d in data)
            {
                if (d.Item1 == "F")
                {
                    shipX += d.Item2 * x;
                    shipY += d.Item2 * y;
                }
                else if (d.Item1 == "R")
                {
                    var steps = d.Item2 / 90;
                    var multX = -1;
                    var multY = 1;
                    for (int i = 0; i < steps; i++)
                    {
                        var tmp = x;
                        x = multY * y;
                        y = multX * tmp;
                        multX = -1 * multY;
                        multY = -1 * multX;
                    }
                }
                else if (d.Item1 == "L")
                {
                    var steps = d.Item2 / 90;
                    var multX = 1;
                    var multY = -1;
                    for (int i = 0; i < steps; i++)
                    {
                        var tmp = x;
                        x = multY * y;
                        y = multX * tmp;
                        multX = -1 * multY;
                        multY = -1 * multX;
                    }
                }
                else
                {
                    var result = Move(d.Item1, d.Item2, direction);
                    x += result.x;
                    y += result.y;
                    direction = result.heading;
                }
            }

            System.Console.WriteLine(Math.Abs(shipX) + Math.Abs(shipY));
        }

        List<string> GetData() => File.ReadAllLines("data.txt").ToList();

    }
}

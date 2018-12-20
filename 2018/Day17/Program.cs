using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.FirstStar();
        }


        public static void Print()
        {
            var data = File.ReadAllLines("input");
            var coord = data.Select(d => d.Split(", ").First()).ToList();
            System.Console.WriteLine("x min: " + coord.Where(c => c.StartsWith("x")).Min(c => int.Parse(c.Substring(2))));
            System.Console.WriteLine("x max: " + coord.Where(c => c.StartsWith("x")).Max(c => int.Parse(c.Substring(2))));
            System.Console.WriteLine("y min: " + coord.Where(c => c.StartsWith("y")).Min(c => int.Parse(c.Substring(2))));
            System.Console.WriteLine("y max: " + coord.Where(c => c.StartsWith("y")).Max(c => int.Parse(c.Substring(2))));
        }

        public enum Direction
        {
            Left,
            Right
        }

        public void FirstStar()
        {
            var map = GetMap();
            var flowStack = new Stack<(int X, int Y, Direction Direction)>();
            flowStack.Push((500 - 465, 1, Direction.Left));

            try
            {
                while (flowStack.Count != 0)
                {
                    var flow = flowStack.Pop();
                    var x = flow.X;
                    var y = flow.Y;
                    var done = false;

                    while (!done)
                    {
                        while (y < map.GetLength(0) && map[y, x] != '#' && map[y, x] != '~')
                        {
                            map[y, x] = '~';
                            y++;
                        }
                        if (y == map.GetLength(0))
                        {
                            done = true;
                        }
                        else if (map[y, x] == '~')
                        {
                            var offset = 0;
                            while (x - offset > 0 && map[y, x - offset] != '#' && map[y - 1, x - offset] == '~')
                            {
                                offset++;
                            }
                            if (x - offset > 0)
                            {
                                offset = 0;
                                while (map[y, x] != '#')
                                {
                                    y++;
                                }
                                y--;
                                var splashPoint = x;
                                while (map[y, x] != '#')
                                {
                                    x--;
                                }
                                var left = x;
                                x++;
                                while (map[y, x] != '#')
                                {
                                    x++;
                                }
                                var right = x;
                                x = splashPoint;
                                while (map[y, left] == '#' && map[y, right] == '#')
                                {
                                    y--;
                                    RowFill(map, x + 1, y, left, right);
                                    RowFill(map, x - 1, y, left, right);
                                }
                            }
                            done = true;
                        }
                        else
                        {
                            var splashPoint = x;
                            y--;
                            int left = 0;
                            int right = 0;
                            while (map[y, x] != '#' && x > 0)
                            {
                                x--;
                            }
                            if (x < 0)
                            {
                                x = splashPoint;
                                while (map[y, x + 1] != '#' || map[y, x - 1] != '#')
                                {
                                    y--;
                                }
                                if (map[y, x + 1] == '#')
                                {
                                    left = x;
                                    x++;
                                }
                                else
                                {
                                    right = x;
                                    x--;
                                }
                                splashPoint = x;
                            }
                            if (left == 0)
                            {
                                while (map[y, x] != '#' && x > 0)
                                {
                                    x--;
                                }
                                left = x;
                                x = splashPoint;
                            }
                            if (right == 0)
                            {
                                while (map[y, x] != '#')
                                {
                                    x++;
                                }
                                right = x;
                                x = splashPoint;
                            }
                            var rightRowFill = false;
                            var leftRowFill = false;
                            do
                            {
                                rightRowFill = RowFill(map, x + 1, y, left, right);
                                leftRowFill = RowFill(map, x - 1, y, left, right);
                                y--;
                            } while (!(leftRowFill || rightRowFill));
                            y++;
                            if (map[y, left] == '~' && map[y, right] == '~')
                            {
                                x = left - 1;
                                flowStack.Push((right + 1, y, Direction.Right));
                            }
                            else if (map[y, left] == '~')
                            {
                                x = left - 1;
                            }
                            else
                            {
                                x = right + 1;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Fail");
                System.Console.WriteLine(e);
            }
            finally
            {
                PrintMap(map);
            }
            var counter = 6;
            for (int y = 0; y < map.GetLength(0); y++) {
                for (int x = 0; x < map.GetLength(1); x++) {
                    if (map[y,x] == '~') {
                        counter++;
                    }
                }
            }
            System.Console.WriteLine(counter);
        }

        bool RowFill(char[,] map, int x, int y, int xleft, int xright)
        {
            if (map[y, x] == '.')
            {
                map[y, x] = '~';
                if (x == xleft || x == xright)
                {
                    return true;
                }
                RowFill(map, x, y + 1, xleft, xright);
                var left = RowFill(map, x + 1, y, xleft, xright);
                var right = RowFill(map, x - 1, y, xleft, xright);
                return left || right;
            }
            return false;
        }

        void Fill(char[,] map, int x, int y)
        {
            if (map[y, x] == '.')
            {
                map[y, x] = '~';
                Fill(map, x + 1, y);
                Fill(map, x - 1, y);
                Fill(map, x, y + 1);
            }
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
            var xOffset = 465;
            var map = new char[1896, 655 - xOffset];
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    map[y, x] = '.';
                }
            }
            map[0, 500 - xOffset] = '+';

            foreach (var line in File.ReadAllLines("input"))
            {
                var parts = line.Split(", ");
                if (line.StartsWith("x"))
                {
                    var x = int.Parse(parts[0].Substring(2)) - xOffset;
                    var toFrom = parts[1].Substring(2).Split("..").Select(i => int.Parse(i)).ToArray();
                    for (int y = toFrom[0]; y < toFrom[1]; y++)
                    {
                        map[y, x] = '#';
                    }
                }
                else
                {
                    var y = int.Parse(parts[0].Substring(2));
                    var toFrom = parts[1].Substring(2).Split("..").Select(i => int.Parse(i) - xOffset).ToArray();
                    for (int x = toFrom[0]; x < toFrom[1]; x++)
                    {
                        map[y, x] = '#';
                    }
                }
            }

            return map;
        }
    }

    public class Bucket
    {

        public Bucket()
        {
            SubBuckets = new List<Bucket>();
            Dividers = new List<Divider>();
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public List<Divider> Dividers { get; set; }

        public List<Bucket> SubBuckets { get; set; }

        public int DistanceToParent { get; set; }

    }

    public class Divider
    {

        public int XStart { get; set; }

        public int YStart { get; set; }

        public int XStop { get; set; }

        public int YStop { get; set; }

    }

}

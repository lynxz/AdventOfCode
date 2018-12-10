using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.FirstAndSecondDay();
        }


        public void FirstAndSecondDay()
        {
            var data = GetData().ToArray();
            var counter = 0;
            while (true)
            {
                counter++;
                foreach (var point in data)
                {
                    point.X += point.Vx;
                    point.Y += point.Vy;
                }
                var comp = data.First().Y;
                if (data.Max(p => Math.Abs(p.Y - comp)) < 10)
                {
                    var row = data.Max(p => p.X) + 1;
                    var column = data.Max(p => p.Y) + 1;
                    var minRow = data.Min(p => p.X) - 1;
                    var minColumn = data.Min(p => p.Y) - 1;
                    row -= minRow;
                    column -= minColumn;
                    var matrix = Enumerable.Repeat('.', row * column).ToArray();
                    var sb = new StringBuilder();
                    foreach (var point in data)
                    {
                        matrix[(point.X - minRow) + row * (point.Y - minColumn)] = '#';
                    }
                    using (var fs = new StreamWriter("output"))
                    {
                        for (int i = 0; i < column; i++)
                        {
                            for (int j = 0; j < row; j++)
                            {
                                fs.Write(matrix[j + i * row]);
                            }
                            fs.WriteLine();
                        }
                    }
                    System.Console.WriteLine(counter);
                    return;
                }
            }
        }

        IEnumerable<Point> GetData()
        {
            var lines = File.ReadAllLines("input");
            foreach (var line in lines)
            {
                var loc = line.Substring(10, 14).Split(',');
                var speed = line.Substring(36, 6).Split(',');
                yield return new Point
                {
                    X = int.Parse(loc[0]),
                    Y = int.Parse(loc[1]),
                    Vx = int.Parse(speed[0]),
                    Vy = int.Parse(speed[1])
                };
            }
        }

    }

    public class Point
    {

        public int X { get; set; }

        public int Y { get; set; }

        public int Vx { get; set; }

        public int Vy { get; set; }


    }
}

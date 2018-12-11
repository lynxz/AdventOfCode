using System;

namespace Day11
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
            var data = GetData();
            var highPower = 0;
            var xHigh = 0;
            var yHigh = 0;
            for (int i = 0; i < 300 - 3; i++)
            {
                for (int j = 0; j < 300 - 3; j++)
                {
                    var totalPower = 0;
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            totalPower += data[j + x + ((i + y) * 300)];
                        }
                    }
                    if (totalPower > highPower)
                    {
                        highPower = totalPower;
                        xHigh = j;
                        yHigh = i;
                    }
                }
            }
            System.Console.WriteLine($"{xHigh + 1},{yHigh + 1}");
        }

        public void SecondStar()
        {
            var data = GetData();
            var highPower = 0;
            var xHigh = 0;
            var yHigh = 0;
            var size = 0;
            for (int s = 1; s < 300; s++)
            {
                for (int i = 0; i < 300 - s; i++)
                {
                    for (int j = 0; j < 300 - s; j++)
                    {
                        var totalPower = 0;
                        for (int y = 0; y < s; y++)
                        {
                            for (int x = 0; x < s; x++)
                            {
                                totalPower += data[j + x + ((i + y) * 300)];
                            }
                        }
                        if (totalPower > highPower)
                        {
                            highPower = totalPower;
                            xHigh = j;
                            yHigh = i;
                            size = s;
                        }
                    }
                }
                System.Console.WriteLine(s + " done!");
            }
            System.Console.WriteLine($"{xHigh + 1},{yHigh + 1},{size}");
        }

        int[] GetData()
        {
            var grid = new int[300 * 300];
            var serial = 2866;
            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    var rack = (j + 1) + 10;
                    var power = rack * (i + 1);
                    power += serial;
                    power *= rack;
                    power = ((power / 100) % 10);
                    grid[j + i * 300] = power - 5;
                }
            }
            return grid;
        }

    }
}

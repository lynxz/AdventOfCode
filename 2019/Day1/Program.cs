using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            //program.FirstStar();
            program.SecondStar();
        }

        void FirstStar() {
            var result = GetData().Select(i => (i / 3) - 2).Sum();
            System.Console.WriteLine(result);
        }

        void SecondStar() {
            var result = GetData().Sum(i => FuelCalculator(i));
            System.Console.WriteLine(result);
        }

        int FuelCalculator(int weight) {
            var fuel = (weight /3) - 2;
            if (fuel <= 0) {
                return 0;
            }
            return fuel + FuelCalculator(fuel); 
        }

        IEnumerable<int> GetData() 
            => File.ReadAllLines("input.txt").Select(l => int.Parse(l));

    }
}

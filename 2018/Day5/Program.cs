using System;
using System.IO;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.FirstStar();
        }

        public void FirstStar()
        {

        }

        public void SecondStar()
        {

        }

        string[] GetData() => File.ReadAllLines("input");

    }
}

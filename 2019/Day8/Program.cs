using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
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
            var size = 25 * 6;
            var data = GetData();
            var noLayers = data.Length / size;
            var image = Enumerable.Range(0, noLayers).Select(i => data.Skip(i * size).Take(size).ToArray()).ToList();
            var lowest = int.MaxValue;
            var result = 0;
            foreach (var layer in image)
            {
                var noZeros = layer.Count(c => c == '0');
                if (noZeros < lowest)
                {
                    result = layer.Count(c => c == '1') * layer.Count(c => c == '2');
                    lowest = noZeros;
                }
            }
            System.Console.WriteLine(result);
        }

        public void SecondStar()
        {
            var size = 25*6;
            var data = GetData();
            var noLayers = data.Length / size;
            var image = Enumerable.Range(0, noLayers).Select(i => data.Skip(i * size).Take(size).ToArray()).ToList();
            var newImage = new char[size];
            for(int i = 0; i < size; i++) {
                newImage[i] = image.First(l => l[i] != '2')[i];
            }
            for(int i = 0; i < 6; i++) {
                for(int j = 0; j < 25; j++) {
                    Console.Out.Write(newImage[i*25 + j] == '0' ? " " : "8");
                }
                System.Console.WriteLine();
            }

        }

        string GetData()
            => File.ReadAllText("input.txt");

    }
}

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day3 {
    class Program {
        static void Main (string[] args) {
            var program = new Program ();
            program.SecondStar ();
        }

        public void FirstStar () {
            int[] square = GenerateSquare ();
            System.Console.WriteLine (square.Count (n => n > 1));
        }

        private int[] GenerateSquare () {
            var size = 1000;
            var square = new int[size * size];
            var data = GetData ();
            foreach (var entry in data) {
                var match = Regex.Match (entry, @"(\d+),(\d+):\s(\d+)x(\d+)");
                var xoff = int.Parse (match.Groups[1].Value);
                var yoff = int.Parse (match.Groups[2].Value);
                var x = int.Parse (match.Groups[3].Value);
                var y = int.Parse (match.Groups[4].Value);

                for (int i = 0; i < y; i++) {
                    for (int j = 0; j < x; j++) {
                        square[(size * (yoff + i)) + (xoff + j)]++;
                    }
                }
            }

            return square;
        }

        public void SecondStar () {
            int[] square = GenerateSquare ();
            var data = GetData ();
            var id = 1;
            foreach (var entry in data) {
                var match = Regex.Match (entry, @"(\d+),(\d+):\s(\d+)x(\d+)");
                var xoff = int.Parse (match.Groups[1].Value);
                var yoff = int.Parse (match.Groups[2].Value);
                var x = int.Parse (match.Groups[3].Value);
                var y = int.Parse (match.Groups[4].Value);
                if (Validate (square, xoff, yoff, x, y)) {
                    System.Console.WriteLine (id);
                    return;
                }
                id++;
            }
        }

        private static bool Validate (int[] square, int xoff, int yoff, int x, int y) {
            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    if (square[(1000 * (yoff + i)) + (xoff + j)] != 1) {
                        return false;
                    }
                }
            }
            return true;
        }

        string[] GetData () => File.ReadAllLines ("input");
    }
}
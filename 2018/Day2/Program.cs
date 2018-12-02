using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Day2 {
    class Program {
        static void Main (string[] args) {
            var program = new Program ();
            program.SecondStar ();
        }

        public void FirstStar () {
            var data = GetData ();
            var numberOfTwos = 0;
            var numberOfThrees = 0;
            foreach (var line in data) {
                var counter = new int[27];
                foreach (var c in line) {
                    counter[(int) (c - 'a')]++;
                }
                numberOfTwos += counter.Any (n => n == 2) ? 1 : 0;
                numberOfThrees += counter.Any (n => n == 3) ? 1 : 0;
            }
            System.Console.WriteLine (numberOfThrees * numberOfTwos);
        }

        public void SecondStar () {
            var data = GetData ();
            for (int i = 0; i < data.Length - 1; i++) {
                for (int j = i + 1; j < data.Length; j++) {
                    var nonMatches = 0;
                    var place = 0;
                    for (int k = 0; k < data[i].Length; k++) {
                        if (data[i][k] != data[j][k]) {
                            nonMatches++;
                            place = k;
                        }
                        if (nonMatches > 1) {
                            break;
                        }
                    }
                    if (nonMatches == 1) {
                        System.Console.WriteLine (data[i].Remove(place, 1));
                        return;
                    }
                }
            }
        }

        string[] GetData () => File.ReadAllLines ("input");

    }
}
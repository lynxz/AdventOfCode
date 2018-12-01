using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1 {
    class Program {
        
        static void Main (string[] args) {
            var program = new Program ();
            //program.FirstStar ();
            program.SecondStar ();
        }

        void FirstStar () {
            var sum = File.ReadAllLines ("input")
                .Select (n => int.Parse (n))
                .Sum ();
            System.Console.WriteLine (sum);
        }

        void SecondStar () {
            var data = File.ReadAllLines ("input").Select (n => int.Parse (n)).ToArray ();
            var hashset = new HashSet<int> ();
            var index = 0;
            var sum = 0;
            while (true) {
                sum += data[index];
                if (hashset.Contains (sum)) {
                    System.Console.WriteLine (sum);
                    return;
                } else {
                    hashset.Add (sum);
                }
                index++;
                if (index >= data.Length) {
                    index = 0;
                }
            }
        }

    }
}
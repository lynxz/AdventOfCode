using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var prg = new Program();
            prg.FirstStar();
            prg.SecondStar();
        }

        void FirstStar()
        {
            var preambleLength = 25;
            var data = GetData();
            
            for(int i = 0; i< data.Count - preambleLength; i++ ) {
                var preamble = data.Skip(i).Take(preambleLength).ToList();
                var valid = false;
                for (int j = 0; j < preamble.Count - 1; j++) {
                    for (int k = j+1; k < preamble.Count; k++) {
                        if (preamble[j] + preamble[k] == data[i+preambleLength] && preamble[j] != preamble[k]) {
                            valid = true;
                            break;
                        }
                    }
                    if (valid) {
                        break;
                    }
                }
                if (!valid) {
                    System.Console.WriteLine(data[i+preambleLength]);
                    break;
                }
            }
        }

        void SecondStar()
        {
            var value = 144381670L;
            var data = GetData();
            var indexOf = data.IndexOf(value);

            for (int i = 2; i < indexOf; i++) {
                for (int j = 0; j < indexOf - i; j++) {
                    var values = data.Skip(j).Take(i).ToList();
                    var sum = values.Sum();
                    if (sum == value) {
                        System.Console.WriteLine(values.Min() + values.Max());
                        Environment.Exit(0);
                    }
                }
            }
        }

        List<long> GetData() => File.ReadAllLines("data.txt").Select(n => long.Parse(n)).ToList();
    }
}

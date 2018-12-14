using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
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
            var itter = 286051;
            var data = new List<int> { 3, 7 };
            var e1 = 0;
            var e2 = 1;
            while (data.Count < itter + 10)
            {
                var sum = data[e1] + data[e2];
                if (sum < 10)
                {
                    data.Add(sum);
                }
                else
                {
                    data.Add(sum / 10);
                    data.Add(sum % 10);
                }
                e1 = (e1 + data[e1] + 1) % data.Count;
                e2 = (e2 + data[e2] + 1) % data.Count;
            }

            System.Console.WriteLine(string.Join("", data.Skip(itter).Take(10)));
        }

        public void SecondStar()
        {
            //286051
            var itter = "286051".Select(i => int.Parse(new [] {i})).ToArray();;
            var data = new List<int> { 3, 7 };
            var e1 = 0;
            var e2 = 1;
            var pos = 0;
            while (true)
            {
                var sum = data[e1] + data[e2];
                if (sum < 10)
                {
                    data.Add(sum);
                }
                else
                {
                    data.Add(sum / 10);
                    data.Add(sum % 10);
                }
                if (data.Count > itter.Length) {
                    while (pos < data.Count - itter.Length) {
                        if (Enumerable.Range(0, itter.Length).All(i => data[i+pos] == itter[i])) {
                            System.Console.WriteLine(pos);
                            return;
                        }
                        pos++;
                    }
                }
                e1 = (e1 + data[e1] + 1) % data.Count;
                e2 = (e2 + data[e2] + 1) % data.Count;
            }
        }

        List<int> GetData() => "286051".Select(d => int.Parse(new string(new[] { d }))).ToList();

    }
}

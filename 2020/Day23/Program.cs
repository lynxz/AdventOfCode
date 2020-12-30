using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            prog.FirstStar();
            prog.SecondStar();
        }

        private void FirstStar()
        {
            var data = GetData();
            Iterate(data, 100);
            var startIndex = data.IndexOf(1);
            Enumerable.Range(1, 8);
            System.Console.WriteLine(string.Join("", Enumerable.Range(1, 8).Select(i => data[(startIndex + i) % data.Count])));
        }

        private static void Iterate(List<int> data, int iterations)
        {
            var currentValue = data.First();
            var tmp = new int[3];
            var stopwatch = new Stopwatch();
            var max = data.Max();
            stopwatch.Start();
            var counter = 0;
            for (int i = 0; i < iterations; i++)
            {
                var currentIndex = data.IndexOf(currentValue);
                for (int j = 1; j < 4; j++)
                    tmp[j - 1] = data[(currentIndex + j) % data.Count];

                var dest = currentValue - 1;
                var done = false;
                while (!done)
                {
                    done = true;
                    if (dest <= 0)
                        dest = max;
                    if (tmp.Contains(dest))
                    {
                        dest--;
                        done = false;
                    }
                }
                for (int j = 0; j < 3; j++)
                    data.Remove(tmp[j]);
                var destIndex = data.IndexOf(dest);
                data.InsertRange(destIndex + 1, tmp);

                currentIndex = data.IndexOf(currentValue) + 1;
                if (currentIndex >= data.Count)
                    currentIndex = 0;
                currentValue = data[currentIndex];
                if (i % 100_000 == 0)
                {
                    System.Console.WriteLine(counter + ": " + stopwatch.Elapsed);
                    stopwatch.Restart();
                    counter++;
                }
            }
        }

        private static void Iterate2(LinkedList<int> data, int iterations)
        {
            var start = data.First;
            var currentNode = start;
            var max = data.Max();
            var tmp = new int[3];
            var index = new Dictionary<int, LinkedListNode<int>>();
            do
            {
                index.Add(currentNode.Value, currentNode);
                currentNode = currentNode.Next;
            } while (currentNode != null);
            currentNode = data.First;

            for (var j = 0; j < iterations; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    var nextNode = currentNode.Next == null ? data.First : currentNode.Next;
                    tmp[i] = nextNode.Value;
                    data.Remove(nextNode);
                }
                var dest = currentNode.Value - 1;
                var done = false;
                while (!done)
                {
                    done = true;
                    if (dest <= 0)
                        dest = max;
                    if (tmp.Contains(dest))
                    {
                        dest--;
                        done = false;
                    }
                }
                var destNode = index[dest];
                foreach (var val in tmp.Reverse())
                {
                    data.AddAfter(destNode, val);
                    index[val] = destNode.Next;
                }
                currentNode = currentNode.Next ?? data.First;
            }
        }

        private void SecondStar()
        {
            var data = GetData();
            for (int i = 10; i <= 1_000_000; i++)
            {
                data.Add(i);
            }
            var newData = new LinkedList<int>(data);
            Iterate2(newData, 10_000_000);
            var node = newData.Find(1);
            var nextValue = node.Next ?? newData.First;
            var nextNextValue = nextValue.Next ?? newData.First;
            System.Console.WriteLine(Convert.ToUInt64(nextValue.Value) * Convert.ToUInt64(nextNextValue.Value));
        }

        List<int> GetData() => "198753462".Select(c => int.Parse(c.ToString())).ToList();

    }

}

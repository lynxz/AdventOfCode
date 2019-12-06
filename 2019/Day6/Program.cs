using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
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
            var graph = GetGraph();
            System.Console.WriteLine(Count(0, graph, "COM"));
        }

        public void SecondStar() {
            var graph = GetGraph();
            var parent = "SAN";
            var santaList = new List<string>();
            do {
                santaList.Add(parent);
                parent = FindParent(graph, parent);
            } while (!string.IsNullOrEmpty(parent));
            parent = "YOU";
            var index = -1;
            var dist = 0;
            do {
                parent = FindParent(graph, parent);
                dist++;
                index = santaList.IndexOf(parent);
            } while (index == -1);
            System.Console.WriteLine(index + dist - 2);
        }

        private Dictionary<string, List<string>> GetGraph()
        {
            var graph = new Dictionary<string, List<string>>();
            var nodes = GetData().ToList();
            foreach (var node in nodes)
            {
                var parts = node.Split(')');
                if (!graph.ContainsKey(parts[0]))
                {
                    graph.Add(parts[0], new List<string>());
                }
                graph[parts[0]].Add(parts[1]);
            }

            return graph;
        }

        string FindParent(Dictionary<string, List<string>> graph, string child) {
            foreach(var kvp in graph) {
                if (kvp.Value.Any(n => n == child)) {
                    return kvp.Key;
                }
            }
            return null;
        }

        int Count(int level, Dictionary<string, List<string>> graph, string node) {
            var sum = level;
            if (graph.ContainsKey(node)) {
                foreach (var subnode in graph[node]) {
                    sum += Count(level + 1, graph, subnode);
                }
            }
            return sum;
        }
 

        IEnumerable<string> GetData() 
            => File.ReadAllLines("input.txt");

    }
}

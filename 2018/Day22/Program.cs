using System;
using System.Collections.Generic;
using System.Linq;

namespace Day22
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
            var map = new int[739 + 1, 9 + 1];
            var depth = 10914;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var geolocialIndex = 0;
                    if (y == map.GetLength(0) - 1 && x == map.GetLength(1) - 1)
                    {
                        geolocialIndex = 0;
                    }
                    else if (y == 0)
                    {
                        geolocialIndex = x * 16807;
                    }
                    else if (x == 0)
                    {
                        geolocialIndex = y * 48271;
                    }
                    else
                    {
                        geolocialIndex = map[y, x - 1] * map[y - 1, x];
                    }
                    map[y, x] = (geolocialIndex + depth) % 20183;
                }
            }

            var sum = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var risk = map[y, x] % 3;
                    if (risk == 0)
                    {
                        System.Console.Write('.');
                    }
                    else if (risk == 1)
                    {
                        System.Console.Write('=');
                    }
                    else
                    {
                        System.Console.Write('|');
                    }
                    sum += risk;
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine(sum);
        }

        public void SecondStar()
        {
            var rows = 739 + 50;
            var columns = 9 + 50;
            var graph = new Dictionary<Node, List<Node>>();
            var indexMap = new int[rows, columns];
            var depth = 10914;
            for (int y = 0; y < indexMap.GetLength(0); y++)
            {
                for (int x = 0; x < indexMap.GetLength(1); x++)
                {
                    var geolocialIndex = 0;
                    if (y == 739 && x == 9)
                    {
                        geolocialIndex = 0;
                    }
                    else if (y == 0)
                    {
                        geolocialIndex = x * 16807;
                    }
                    else if (x == 0)
                    {
                        geolocialIndex = y * 48271;
                    }
                    else
                    {
                        geolocialIndex = indexMap[y, x - 1] * indexMap[y - 1, x];
                    }
                    indexMap[y, x] = (geolocialIndex + depth) % 20183;
                    var region = indexMap[y, x] % 3;
                    var nodes = GetNodesForRegion(x, y, region);
                    foreach (var node in nodes)
                    {
                        graph.Add(node, new List<Node>());
                        if (y > 0)
                        {
                            var topNodes = graph.Keys.Where(n => n.X == x && n.Y == y - 1);
                            foreach (var topNode in topNodes)
                            {
                                graph[topNode].Add(node);
                                graph[node].Add(topNode);
                            }
                        }
                        if (x > 0)
                        {
                            var sideNodes = graph.Keys.Where(n => n.X == x - 1 && n.Y == y);
                            foreach (var sideNode in sideNodes)
                            {
                                graph[sideNode].Add(node);
                                graph[node].Add(sideNode);
                            }
                        }
                    }
                }
            }
            var start = graph.Keys.First(n => n.X == 0 && n.Y == 0 && n.Tool == Tools.Torch);
            var end = graph.Keys.First(n => n.X == 9 && n.Y == 739 && n.Tool == Tools.Torch);

            System.Console.WriteLine(ShortestPath(graph, start, end));
        }

        private IEnumerable<Node> GetNodesForRegion(int x, int y, int region)
        {
            if (region == 0)
            {
                yield return new Node { X = x, Y = y, Region = region, Tool = Tools.Rope };
                yield return new Node { X = x, Y = y, Region = region, Tool = Tools.Torch };
            }
            else if (region == 1)
            {
                yield return new Node { X = x, Y = y, Region = region, Tool = Tools.Rope };
                yield return new Node { X = x, Y = y, Region = region, Tool = Tools.Empty };
            }
            else
            {
                yield return new Node { X = x, Y = y, Region = region, Tool = Tools.Torch };
                yield return new Node { X = x, Y = y, Region = region, Tool = Tools.Empty };
            }
        }

        public int ShortestPath(Dictionary<Node, List<Node>> graph, Node start, Node end)
        {
            var q = graph.Keys.ToList();
            var time = q.ToDictionary(k => k, k => int.MaxValue);
            time[start] = 0;

            while (q.Count > 0)
            {
                var min = int.MaxValue;
                Node u = null;
                foreach (var node in q)
                {
                    if (time[node] < min)
                    {
                        min = time[node];
                        u = node;
                    }
                }
                q.Remove(u);
                if (q.Count % 100 == 0)
                {
                    System.Console.WriteLine("Nodes left: " + q.Count);
                }
                if (u == end)
                {
                    return time[end];
                }
                foreach (var v in graph[u])
                {
                    var alt = time[u] + (u.Tool != v.Tool ? 8 : 1);
                    if (alt < time[v])
                    {
                        time[v] = alt;
                    }
                }
            }

            return 0;
        }

    }

    public enum Tools
    {
        Torch,
        Rope,
        Empty
    }

    public class Node
    {

        public int Region { get; set; }

        public Tools Tool { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

    }

}

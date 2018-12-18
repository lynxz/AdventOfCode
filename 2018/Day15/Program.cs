using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
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
            var map = GetData().ToTwoDimensionalArray();
            var graph = GenerateGraph(map);
            var result = SimulateBattle(map, graph);
            System.Console.WriteLine(result);
        }

        private int SimulateBattle(char[,] map, Dictionary<Node, List<Node>> graph, bool print = true)
        {
            var counter = 0;
            if (print)
            {
                Print(map, graph.Keys.Where(n => n.Occupied != null).ToList());
                System.Console.WriteLine(string.Join(",", graph.Keys.Where(n => n.Occupied != null).Select(n => $"{n.Occupied.Class} {n.Occupied.HP}")));
            }
            var notDone = true;
            while (notDone)
            {
                notDone = false;
                foreach (var k in graph.Keys.Where(n => n.Occupied != null).OrderBy(x => x.Y).ThenBy(x => x.X).ToList())
                {
                    var result = Step(graph, k);
                    if (result.Item1)
                    {
                        notDone = true;
                        Attack(graph, result.Item2);
                    }
                }
                if (notDone)
                {
                    if (print)
                    {
                        Print(map, graph.Keys.Where(n => n.Occupied != null).ToList());
                        System.Console.WriteLine(string.Join(",", graph.Keys.Where(n => n.Occupied != null).Select(n => $"{n.Occupied.Class} {n.Occupied.HP}")));
                    }
                }
                var occupied = graph.Keys.Where(n => n.Occupied != null).ToList();
                if (occupied.Where(n => n.Occupied.Class == Class.Elf).Count() > 0 && occupied.Where(n => n.Occupied.Class == Class.Gnome).Count() > 0)
                {
                    counter++;
                }
            }
            var hp = graph.Keys.Where(n => n.Occupied != null).Sum(n => n.Occupied.HP);
            if (print)
            {
                System.Console.WriteLine($"Steps: {counter} HP: {hp}");
            }
            return counter * hp;
        }

        public void SecondStar()
        {
            for (int i = 8; i < 30; i++)
            {
                var map = GetData().ToTwoDimensionalArray();
                var graph = GenerateGraph(map, i);
                var elves = graph.Keys.Where(n => n.Occupied != null && n.Occupied.Class == Class.Elf).Count();
                var result = SimulateBattle(map, graph, false);
                var survivors = graph.Keys.Where(n => n.Occupied != null && n.Occupied.Class == Class.Elf).Count();
                if (survivors == elves)
                {
                    System.Console.WriteLine("Success " + " " + i + " " + result);
                    return;
                }
                else
                {
                    System.Console.WriteLine("Fail" + " " + i + " " + result);
                }
            }
        }

        void Print(char[,] map, List<Node> occupiedNodes)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    var n = occupiedNodes.FirstOrDefault(on => on.X == j && on.Y == i);
                    if (n != null)
                    {
                        Console.Write(n.Occupied.Class == Class.Elf ? 'E' : 'G');
                    }
                    else
                    {
                        Console.Write(map[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }

        void Attack(Dictionary<Node, List<Node>> g, Node n)
        {
            foreach (var nb in g[n].Where(x => x.Occupied != null).OrderBy(x => x.Occupied.HP).ThenBy(x => x.Y).ThenBy(x => x.X))
            {
                if (nb.Occupied != null && n.Occupied != null && nb.Occupied.Class != n.Occupied.Class)
                {
                    nb.Occupied.HP -= n.Occupied.Force;
                    if (nb.Occupied.HP <= 0)
                    {
                        nb.Occupied = null;
                    }
                    break;
                }
            }
        }

        (bool, Node) Step(Dictionary<Node, List<Node>> g, Node node)
        {
            if (node.Occupied == null)
            {
                return (false, node);
            }
            var dist = 5000;
            Node nextNode = null;

            if (g[node].Any(n => n.Occupied != null && n.Occupied.Class != node.Occupied.Class))
            {
                return (true, node);
            }

            foreach (var enemy in g.Keys.Where(n => n.Occupied != null && n.Occupied.Class != node.Occupied.Class).OrderBy(n => n.Y).ThenBy(n => n.X))
            {
                var found = Djikstra(g, enemy, node);
                if (found.Item1 < dist)
                {
                    dist = found.Item1;
                    nextNode = found.Item2;
                }
            }

            if (nextNode != null)
            {
                if (nextNode.Occupied == null)
                {
                    nextNode.Occupied = node.Occupied;
                    node.Occupied = null;
                    return (true, nextNode);
                }
                return (true, node);
            }

            return (false, node);
        }

        (int, Node) Djikstra(Dictionary<Node, List<Node>> graph, Node start, Node stop)
        {
            var q = new HashSet<Node>(graph.Keys);
            var dist = graph.Keys.ToDictionary(k => k, k => int.MaxValue);
            var prev = new Dictionary<Node, Node>();

            dist[start] = 0;

            while (q.Count > 0)
            {
                Node node = null;
                var minDist = int.MaxValue;
                foreach (var n in q.OrderBy(k => k.Y).ThenBy(k => k.X))
                {
                    if (dist[n] < minDist)
                    {
                        node = n;
                        minDist = dist[n];
                    }
                }

                if (node == null)
                {
                    return (int.MaxValue, start);
                }

                q.Remove(node);

                if (node == stop)
                {
                    return (dist[node], prev[node]);
                }

                foreach (var neighbor in graph[node].OrderBy(n => n.Y).ThenBy(n => n.X))
                {
                    var newDist = (neighbor.Occupied == null || neighbor == stop) && dist[node] != int.MaxValue ? dist[node] + 1 : int.MaxValue;
                    if (newDist < dist[neighbor])
                    {
                        dist[neighbor] = newDist;
                        prev[neighbor] = node;
                    }
                }
            }

            return (dist[stop], prev[stop]);
        }

        Dictionary<Node, List<Node>> GenerateGraph(char[,] map, int elfForce = 3)
        {
            var graph = new Dictionary<Node, List<Node>>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '.')
                    {
                        CoreGenerateGraph(map, graph, i, j);
                        return graph;
                    }
                }
            }

            Node CoreGenerateGraph(char[,] m, Dictionary<Node, List<Node>> g, int y, int x)
            {
                if (IsValid(map[y, x]))
                {
                    var n = g.Keys.FirstOrDefault(k => k.X == x && k.Y == y);
                    if (n == null)
                    {
                        n = GenerateNode(map[y, x], y, x, elfForce);
                        map[y, x] = '.';
                        g.Add(n, new List<Node>());
                        var r = CoreGenerateGraph(m, g, n.Y - 1, n.X);
                        if (r != null)
                        {
                            g[n].Add(r);
                        }
                        r = CoreGenerateGraph(m, g, n.Y + 1, n.X);
                        if (r != null)
                        {
                            g[n].Add(r);
                        }
                        r = CoreGenerateGraph(m, g, n.Y, n.X - 1);
                        if (r != null)
                        {
                            g[n].Add(r);
                        }
                        r = CoreGenerateGraph(m, g, n.Y, n.X + 1);
                        if (r != null)
                        {
                            g[n].Add(r);
                        }
                    }
                    return n;
                }
                return null;
            }

            return graph;
        }

        bool IsValid(char c) => c == '.' || c == 'G' || c == 'E';

        Node GenerateNode(char c, int Y, int X, int elfForce = 3)
        {
            var newNode = new Node { Y = Y, X = X };
            switch (c)
            {
                case 'G':
                    newNode.Occupied = new Character { Class = Class.Gnome, HP = 200, Force = 3 };
                    break;
                case 'E':
                    newNode.Occupied = new Character { Class = Class.Elf, HP = 200, Force = elfForce };
                    break;
            }
            return newNode;
        }
        IEnumerable<IEnumerable<char>> GetData() => File.ReadAllLines("input")
            .Select(c => c.ToArray());

    }

    public enum Class
    {
        Gnome,
        Elf
    }

    public class Node
    {

        public int X { get; set; }

        public int Y { get; set; }

        public Character Occupied { get; set; }

        public override int GetHashCode() => Y * 100000 + X;

    }

    public class Character
    {

        public Class Class { get; set; }

        public int HP { get; set; }

        public int Force { get; set; }

    }

}

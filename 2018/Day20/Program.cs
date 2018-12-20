using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {

        Dictionary<Node, List<Node>> _graph;

        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        public void FirstStar()
        {
            _graph = new Dictionary<Node, List<Node>>();
            var data = GetData();
            var root = new Node { X = 0, Y = 0 };
            _graph.Add(root, new List<Node>());
            GenerateGraph(data, 0, root);
            System.Console.WriteLine(ShortestPath(root).Max());
        }

        public void SecondStar()
        {
            _graph = new Dictionary<Node, List<Node>>();
            var data = GetData();
            var root = new Node { X = 0, Y = 0 };
            _graph.Add(root, new List<Node>());
            GenerateGraph(data, 0, root);
            System.Console.WriteLine(ShortestPath(root).Count(i => i >= 1000));
        }

        (Node node, int pos) GenerateGraph(string data, int pos, Node currentNode)
        {
            while (pos < data.Length)
            {
                switch (data[pos])
                {
                    case '^':
                        pos++;
                        break;
                    case '$':
                        pos++;
                        break;
                    case 'N':
                        currentNode = AddNodeToGraph(currentNode, new Node { X = currentNode.X, Y = currentNode.Y + 1 });
                        pos++;
                        break;
                    case 'E':
                        currentNode = AddNodeToGraph(currentNode, new Node { X = currentNode.X + 1, Y = currentNode.Y });
                        pos++;
                        break;
                    case 'W':
                        currentNode = AddNodeToGraph(currentNode, new Node { X = currentNode.X - 1, Y = currentNode.Y });
                        pos++;
                        break;
                    case 'S':
                        currentNode = AddNodeToGraph(currentNode, new Node { X = currentNode.X, Y = currentNode.Y - 1 });
                        pos++;
                        break;
                    case '(':
                        var branchNodes = new List<Node>();
                        Node branchNode = null;
                        pos++;
                        do
                        {
                            (Node n, int p) = GenerateGraph(data, pos, currentNode);
                            branchNode = n;
                            pos = p;
                            if (branchNode != null)
                            {
                                branchNodes.Add(branchNode);
                            }
                        } while (branchNode != null);
                        branchNodes.Select(n => GenerateGraph(data, pos, n));
                        break;
                    case ')':
                        return (null, ++pos);
                    case '|':
                        return (currentNode, ++pos);
                }
            }
            return (null, pos);
        }

        int[] ShortestPath(Node start) {
            var q = _graph.Keys.ToList();
            var dist = q.ToDictionary(k => k, k => int.MaxValue);
            dist[start] = 0;

            while (q.Count != 0) {
                var comp = int.MaxValue;
                Node u = null;
                foreach (var n in q) {
                    if (dist[n] < comp) {
                        u = n;
                        comp = dist[n];
                    }
                }
                q.Remove(u);

                foreach (var v in _graph[u]) {
                    var alt = dist[u] + 1;
                    if (alt < dist[v]) {
                        dist[v] = alt;
                    }
                }
            }
            return dist.Values.ToArray();
        }

        Node AddNodeToGraph(Node oldNode, Node newNode)
        {
            var returnValue = _graph.Keys.FirstOrDefault(n => n.X == newNode.X && n.Y == newNode.Y);
            if (returnValue == null)
            {
                returnValue = newNode;
                _graph.Add(newNode, new List<Node>());
            }
            if (!_graph[oldNode].Any(n => n.X == returnValue.X && n.Y == returnValue.Y) && 
                !(oldNode.X == returnValue.X && oldNode.Y == returnValue.Y) )
            {
                _graph[oldNode].Add(returnValue);
                _graph[returnValue].Add(oldNode);
            }
            return returnValue;
        }

        string GetData() => File.ReadAllText("input");
    }

    public class Node
    {

        public int X { get; set; }

        public int Y { get; set; }

    }

}

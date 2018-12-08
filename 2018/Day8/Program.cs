using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
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
            var data = GetData();
            var index = 0;
            var root = new Node();
            ProcessNode(data, index, root);
            System.Console.WriteLine(Sum(root));
        }

        int Sum(Node node)
        {
            var value = node.Metadata.Sum();
            value += node.Children.Sum(n => Sum(n));
            return value;
        }

        public void SecondStar()
        {
            var data = GetData();
            var index = 0;
            var root = new Node();
            ProcessNode(data, index, root);
            System.Console.WriteLine(NodeSum(root.Children.First()));
        }

        int NodeSum(Node n)
        {
            if (n.Children.Count == 0)
            {
                return n.Metadata.Sum();
            }
            var sum = 0;
            foreach (var meta in n.Metadata)
            {
                var index = meta - 1;
                if (index < n.Children.Count)
                {
                    sum += NodeSum(n.Children[index]);
                }
            }
            return sum;
        }

        public int ProcessNode(int[] data, int index, Node parent)
        {
            var newNode = new Node();
            parent.Children.Add(newNode);
            var children = data[index];
            var metadata = data[++index];
            for (var i = 0; i < children; i++)
            {
                index = ProcessNode(data, ++index, newNode);
            }
            for (var i = 0; i < metadata; i++)
            {
                newNode.Metadata.Add(data[++index]);
            }
            return index;
        }

        int[] GetData() => File.ReadAllText("input")
            .Split(' ')
            .Select(v => int.Parse(v)).ToArray();

    }

    public class Node
    {

        public Node()
        {
            Children = new List<Node>();
            Metadata = new List<int>();
        }

        public List<Node> Children { get; set; }

        public List<int> Metadata { get; set; }

    }
}

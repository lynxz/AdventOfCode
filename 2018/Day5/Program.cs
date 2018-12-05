using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day5
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
            var first = GetList(GetData());
            System.Console.WriteLine(GetReduceLength(first));
        }

        private static int GetReduceLength(Node first)
        {
            var current = first;
            while (current.Next != null)
            {
                if (current.Value != current.Next.Value && char.ToUpper(current.Value) == char.ToUpper(current.Next.Value))
                {
                    if (current.Previous != null)
                    {
                        current.Previous.Next = current.Next.Next;
                        current = current.Previous;
                        if (current.Next != null)
                        {
                            current.Next.Previous = current;
                        }
                    }
                    else
                    {
                        current = current.Next.Next;
                        current.Previous = null;
                        first = current;
                    }
                }
                else
                {
                    current = current.Next;
                }
            }

            current = first;
            int count = 0;
            do
            {
                current = current.Next;
                count++;
            } while (current != null);
            return count;
        }

        private Node GetList(string data)
        {
            Node first = null;
            Node current = null;
            foreach (var value in data)
            {
                var newNode = new Node { Value = value };
                if (first == null)
                {
                    first = newNode;
                    current = newNode;
                }
                else
                {
                    current.Next = newNode;
                    newNode.Previous = current;
                    current = newNode;
                }
            }
            return first;
        }

        public void SecondStar()
        {
            var length = int.MaxValue;
            foreach (var c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                var data = GetData().Replace(c.ToString(), "");
                data = data.Replace(new string(char.ToLower(c).ToString()), "");
                var first = GetList(data);
                var newLength = GetReduceLength(first);
                if (newLength < length) {
                    length = newLength;
                }
            }
            System.Console.WriteLine(length);
        }

        string GetData() => File.ReadAllText("input");

    }

    public class Node
    {

        public char Value { get; set; }

        public Node Next { get; set; }

        public Node Previous { get; set; }

    }

}

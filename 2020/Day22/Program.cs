using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
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
            Queue<int> player1 = new();
            Queue<int> player2 = new();
            var split = data.IndexOf(string.Empty);
            data.Skip(1).Take(split - 1).Select(n => int.Parse(n)).ToList().ForEach(n => player1.Enqueue(n));
            data.Skip(split + 2).Take(data.Count - split - 2).Select(n => int.Parse(n)).ToList().ForEach(n => player2.Enqueue(n));
            while (player1.Count > 0 && player2.Count > 0)
            {
                var p1 = player1.Dequeue();
                var p2 = player2.Dequeue();
                if (p1 > p2)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }
                else
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
            }
            var winner = player1.Count > player2.Count ? player1 : player2;
            var result = Enumerable.Range(1, winner.Count).Reverse().Sum(i => i * winner.Dequeue());
            System.Console.WriteLine(result);
        }

        private void SecondStar()
        {
            var data = GetData();
            Queue<int> player1 = new();
            Queue<int> player2 = new();

            var split = data.IndexOf(string.Empty);
            data.Skip(1).Take(split - 1).Select(n => int.Parse(n)).ToList().ForEach(n => player1.Enqueue(n));
            data.Skip(split + 2).Take(data.Count - split - 2).Select(n => int.Parse(n)).ToList().ForEach(n => player2.Enqueue(n));
            var winner = Play(player1, player2) == 1 ? player1 : player2;
            System.Console.WriteLine(Enumerable.Range(1, winner.Count).Reverse().Sum(i => i * winner.Dequeue()));
        }

        private static int Play(Queue<int> player1, Queue<int> player2)
        {
            HashSet<string> mem1 = new();
            HashSet<string> mem2 = new();
            while (player1.Count > 0 && player2.Count > 0)
            {
                var hash1 = string.Join(" ", player1.Select(i => i.ToString()));
                var hash2 = string.Join(" ", player2.Select(i => i.ToString()));
                if (mem1.Contains(hash1) || mem2.Contains(hash2))
                {
                    return 1;
                }
                mem1.Add(hash1);
                mem2.Add(hash2);

                var p1 = player1.Dequeue();
                var p2 = player2.Dequeue();
                var winner = 0;
                if (player1.Count >= p1 && player2.Count >= p2)
                {
                    winner = Play(new Queue<int>(player1.Take(p1)), new Queue<int>(player2.Take(p2)));
                }
                else
                {
                    winner = p1 > p2 ? 1 : 2;
                }
                if (winner == 1)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }
                else
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
            }

            return player1.Count > player2.Count ? 1 : 2;
        }

        List<string> GetData() => File.ReadAllLines("data.txt").ToList();

    }
}

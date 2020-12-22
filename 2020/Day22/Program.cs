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
            var players = GetData();
            while (players.All(p => p.Count > 0))
            {
                var cards = players.Select(p => p.Dequeue()).ToArray();
                var winner = cards[0] > cards[1] ? 0 : 1;
                players[winner].Enqueue(cards[winner]);
                players[winner].Enqueue(cards[(winner + 1) % 2]);
            }
            System.Console.WriteLine(players[players[0].Count > players[1].Count ? 0 : 1].Reverse().Select((c, i) => c * (i + 1)).Sum());
        }

        private void SecondStar()
        {
            var players = GetData();
            var winner = Play(players);
            System.Console.WriteLine(Enumerable.Range(1, players[winner].Count).Reverse().Sum(i => i * players[winner].Dequeue()));
        }

        private static int Play(Queue<int>[] players)
        {
            var memories = new [ ] {new HashSet<string>(), new HashSet<string>()};
            while (players.All(p => p.Count > 0))
            {
                var hashes = players.Select(p => string.Join(" ", p.Select(i => i))).ToArray();
                if (Enumerable.Range(0,2).Any(i => memories[i].Contains(hashes[i])))
                    return 0;
                
                Enumerable.Range(0,2).ToList().ForEach(i => memories[i].Add(hashes[i]));

                var cards = players.Select(p => p.Dequeue()).ToArray();
                var winner = 0;
                if (Enumerable.Range(0, 2).All(i => players[i].Count >= cards[i]))
                    winner = Play(players.Select((p, i) => new Queue<int>(p.Take(cards[i]))).ToArray());
                else
                    winner = cards[0] > cards[1] ? 0 : 1;

                players[winner].Enqueue(cards[winner]);
                players[winner].Enqueue(cards[(winner + 1) % 2]);
            }

            return players[0].Count > players[1].Count ? 0 : 1;
        }

        Queue<int>[] GetData() => File.ReadAllText("data.txt")
            .Split("\r\n\r\n")
            .Select(p => new Queue<int>(p.Split(Environment.NewLine).Skip(1).Select(int.Parse)))
            .ToArray();

    }
}

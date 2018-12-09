using System;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.FirstStar();
        }

        public void FirstStar()
        {
            System.Console.WriteLine(FindHighScore(70769));
        }

        long FindHighScore(int endMarble)
        {
            var players = new long[418];
            var currentPlayer = 0;
            var highMarble = 0;
            var currentMarble = new Marble
            {
                Number = 0
            };
            currentMarble.Next = currentMarble;
            currentMarble.Previous = currentMarble;
            while (highMarble < endMarble)
            {
                currentPlayer++;
                highMarble++;
                if (currentPlayer >= players.Length)
                {
                    currentPlayer = 0;
                }
                if (highMarble % 23 == 0)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        currentMarble = currentMarble.Previous;
                    }
                    players[currentPlayer] += highMarble + currentMarble.Number;
                    var prevMarble = currentMarble.Previous;
                    currentMarble = currentMarble.Next;
                    prevMarble.Next = currentMarble;
                    currentMarble.Previous = prevMarble;
                }
                else
                {
                    var firstMarble = currentMarble.Next;
                    var secondMarble = currentMarble.Next.Next;
                    var newMarble = new Marble
                    {
                        Number = highMarble,
                        Previous = firstMarble,
                        Next = secondMarble
                    };
                    firstMarble.Next = newMarble;
                    secondMarble.Previous = newMarble;
                    currentMarble = newMarble;
                }
            }
            return players.Max();
        }

        public void SecondStar()
        {
            System.Console.WriteLine(FindHighScore(70769 * 100));
        }

    }

    public class Marble
    {

        public int Number { get; set; }

        public Marble Next { get; set; }

        public Marble Previous { get; set; }

    }

}

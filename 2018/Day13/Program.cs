using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
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
            var tracks = GetData();
            var carts = GetCarts(tracks).ToList();
            while (Tick(tracks, carts));
        }

        public void SecondStar()
        {
            var tracks = GetData();
            var carts = GetCarts(tracks).ToList();
            do {
                Tick(tracks, carts);
            } while(carts.Count > 1);
            var cart = carts.Last();
            System.Console.WriteLine($"{cart.X},{cart.Y}");
        }

        bool Tick(char[,] tracks, List<Cart> carts)
        {
            bool crash = false;
            foreach (var cart in carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList())
            {
                switch (cart.Direction)
                {
                    case Direction.Up:
                        cart.Y -= 1;
                        if (tracks[cart.Y, cart.X] == '/')
                        {
                            cart.Direction = Direction.Right;
                        }
                        else if (tracks[cart.Y, cart.X] == '\\')
                        {
                            cart.Direction = Direction.Left;
                        }
                        else if (tracks[cart.Y, cart.X] == '+')
                        {
                            DoTurn(cart);
                            switch (cart.LastTurn)
                            {
                                case Turn.Left:
                                    cart.Direction = Direction.Left;
                                    break;
                                case Turn.Straight:
                                    cart.Direction = Direction.Up;
                                    break;
                                case Turn.Right:
                                    cart.Direction = Direction.Right;
                                    break;
                            }
                        }
                        break;
                    case Direction.Down:
                        cart.Y += 1;
                        if (tracks[cart.Y, cart.X] == '/')
                        {
                            cart.Direction = Direction.Left;
                        }
                        else if (tracks[cart.Y, cart.X] == '\\')
                        {
                            cart.Direction = Direction.Right;
                        }
                        else if (tracks[cart.Y, cart.X] == '+')
                        {
                            DoTurn(cart);
                            switch (cart.LastTurn)
                            {
                                case Turn.Left:
                                    cart.Direction = Direction.Right;
                                    break;
                                case Turn.Straight:
                                    cart.Direction = Direction.Down;
                                    break;
                                case Turn.Right:
                                    cart.Direction = Direction.Left;
                                    break;
                            }
                        }
                        break;
                    case Direction.Left:
                        cart.X -= 1;
                        if (tracks[cart.Y, cart.X] == '/')
                        {
                            cart.Direction = Direction.Down;
                        }
                        else if (tracks[cart.Y, cart.X] == '\\')
                        {
                            cart.Direction = Direction.Up;
                        }
                        else if (tracks[cart.Y, cart.X] == '+')
                        {
                            DoTurn(cart);
                            switch (cart.LastTurn)
                            {
                                case Turn.Left:
                                    cart.Direction = Direction.Down;
                                    break;
                                case Turn.Straight:
                                    cart.Direction = Direction.Left;
                                    break;
                                case Turn.Right:
                                    cart.Direction = Direction.Up;
                                    break;
                            }
                        }
                        break;
                    case Direction.Right:
                        cart.X += 1;
                        if (tracks[cart.Y, cart.X] == '/')
                        {
                            cart.Direction = Direction.Up;
                        }
                        else if (tracks[cart.Y, cart.X] == '\\')
                        {
                            cart.Direction = Direction.Down;
                        }
                        else if (tracks[cart.Y, cart.X] == '+')
                        {
                            DoTurn(cart);
                            switch (cart.LastTurn)
                            {
                                case Turn.Left:
                                    cart.Direction = Direction.Up;
                                    break;
                                case Turn.Straight:
                                    cart.Direction = Direction.Right;
                                    break;
                                case Turn.Right:
                                    cart.Direction = Direction.Down;
                                    break;
                            }
                        }
                        break;
                }
                var crashCart = carts.Where(c => c != cart).FirstOrDefault(c => c.X == cart.X && c.Y == cart.Y);
                if (crashCart != null)
                {
                    System.Console.WriteLine($"{cart.X},{cart.Y}");
                    carts.Remove(cart);
                    carts.Remove(crashCart);
                    crash = true;
                }
            }
            return crash;
        }

        Turn DoTurn(Cart cart) => cart.LastTurn = (Turn)(((int)cart.LastTurn + 1) % 3);

        char[,] GetData()
        {
            var data = File.ReadAllLines("input");
            var result = new char[data.Length, data[0].Length];
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    result[i, j] = data[i][j];
                }
            }
            return result;
        }

        IEnumerable<Cart> GetCarts(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i, j])
                    {
                        case '>':
                            map[i, j] = '-';
                            yield return new Cart { X = j, Y = i, Direction = Direction.Right, LastTurn = Turn.Right };
                            break;
                        case '<':
                            map[i, j] = '-';
                            yield return new Cart { X = j, Y = i, Direction = Direction.Left, LastTurn = Turn.Right };
                            break;
                        case 'v':
                            map[i, j] = '|';
                            yield return new Cart { X = j, Y = i, Direction = Direction.Down, LastTurn = Turn.Right };
                            break;
                        case '^':
                            map[i, j] = '|';
                            yield return new Cart { X = j, Y = i, Direction = Direction.Up, LastTurn = Turn.Right };
                            break;
                    }
                }
            }
        }

    }

    public enum Turn
    {
        Left,
        Straight,
        Right
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Cart
    {

        public int X { get; set; }

        public int Y { get; set; }

        public Direction Direction { get; set; }

        public Turn LastTurn { get; set; }

    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AOC;

namespace Day17
{

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.SecondStar();
        }

        void FirstStar()
        {
            int columns, rows;
            char[,] array;
            GenerateMatrix(out columns, out rows, out array);

            var sum = 0;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (array[x, y] == '#')
                    {
                        if (x - 1 >= 0 && array[x - 1, y] == '#' &&
                        x + 1 < columns && array[x + 1, y] == '#' &&
                        y - 1 >= 0 && array[x, y - 1] == '#' &&
                        y + 1 < rows && array[x, y + 1] == '#')
                        {
                            sum += x * y;
                            Console.Write('O');
                        }
                        else
                        {
                            Console.Write(array[x, y]);
                        }
                    }
                    else
                    {
                        Console.Write(array[x, y]);
                    }


                }
            }

            System.Console.WriteLine(sum);
        }



        void SecondStar()
        {
            int columns, rows;
            char[,] array;
            GenerateMatrix(out columns, out rows, out array);
            var currentCoordinate = FindStart(array, columns, rows);
            var currentDirection = Direction.Up;
            var moves = new List<(char turn, int steps)>();
            var stepCounter = 0;
            var turn = 'L';
            var done = false;
            while (!done)
            {
                var nextCoordinate = (x: 0, y: 0);
                nextCoordinate = GetNextCoordinate(currentCoordinate, currentDirection);
                if (!CoordinateValid(columns, rows, array, nextCoordinate))
                {
                    if (stepCounter > 0)
                    {
                        moves.Add((turn, stepCounter));
                    }
                    var nextDirection = (Direction)(-1);
                    if (currentDirection == Direction.Up || currentDirection == Direction.Down)
                    {
                        foreach (var direction in new[] { Direction.Left, Direction.Right })
                        {
                            var newCoordinate = GetNextCoordinate(currentCoordinate, direction);
                            if (CoordinateValid(columns, rows, array, newCoordinate))
                            {
                                nextDirection = direction;
                            }
                        }
                    }
                    else
                    {
                        foreach (var direction in new[] { Direction.Up, Direction.Down })
                        {
                            var newCoordinate = GetNextCoordinate(currentCoordinate, direction);
                            if (CoordinateValid(columns, rows, array, newCoordinate))
                            {
                                nextDirection = direction;
                            }
                        }
                    }
                    if ((int)nextDirection != -1)
                    {
                        turn = GetTurn(currentDirection, nextDirection);
                        currentDirection = nextDirection;
                        stepCounter = 0;
                    }
                    else
                    {
                        done = true;
                    }
                }
                else
                {
                    stepCounter++;
                    currentCoordinate = nextCoordinate;
                }
            }
            System.Console.WriteLine(string.Join(",", moves.Select(m => $"{m.turn},{m.steps}")));

            var count = 0;
            var moveGroups = new List<List<(char turn, int steps)>>();
            while (moveGroups.Count < 3)
            {
                foreach (var group in moveGroups)
                {
                    if (Enumerable.Range(0, group.Count).All(j => group[j] == moves[count + j]))
                    {
                        count += group.Count;
                    }
                }
                var subList = new List<(char turn, int steps)>();
                var matches = 0;
                do
                {
                    subList.Add(moves[count]);
                    count++;
                    matches = 0;
                    for (int i = count; i < moves.Count - subList.Count; i++)
                    {
                        if (Enumerable.Range(0, subList.Count).All(j => subList[j] == moves[i + j]))
                        {
                            matches++;
                        }
                    }
                } while (matches > 0);
                var itemToRemove = subList.Last();
                subList.Remove(itemToRemove);
                count--;
                moveGroups.Add(subList);
            }



        }

        char GetTurn(Direction currentDirection, Direction nextDirection)
        {
            switch (currentDirection)
            {
                case Direction.Up:
                    return nextDirection == Direction.Left ? 'L' : 'R';
                case Direction.Down:
                    return nextDirection == Direction.Left ? 'R' : 'L';
                case Direction.Left:
                    return nextDirection == Direction.Up ? 'R' : 'L';
                case Direction.Right:
                    return nextDirection == Direction.Up ? 'L' : 'R';
            }
            throw new InvalidOperationException();
        }

        private static bool CoordinateValid(int columns, int rows, char[,] array, (int x, int y) coordinate) =>
            coordinate.x > -1 && coordinate.x < columns &&
            coordinate.y > -1 && coordinate.y < rows &&
            array[coordinate.x, coordinate.y] == '#';


        private static (int x, int y) GetNextCoordinate((int x, int y) currentCoordinate, Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.Up:
                    return (x: currentCoordinate.x, y: currentCoordinate.y - 1);
                case Direction.Down:
                    return (x: currentCoordinate.x, y: currentCoordinate.y + 1);
                case Direction.Left:
                    return (x: currentCoordinate.x - 1, y: currentCoordinate.y);
                case Direction.Right:
                    return (x: currentCoordinate.x + 1, y: currentCoordinate.y);
            }

            throw new InvalidOperationException();
        }

        private static (int x, int y) FindStart(char[,] array, int columns, int rows)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (array[x, y] == '^')
                    {
                        return (x, y);
                    }
                }
            }
            throw new InvalidOperationException();
        }

        private void GenerateMatrix(out int columns, out int rows, out char[,] array)
        {
            var chars = new List<char>();
            var ops = GetData();
            var computer = new IntComputer(
                ops,
                () => 0,
                c =>
                {
                    chars.Add((char)Convert.ToByte(c));
                });
            computer.RunProgram();

            columns = chars.IndexOf((char)10) + 1;
            rows = (chars.Count - 1) / (columns);
            array = new char[columns, rows];
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    array[x, y] = chars[columns * y + x];
                }
            }
        }



        List<long> GetData() =>
            File.ReadAllText("input.txt").Split(",").Select(i => long.Parse(i)).ToList();

    }
}

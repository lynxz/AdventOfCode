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

            var compressed = Compress(string.Join(",", moves.Select(m => $"{m.turn},{m.steps}")).Split(',').ToList());

            var input =
                compressed.Seq.Select(x => (long)x).Append(10)
                .Concat(compressed.A.Select(x => (long)x)).Append(10)
                .Concat(compressed.B.Select(x => (long)x)).Append(10)
                .Concat(compressed.C.Select(x => (long)x)).Append(10)
                .Append((long)'n').Append(10)
                .ToArray();

            var ops = GetData();
            ops[0] = 2;
            long dust = 0;
            var i = 0;

            var computer = new IntComputer(ops, () => input[i++], c => dust = c);
            computer.RunProgram();

            System.Console.WriteLine(dust);
        }

        (string A, string B, string C, string Seq) Compress(List<string> sequence)
        {
            var fullSequence = string.Join(',', sequence);
            for (int aLength = 10; aLength >= 2; aLength -= 2)
            {
                var aSeq = string.Join(',', sequence.Take(aLength));

                if (aSeq.Length > 20)
                    continue;

                int nextIndexA = aSeq.Length + 1;
                int aCount = 1;
                while (fullSequence.Substring(nextIndexA, aSeq.Length) == aSeq)
                {
                    nextIndexA += aSeq.Length + 1;
                    aCount++;
                }

                for (int bLength = 10; bLength >= 2; bLength -= 2)
                {
                    var bSeq = string.Join(',', sequence.Skip(aLength * aCount).Take(bLength));

                    if (bSeq.Length > 20)
                        continue;

                    int nextIndexB = nextIndexA + bSeq.Length + 1;
                    int bCount = 1;
                    int abCount = 0;
                    bool match = true;

                    while (match)
                    {
                        match = false;
                        if (fullSequence.Substring(nextIndexB, aSeq.Length) == aSeq)
                        {
                            match = true;
                            nextIndexB += aSeq.Length + 1;
                            abCount++;
                        }

                        if (fullSequence.Substring(nextIndexB, bSeq.Length) == bSeq)
                        {
                            match = true;
                            nextIndexB += bSeq.Length + 1;
                            bCount++;
                        }
                    }

                    for (int cLength = 10; cLength >= 2; cLength -= 2)
                    {
                        var cSeq = string.Join(',', sequence.Skip(aLength * (aCount + abCount) + bLength * bCount).Take(cLength));

                        if (cSeq.Length > 20)
                            continue;

                        var seq = GetSeq(aSeq, bSeq, cSeq, fullSequence);

                        if (seq != null)
                            return (aSeq, bSeq, cSeq, seq);
                    }
                }
            }

            throw new Exception();
        }

        private string GetSeq(string aSeq, string bSeq, string cSeq, string fullSequence)
        {
            var index = 0;
            var output = new List<string>();

            while (index < fullSequence.Length)
            {
                if (index + aSeq.Length <= fullSequence.Length && fullSequence.Substring(index, aSeq.Length) == aSeq)
                {
                    index += aSeq.Length + 1;
                    output.Add("A");
                }
                else if (index + bSeq.Length <= fullSequence.Length && fullSequence.Substring(index, bSeq.Length) == bSeq)
                {
                    index += bSeq.Length + 1;
                    output.Add("B");
                }
                else if (index + cSeq.Length <= fullSequence.Length && fullSequence.Substring(index, cSeq.Length) == cSeq)
                {
                    index += cSeq.Length + 1;
                    output.Add("C");
                }
                else
                {
                    return null;
                }
            }

            var returnString = string.Join(',', output);
            return returnString.Length <= 20 ? returnString : null;
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

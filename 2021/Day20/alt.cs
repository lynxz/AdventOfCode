using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2021
{
    public class Alt
    {
        class SparseMatrix2D<T>
        {
            public Dictionary<(int X, int Y), T> Map { get; private set; }

            public SparseMatrix2D()
            {
                Map = new Dictionary<(int, int), T>();
            }

            public T this[int x, int y]
            {
                get
                {
                    return Map.ContainsKey((x, y)) ? Map[(x, y)] : default(T);
                }
                set
                {
                    if (EqualityComparer<T>.Default.Equals(value, default(T)))
                        Map.Remove((x, y));
                    else
                        Map[(x, y)] = value;
                }
            }

            public T this[int x, int y, T defaultValue]
            {
                get
                {
                    return Map.ContainsKey((x, y)) ? Map[(x, y)] : defaultValue;
                }
                set
                {
                    if (EqualityComparer<T>.Default.Equals(value, defaultValue))
                        Map.Remove((x, y));
                    else
                        Map[(x, y)] = value;
                }
            }
        }

        public static void Day20(List<string> input)
        {
            StringBuilder algo = new StringBuilder();
            int line = 0;
            while (input[line] != "")
            {
                algo.Append(input[line]);
                line++;
            }
            bool[] algobit = new bool[512];
            for (int i = 0; i < 512; i++)
            {
                algobit[i] = algo[i] == '#';
            }
            line++;
            int width = input[line].Length;
            int height = input.Count - line;
            SparseMatrix2D<byte> image = new SparseMatrix2D<byte>();
            SparseMatrix2D<byte> image2 = new SparseMatrix2D<byte>();
            int sy = 0;
            while (input[line + sy] != "")
            {
                for (int i = 0; i < width; i++)
                {
                    image[i, sy] = input[line + sy][i] == '#' ? (byte)1 : (byte)0;
                }
                sy++;
            }
            ;
            bool swapBlackWhite = algobit[0];
            int steps = 0;
            Enhance(1);
            (int x1, int x2, int y1, int y2) = (image.Map.Keys.Min(s => s.X), image.Map.Keys.Max(s => s.X), image.Map.Keys.Min(s => s.Y), image.Map.Keys.Max(s => s.Y));
            for (int y = y1; y <= y2; y++)
            {
                for (int x = x1; x <= x2; x++)
                {
                    Console.Write(image[x,y] == 1 ? "#" : ".");
                }
                System.Console.WriteLine();
            }
            Console.WriteLine($"{image.Map.Count}");
            // Enhance(50);
            // Console.WriteLine($"{image.Map.Count}");

            void Enhance(int to)
            {
                for (; steps < to; steps++)
                {
                    (int x1, int x2, int y1, int y2) = (image.Map.Keys.Min(s => s.X), image.Map.Keys.Max(s => s.X), image.Map.Keys.Min(s => s.Y), image.Map.Keys.Max(s => s.Y));
                    for (int y = y1 - 1; y <= y2 + 1; y++)
                    {
                        for (int x = x1 - 1; x <= x2 + 1; x++)
                        {
                            if (swapBlackWhite)
                            {
                                byte next = algobit[Address(x, y, (byte)(steps % 2))] ? (byte)1 : (byte)0;
                                image2[x, y, (byte)((steps + 1) % 2)] = next;
                            }
                            else
                            {
                                byte next = algobit[Address(x, y)] ? (byte)1 : (byte)0;
                                image2[x, y] = next;
                            }
                        }
                    }
                    (image, image2) = (image2, image);
                    image2.Map.Clear();
                }



            }

            int Address(int x, int y, byte defaultValue = 0)
            {
                int addr = 0;
                for (int j = -1; j <= 1; j++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        addr = (addr << 1) | image[x + i, y + j, defaultValue];
                    }
                }
                return addr;
            }
        }
    }
}
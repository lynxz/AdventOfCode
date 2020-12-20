using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day20
{

    enum Placement
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3
    }

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
            var data = GetPicture();
            var result = data.Where(t => t.Neighbors.Count(n => n == null) == 2).Aggregate(1UL, (acc, t) => acc * Convert.ToUInt64(t.Number));
            System.Console.WriteLine(result);
        }

        private List<Tile> GetPicture()
        {
            var data = GetData();
            for (int i = 0; i < data.Count - 1; i++)
            {
                for (int r = 0; r < 4; r++)
                {
                    if (data[i].Neighbors[(int)Placement.Top] != null)
                    {
                        data[i].Rotate();
                        continue;
                    }

                    for (int j = i + 1; j < data.Count; j++)
                    {
                        if (data[j].Neighbors.All(n => n != null) || data[i].Neighbors.Any(n => n == data[j]))
                            continue;

                        if (data[i].Connect(data[j]))
                        {
                            if (data[i].Neighbors[(int)Placement.Top] != null)
                                throw new Exception();
                            data[i].Neighbors[(int)Placement.Top] = data[j];
                            if (data[j].Neighbors[(int)Placement.Bottom] != null)
                                throw new Exception();
                            data[j].Neighbors[(int)Placement.Bottom] = data[i];
                            break;

                        }
                    }
                    data[i].Rotate();
                }
            }

            return data;
        }

        private void SecondStar()
        {
            var data = GetPicture();
            var upperLeft = data.Single(d => d.Neighbors[(int)Placement.Left] == null && d.Neighbors[(int)Placement.Top] == null);
            var image = new List<string>();
            var tile = upperLeft;
            while (tile != null)
            {
                for (int i = 1; i < 9; i++)
                {
                    StringBuilder sb = new();
                    var currentTile = tile;
                    while (currentTile != null)
                    {
                        for (int j = 1; j < 9; j++)
                        {
                            sb.Append(currentTile.Image[i, j]);
                        }
                        currentTile = currentTile.Neighbors[(int)Placement.Right];
                    }
                    image.Add(sb.ToString());
                }
                tile = tile.Neighbors[(int)Placement.Bottom];
            }

            image = FlipVertically(image);
            image = Rotate(image);
            image = Rotate(image);
            image = Rotate(image);

            var head = "                  # ";
            var start = head.IndexOf("#");
            var length = head.Length;
            var body = new Regex("^#....##....##....###$");
            var belly = new Regex("^.#..#..#..#..#..#...$");
            var monsterCounter = 0;
            for (int i = 0; i < image.Count - 2; i++)
                for (int j = start; j < image[0].Length - 1; j++)
                {
                    if (image[i][j] == '#')
                    {
                        if (body.Match(image[i + 1].Substring(j - start, length)).Success &&
                        belly.Match(image[i + 2].Substring(j - start, length)).Success)
                        {
                            monsterCounter++;
                        }

                    }
                }
                
            System.Console.WriteLine(image.Sum(l => l.Where(c => c == '#').Count()) - monsterCounter * 15);
        }

        List<string> Rotate(List<string> image)
        {
            var width = image[0].Length;
            var newImage = new char[image.Count, width];
            for (int i = 0; i < image.Count; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    newImage[j, width - 1 - i] = image[i][j];
                }
            }
            return ToStrings(newImage).ToList();
        }

        List<string> FlipVertically(List<string> image)
        {
            var width = image[0].Length;
            var newImage = new char[image.Count, width];
            for (int i = 0; i < image.Count; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    newImage[i, j] = image[width - 1 - i][j];
                }
            }
            return ToStrings(newImage).ToList();
        }

        IEnumerable<string> ToStrings(char[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                StringBuilder sb = new();
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    sb.Append(data[i, j]);
                }
                yield return sb.ToString();
            }
        }

        List<Tile> GetData()
        {
            List<Tile> tiles = new();
            var data = File.ReadAllText("data.txt").Split("\r\n\r\n");
            foreach (var block in data)
            {
                var lines = block.Split(Environment.NewLine);
                var number = int.Parse(Regex.Match(lines[0], @"\d+").Value);
                var image = new char[10, 10];
                for (int i = 1; i < 11; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        image[i - 1, j] = lines[i][j];
                    }
                }
                tiles.Add(new Tile(number, image));
            }
            return tiles;
        }

        void Print(Tile tile)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    System.Console.Write(tile.Image[i, j]);
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
        }

        void PrintAll(Tile tile)
        {
            while (tile != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    var tilePrinting = tile;
                    while (tilePrinting != null)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            System.Console.Write(tilePrinting.Image[i, j]);
                        }
                        System.Console.Write(" ");
                        tilePrinting = tilePrinting.Neighbors[(int)Placement.Right];
                    }
                    System.Console.WriteLine();
                }
                System.Console.WriteLine();
                tile = tile.Neighbors[(int)Placement.Bottom];
            }
        }

    }

    class Tile
    {

        public Tile(int number, char[,] image)
        {
            Number = number;
            Image = image;
            Neighbors = new Tile[4];
        }

        public int Number { get; private set; }
        public char[,] Image { get; private set; }

        public Tile[] Neighbors { get; private set; }

        public Tile Rotate()
        {
            InternalRotate();
            var connected = new List<Tile>();
            GetConnected(connected);
            foreach (var connectedTile in connected)
            {
                if (connectedTile != this)
                    connectedTile.InternalRotate();
            }
            return this;
        }

        private void InternalRotate()
        {
            var newImage = new char[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    newImage[j, 9 - i] = Image[i, j];
                }
            }
            var tmp = Neighbors[(int)Placement.Top];
            Neighbors[(int)Placement.Top] = Neighbors[(int)Placement.Left];
            Neighbors[(int)Placement.Left] = Neighbors[(int)Placement.Bottom];
            Neighbors[(int)Placement.Bottom] = Neighbors[(int)Placement.Right];
            Neighbors[(int)Placement.Right] = tmp;
            Image = newImage;
        }

        public Tile FlipHorizontally()
        {
            InternalFlipHorizontally();
            var connected = new List<Tile>();
            GetConnected(connected);
            foreach (var connectedTile in connected)
            {
                if (connectedTile != this)
                    connectedTile.InternalFlipHorizontally();
            }
            return this;
        }

        private void InternalFlipHorizontally()
        {
            var newImage = new char[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    newImage[i, j] = Image[i, 9 - j];
                }
            }
            Image = newImage;
            var tmp = Neighbors[(int)Placement.Left];
            Neighbors[(int)Placement.Left] = Neighbors[(int)Placement.Right];
            Neighbors[(int)Placement.Right] = tmp;
        }

        public Tile FlipVertically()
        {
            InternalFlipVertically();
            var connected = new List<Tile>();
            GetConnected(connected);
            foreach (var connectedTile in connected)
            {
                if (connectedTile != this)
                    connectedTile.InternalFlipVertically();
            }
            return this;
        }

        private void InternalFlipVertically()
        {
            var newImage = new char[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    newImage[i, j] = Image[9 - i, j];
                }
            }
            Image = newImage;
            var tmp = Neighbors[(int)Placement.Top];
            Neighbors[(int)Placement.Top] = Neighbors[(int)Placement.Bottom];
            Neighbors[(int)Placement.Bottom] = tmp;
        }

        public void GetConnected(List<Tile> tiles)
        {
            foreach (var neighbor in Neighbors.Where(n => n != null))
            {
                if (!tiles.Contains(neighbor))
                {
                    tiles.Add(neighbor);
                    neighbor.GetConnected(tiles);
                }
            }
        }

        public bool Connect(Tile tile)
        {
            var currentTop = new String(Enumerable.Range(0, 10).Select(i => Image[0, i]).ToArray());
            var tileBottom = new String(Enumerable.Range(0, 10).Select(i => tile.Image[9, i]).ToArray());
            var tileTop = new String(Enumerable.Range(0, 10).Select(i => tile.Image[0, i]).ToArray());
            var tileLeft = new String(Enumerable.Range(0, 10).Select(i => tile.Image[i, 0]).ToArray());
            var tileRight = new String(Enumerable.Range(0, 10).Select(i => tile.Image[i, 9]).ToArray());

            if (currentTop == tileBottom)
                return true;
            if (currentTop == tileTop)
            {
                tile.FlipVertically();
                return true;
            }
            if (currentTop == tileLeft)
            {
                tile.Rotate().Rotate().Rotate();
                return true;
            }
            if (currentTop == tileRight)
            {
                tile.FlipVertically().Rotate();
                return true;
            }
            if (currentTop == new String(tileBottom.Reverse().ToArray()))
            {
                tile.FlipHorizontally();
                return true;
            }
            if (currentTop == new String(tileTop.Reverse().ToArray()))
            {
                tile.FlipHorizontally().FlipVertically();
                return true;
            }
            if (currentTop == new String(tileLeft.Reverse().ToArray()))
            {
                tile.FlipVertically().Rotate().Rotate().Rotate();
                return true;
            }
            if (currentTop == new String(tileRight.Reverse().ToArray()))
            {
                tile.Rotate();
                return true;
            }

            return false;
        }

    }

}

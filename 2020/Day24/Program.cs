using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
            prog.FirstStar();
            prog.SecondStar();
        }

        void FirstStar()
        {
            var tiles = GetTiles();
            System.Console.WriteLine(tiles.Count(kvp => !kvp.Value));
        }

        void SecondStar()
        {
            var tiles = GetTiles();
            for (int i = 0; i < 100; i++)
            {
                foreach (var coord in tiles.Where(kvp => !kvp.Value).Select(kvp => kvp.Key).ToList())
                    foreach (var neighbor in GetNeighbors(coord))
                        if (!tiles.ContainsKey(neighbor))
                            tiles.Add(neighbor, true);

                var newTiles = new Dictionary<(int x, int y, int z), bool>();
                foreach (var tile in tiles)
                {
                    var blackNeighbors = GetNeighbors(tile.Key).Count(n => tiles.ContainsKey(n) ? !tiles[n] : false);
                    
                    if (!tile.Value && (blackNeighbors == 0 || blackNeighbors > 2))
                        newTiles.Add(tile.Key, true);
                    else if (tile.Value && blackNeighbors == 2)
                        newTiles.Add(tile.Key, false);
                    else
                        newTiles.Add(tile.Key, tile.Value);
                }
                tiles = newTiles;
            }
            System.Console.WriteLine(tiles.Count(kvp => !kvp.Value));
        }

        IEnumerable<(int x, int y, int z)> GetNeighbors((int x, int y, int z) coord)
        {
            yield return (coord.x - 1, coord.y, coord.z + 1);
            yield return (coord.x, coord.y - 1, coord.z + 1);
            yield return (coord.x, coord.y + 1, coord.z - 1);
            yield return (coord.x + 1, coord.y, coord.z - 1);
            yield return (coord.x - 1, coord.y + 1, coord.z);
            yield return (coord.x + 1, coord.y - 1, coord.z);
        }

        private Dictionary<(int, int, int), bool> GetTiles()
        {
            var data = GetData();
            var dict = new Dictionary<(int, int, int), bool>();
            var regex = new Regex("se|ne|nw|sw|w|e");
            foreach (var l in data)
            {
                var coord = (x: 0, y: 0, z: 0);
                var matches = regex.Matches(l);
                foreach (Match match in matches)
                {
                    (int x, int y, int z) result = match.Value switch
                    {
                        "se" => (-1, 0, 1),
                        "sw" => (0, -1, 1),
                        "ne" => (0, 1, -1),
                        "nw" => (1, 0, -1),
                        "e" => (-1, 1, 0),
                        "w" => (1, -1, 0)
                    };
                    coord.x += result.x;
                    coord.y += result.y;
                    coord.z += result.z;
                }
                if (!dict.ContainsKey(coord))
                    dict.Add(coord, true);

                dict[coord] = !dict[coord];
            }

            return dict;
        }

        List<string> GetData() => File.ReadAllLines("data.txt").ToList();
    }

}

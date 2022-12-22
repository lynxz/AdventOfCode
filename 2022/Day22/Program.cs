using System.Diagnostics;
using Tools;

Day22 day = new();
day.OutputSecondStar();

public class Day22 : DayBase
{
    public Day22() : base("22")
    {

    }

    public override string FirstStar()
    {
        var data = GetRawData();
        var parts = data.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var map = parts[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Select(c => c).ToArray()).ToArray();
        var dir = parts[1];
        var offset = 0;
        var h = 0;
        var heading = new[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
        var d = new[] { '>', 'v', '<', '^' };
        var pos = (Y: 0, X: 0);

        pos.X = Enumerable.Range(0, map[0].Length).First(i => map[0][i] == '.');


        while (offset < dir.Length - 1)
        {
            var i = 1;
            while (char.IsNumber(dir[offset + i]))
                i++;
            var steps = int.Parse(dir[offset..(offset + i)]);
            offset += i;

            for (int s = 0; s < steps; s++)
            {
                var nextY = pos.Y + heading[h, 0];
                if (nextY < 0)
                {
                    nextY = Enumerable.Range(1, map.GetLength(0)).Select(i => map.GetLength(0) - i).First(y => pos.X < map[y].Length && map[y][pos.X] != ' ');
                }
                else if (nextY >= map.GetLength(0) || (map[nextY].Length <= pos.X && h == 1))
                {
                    nextY = Enumerable.Range(0, map.GetLength(0)).First(i => pos.X < map[i].Length && map[i][pos.X] != ' ');
                }
                else if (nextY >= map.GetLength(0) || (map[nextY].Length <= pos.X && h == 3))
                {
                    nextY = Enumerable.Range(1, map.GetLength(0)).Select(i => map.GetLength(0) - i).First(y => pos.X < map[y].Length && map[y][pos.X] != ' ');
                }
                else if (map[nextY][pos.X] == ' ' && h == 1)
                {
                    nextY = Enumerable.Range(0, map.GetLength(0)).First(i => pos.X < map[i].Length && map[i][pos.X] != ' ');
                }
                else if (map[nextY][pos.X] == ' ' && h == 3)
                {
                    nextY = Enumerable.Range(1, map.GetLength(0)).Select(i => map.GetLength(0) - i).First(y => pos.X < map[y].Length && map[y][pos.X] != ' ');
                }

                var nextX = pos.X + heading[h, 1];
                if (nextX < 0)
                {
                    nextX = map[pos.Y].Length - 1;
                }
                else if (nextX >= map[pos.Y].Length)
                {
                    nextX = Enumerable.Range(0, map[pos.Y].Length).First(i => map[pos.Y][i] != ' ');
                }
                else if (map[pos.Y][nextX] == ' ')
                {
                    nextX = map[pos.Y].Length - 1;
                }

                if (map[nextY][nextX] == '.')
                    pos = (Y: nextY, X: nextX);
            }

            if (offset < dir.Length - 1)
            {
                var turn = dir[offset..(offset + 1)];
                offset++;
                h += turn == "L" ? -1 : 1;
                h = h < 0 ? 3 : h % 4;
            }
        }

        var result = ((pos.Y + 1) * 1000) + ((pos.X + 1) * 4) + h;

        return result.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData();
        var parts = data.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var map = parts[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Select(c => c).ToArray()).ToArray();
        var dir = parts[1];
        var offset = 0;
        var h = 0;
        var heading = new[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
        var d = new[] { '>', 'v', '<', '^' };
        var pos = (Y: 0, X: 0);

        pos.X = Enumerable.Range(0, map[0].Length).First(i => map[0][i] == '.');

        while (offset < dir.Length - 1)
        {
            var i = 1;
            while (char.IsNumber(dir[offset + i]))
                i++;
            var steps = int.Parse(dir[offset..(offset + i)]);
            offset += i;

            for (int s = 0; s < steps; s++)
            {
                var nextY = pos.Y + heading[h, 0];
                var nextX = pos.X + heading[h, 1];
                var nextH = 0;

                (nextY, nextX, nextH) = (Y: nextY, X: nextX, H: h) switch
                {
                    ( < 0, >= 50, 3) and (_, < 100, _) => (100 + pos.X, 0, 0),
                    ( >= 150, < 0, 2) => (0, pos.Y - 100, 1),
                    ( < 0, >= 100, 3) => (199, pos.X - 100, 3),
                    (200, >= 0, 1) and (_, < 50, _) => (0, 100 + pos.X, 1),
                    (50, >= 100, 1) => (pos.X - 50, 99, 2),
                    ( >= 50, 100, 0) and ( < 100, _, _) => (49, pos.Y + 50, 3),
                    ( < 50, 49, 2) => (149 - pos.Y, 0, 0),
                    ( >= 100, < 0, 2) and ( < 150, _, _) => (149 - pos.Y, 50, 0),
                    ( >= 50, 49, 2) and ( < 100, _, _) => (100, pos.Y - 50, 1),
                    (99, >= 0, 3) and (_, < 50, _) => (pos.X + 50, 50, 0),
                    (150, >= 50, 1) => (pos.X+ 100, 49, 2),
                    ( >= 150, 50, 0) => (149, pos.Y - 100, 3),
                    ( >= 0, 150, 0) and ( < 50, _, _) => (149 - pos.Y, 99, 2),
                    ( >= 100, 100, 0) and ( < 150, _, _) => (149 - pos.Y, 149, 2),
                    _ => (nextY, nextX, h)
                };

                if (map[nextY][nextX] == '.')
                {
                    pos = (Y: nextY, X: nextX);
                    h = nextH;
                }
            }

            if (offset < dir.Length - 1)
            {
                var turn = dir[offset..(offset + 1)];
                offset++;
                h += turn == "L" ? -1 : 1;
                h = h < 0 ? 3 : h % 4;
            }
        }

        return (((pos.Y + 1) * 1000) + ((pos.X + 1) * 4) + h).ToString();
    }
}
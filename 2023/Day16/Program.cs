using Tools;

Day16 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day16 : DayBase
{
    public Day16() : base("16")
    {
    }

    public override string FirstStar()
    {
        var map = GetRowData().ToMultidimensionalArray();
        return LightPath(map, (0, -1, (0, 1))).ToString();
    }

    public override string SecondStar()
    {
        var map = GetRowData().ToMultidimensionalArray();
        List<int> heatedTiles = new();
        heatedTiles.AddRange(Enumerable.Range(0, map.GetLength(0)).Select(y => LightPath(map, (y, -1, (0, 1)))));
        heatedTiles.AddRange(Enumerable.Range(0, map.GetLength(0)).Select(y => LightPath(map, (y, map.GetLength(1), (0, -1)))));
        heatedTiles.AddRange(Enumerable.Range(0, map.GetLength(1)).Select(x => LightPath(map, (-1, x, (1, 0)))));
        heatedTiles.AddRange(Enumerable.Range(0, map.GetLength(1)).Select(x => LightPath(map, (map.GetLength(0), x, (-1, 0)))));
        return heatedTiles.Max().ToString();
    }

    int LightPath(char[,] map, (int y, int x, (int y, int x) dir) start)
    {
        var result = Enumerable.Range(0, map.GetLength(0)).Select(y => Enumerable.Range(0, map.GetLength(1)).Select(x => new List<(int, int)>())).ToMultidimensionalArray();
        Stack<(int y, int x, (int y, int x) dir)> stack = new();
        stack.Push(start);

        while (stack.Count > 0)
        {
            var (y, x, dir) = stack.Pop();
            var hitWall = false;

            while (!hitWall)
            {
                var newY = y + dir.y;
                var newX = x + dir.x;
                if (newY < 0 || newY >= map.GetLength(0) || newX < 0 || newX >= map.GetLength(1) || result[newY, newX].Contains(dir))
                    hitWall = true;

                if (!hitWall)
                {
                    result[newY, newX].Add(dir);

                    if (map[newY, newX] == '/')
                    {
                        dir = (dir.x * -1, dir.y * -1);
                    }
                    else if (map[newY, newX] == '\\')
                    {
                        dir = (dir.x, dir.y);
                    }
                    else if (map[newY, newX] == '|')
                    {
                        if (dir.x != 0)
                        {
                            stack.Push((newY, newX, (1, 0)));
                            stack.Push((newY, newX, (-1, 0)));
                            hitWall = true;
                        }
                    }
                    else if (map[newY, newX] == '-')
                    {
                        if (dir.y != 0)
                        {
                            stack.Push((newY, newX, (0, 1)));
                            stack.Push((newY, newX, (0, -1)));
                            hitWall = true;
                        }
                    }
                    x = newX;
                    y = newY;
                }
            }
        }

        return Enumerable.Range(0, map.GetLength(0)).Sum(y => Enumerable.Range(0, map.GetLength(1)).Count(x => result[y, x].Count > 0));
    }
}
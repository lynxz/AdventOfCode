using Tools;

var day = new Day06();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day06 : DayBase
{
    public Day06() : base("6")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var yMax = data.Length;
        var xMax = data[0].Length;

        var obst = Enumerable.Range(0, yMax).SelectMany(y => Enumerable.Range(0, xMax).Where(x => data[y][x] != '.').Select(x => (x, y))).ToList();
        var pos = obst.First(c => data[c.y][c.x] != '#');
        obst.Remove(pos);
        var dir = GetDir(data, pos);
        Walk(yMax, xMax, obst, pos, dir, out var hashTable);

        return hashTable.Count.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var yMax = data.Length;
        var xMax = data[0].Length;

        var obst = Enumerable.Range(0, yMax).SelectMany(y => Enumerable.Range(0, xMax).Where(x => data[y][x] != '.').Select(x => (x, y))).ToList();
        var start = obst.First(c => data[c.y][c.x] != '#');
        //var pos = start;
        obst.Remove(start);

        var dir = GetDir(data, start);
        Walk(yMax, xMax, obst, start, dir, out var coords);

        var loopCount = 0;
        foreach (var coord in coords)
        {
            if (coord == start)
            {
                continue;
            }
            var newObst = new List<(int x, int y)>(obst)
                {
                    coord
                };
            if (Walk(yMax, xMax, newObst, start, dir, out _))
            {
                loopCount++;
            }
        }

        return loopCount.ToString();
    }

    private static (int x, int y) GetDir(string[] data, (int x, int y) pos)
    {
        return data[pos.y][pos.x] switch
        {
            '^' => (0, -1),
            'v' => (0, 1),
            '<' => (-1, 0),
            '>' => (1, 0),
            _ => throw new Exception("Invalid start")
        };
    }

    private static bool Walk(int yMax, int xMax, List<(int x, int y)> obst, (int x, int y) pos, (int x, int y) dir, out HashSet<(int x, int y)> hashTable)
    {
        hashTable = new HashSet<(int x, int y)>();
        var loopTable = new HashSet<(int x, int y, int dx, int dy)>();

        while (pos.x > 0 && pos.x < xMax && pos.y > 0 && pos.y < yMax)
        {
            hashTable.Add(pos);
            loopTable.Add((pos.x, pos.y, dir.x, dir.y));
            var nextPos = (pos.x + dir.x, pos.y + dir.y);
            while (obst.Contains(nextPos))
            {
                dir = (x: -dir.y, y: dir.x);
                nextPos = (pos.x + dir.x, pos.y + dir.y);
            }
            pos = nextPos;
            if (loopTable.Contains((pos.x, pos.y, dir.x, dir.y)))
            {
                return true;
            }
        }
        return false;
    }

}
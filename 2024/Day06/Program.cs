using System.Collections;
using Tools;

var day = new Day06();
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

        (int x, int y) dir = data[pos.y][pos.x] switch
        {
            '^' => (0, -1),
            'v' => (0, 1),
            '<' => (-1, 0),
            '>' => (1, 0),
            _ => throw new Exception("Invalid start")
        };

        var hashTable = new HashSet<(int x, int y)>();


        while (pos.x > 0 && pos.x < xMax && pos.y > 0 && pos.y < yMax)
        {
            hashTable.Add(pos);
            var nextPos = (pos.x + dir.x, pos.y + dir.y);
            while (obst.Contains(nextPos))
            {
                var newDir = (x: -dir.y, y: dir.x);
                nextPos = (pos.x + newDir.x, pos.y + newDir.y);
                dir = newDir;
            }
            pos = nextPos;
        }

        return hashTable.Count.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var yMax = data.Length;
        var xMax = data[0].Length;

        var obst = Enumerable.Range(0, yMax).SelectMany(y => Enumerable.Range(0, xMax).Where(x => data[y][x] != '.').Select(x => (x, y))).ToList();
        var start = obst.First(c => data[c.y][c.x] != '#');
        var pos = start;
        obst.Remove(pos);

        (int x, int y) ogDir = data[pos.y][pos.x] switch
        {
            '^' => (0, -1),
            'v' => (0, 1),
            '<' => (-1, 0),
            '>' => (1, 0),
            _ => throw new Exception("Invalid start")
        };

        var loopCount = 0;
        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                pos = start;
                var dir = ogDir;
                var hashTable = new HashSet<(int x, int y, int dx, int dy)>();
                System.Console.Write(".");
                if (obst.Contains((x, y)) || (x, y) == start)
                {
                    continue;
                }
                var newObst = new List<(int x, int y)>(obst)
                {
                    (x, y)
                };
                while (pos.x > 0 && pos.x < xMax && pos.y > 0 && pos.y < yMax)
                {
                    hashTable.Add((pos.x, pos.y, dir.x, dir.y));
                    var nextPos = (pos.x + dir.x, pos.y + dir.y);
                    while (newObst.Contains(nextPos))
                    {
                        var newDir = (x: -dir.y, y: dir.x);
                        nextPos = (pos.x + newDir.x, pos.y + newDir.y);
                        dir = newDir;
                    }
                    pos = nextPos;
                    if (hashTable.Contains((pos.x, pos.y, dir.x, dir.y)))
                    {
                        loopCount++;
                        break;
                    }
                }
            }
            System.Console.WriteLine();
        }


        return loopCount.ToString();
    }
}
using Tools;

Day18 day = new Day18();
day.OutputSecondStar();

public class Day18 : DayBase
{
    public Day18() : base("18")
    {
    }


    public override string FirstStar()
    {
        var data = GetRowData();
        var map = Enumerable.Range(0, 500).Select(x => Enumerable.Range(0, 500).Select(y => '.')).ToMultidimensionalArray();
        var directions = new Dictionary<char, (int, int)> {
            { 'U', (-1, 0) },
            { 'R', (0, 1) },
            { 'D', (1, 0) },
            { 'L', (0, -1) }
        };

        var startX = 400;
        var startY = 200;

        map[startY, startX] = '#';

        foreach (var row in data)
        {
            var direction = row[0];
            var distance = row.GetIntegers().First();

            var (dx, dy) = directions[direction];

            for (int i = 0; i < distance; i++)
            {
                startX += dx;
                startY += dy;

                map[startY, startX] = '#';
            }
        }

        var stack = new Stack<(int, int)>();
        stack.Push((250, 250));

        while (stack.Count > 0)
        {
            var (x, y) = stack.Pop();

            if (map[y, x] == '#')
            {
                continue;
            }

            map[y, x] = '#';

            foreach (var (dx, dy) in directions.Values)
            {
                var newX = x + dx;
                var newY = y + dy;

                if (newX < 0 || newX >= 500 || newY < 0 || newY >= 500)
                {
                    continue;
                }

                if (map[newY, newX] == '#')
                {
                    continue;
                }

                stack.Push((newX, newY));
            }
        }

        return Enumerable.Range(0, 500).Sum(y => Enumerable.Range(0, 500).Count(x => map[y, x] == '#')).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var verticalRanges = new List<(int, int)>();
        var horizontalRanges = new List<(int, int)>();
        var directions = new Dictionary<char, (int, int)> {
            { '3', (-1, 0) },
            { '0', (0, 1) },
            { '1', (1, 0) },
            { '2', (0, -1) }
        };

        var currentX = 0;
        var currentY = 0;
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        foreach (var row in data)
        {
            var d = row.Split('#').Last()[..^1];
            var hex = d[..5];
            var direction = d.Last();
            var distance = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            var (dy, dx) = directions[direction];
            if (direction == '0' || direction == '2')
            {
                verticalRanges.Add((currentX + dx, currentX + dx * distance));
                currentX += dx * distance +dx;
                if (currentX < minX)
                {
                    minX = currentX;
                }
                if (currentX > maxX)
                {
                    maxX = currentX;
                }
            }
            else
            {
                horizontalRanges.Add((currentY + dy, currentY + dy * distance));
                currentY += dy * distance + dy;
                if (currentY < minY)
                {
                    minY = currentY;
                }
                if (currentY > maxY)
                {
                    maxY = currentY;
                }
            }
        }

        return "";

    }
}
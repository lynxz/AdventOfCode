using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Tools;

var day = new Day15();
day.OutputSecondStar();

public class Day15 : DayBase
{
    public Day15() : base("15")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData().Replace("\r", "").Split("\n\n");
        var map = data[0].Split("\n").Select(x => x.Trim().ToCharArray()).ToArray();
        var steps = data[1].Replace("\n", "");

        var maxY = map.Length;
        var maxX = map[0].Length;

        var robot = Enumerable.Range(0, maxY)
            .SelectMany(y => Enumerable.Range(0, maxX).Select(x => (x, y)))
            .Where(p => map[p.y][p.x] == '@')
            .First();

        foreach (var step in steps)
        {
            var (x, y) = robot;
            var (dx, dy) = step switch
            {
                '^' => (0, -1),
                'v' => (0, 1),
                '<' => (-1, 0),
                '>' => (1, 0),
                _ => (0, 0)
            };
            var nx = x + dx;
            var ny = y + dy;

            var stack = new Stack<(int x, int y)>();
            while (map[ny][nx] != '#' && map[ny][nx] != '.')
            {
                stack.Push((nx, ny));
                nx += dx;
                ny += dy;
            }

            if (map[ny][nx] == '.')
            {
                var (tx, ty) = (0, 0);
                while (stack.Count > 0)
                {
                    (tx, ty) = stack.Pop();
                    map[ny][nx] = map[ty][tx];
                    ny = ty;
                    nx = tx;
                }
                map[robot.y][robot.x] = '.';
                robot = (x: nx, y: ny);
                map[ny][nx] = '@';
            }
        }

        var sum = Enumerable.Range(0, maxY)
            .SelectMany(y => Enumerable.Range(0, maxX).Where(x => map[y][x] == 'O').Select(x => (x, y)))
            .Sum(p => p.y * 100 + p.x);

        return sum.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData().Replace("\r", "").Split("\n\n");
        var map = data[0]
            .Split("\n")
            .Select(x =>
                x.Trim()
                .SelectMany(c => c switch
                {
                    '#' => ['#', '#'],
                    '.' => ['.', '.'],
                    '@' => ['@', '.'],
                    'O' => ['[', ']'],
                    _ => new[] { c }
                })
                .ToArray()
            ).ToMultidimensionalArray()!;
        var steps = data[1].Replace("\n", "");

        var maxY = map.GetLength(0);
        var maxX = map.GetLength(1);

        var robot = Enumerable.Range(0, maxY)
            .SelectMany(y => Enumerable.Range(0, maxX).Select(x => (x, y)))
            .Where(p => map[p.y, p.x] == '@')
            .First();

        // map.Print();
        // System.Console.WriteLine();

        foreach (var step in steps)
        {
            var (x, y) = robot;
            var (dx, dy) = step switch
            {
                '^' => (0, -1),
                'v' => (0, 1),
                '<' => (-1, 0),
                '>' => (1, 0),
                _ => (0, 0)
            };
            var nx = x + dx;
            var ny = y + dy;

            var stack = new Stack<(int ox, int oy, int nx, int ny)>();
            var canMove = dy == 0 ?
                HorizontalPush(x, y, dx, dy, map, stack) :
                VerticalPush(x, y, dx, dy, map, stack);

            if (canMove)
            {
                while (stack.Count > 0)
                {
                    var (ox, oy, tx, ty) = stack.Pop();
                    map[ty, tx] = map[oy, ox];
                    map[oy, ox] = '.';
                    if (map[ty, tx] == '@')
                    {
                        robot = (x: tx, y: ty);
                    }
                }
            }

            // System.Console.WriteLine($"Move: {step}:");
            // map.Print();
            // System.Console.WriteLine();
        }



        var sum = Enumerable.Range(0, maxY)
            .SelectMany(y => Enumerable.Range(0, maxX).Where(x => map[y, x] == '[').Select(x => (x, y)))
            .Sum(p => p.y * 100 + p.x);

        return sum.ToString();
    }

    private bool HorizontalPush(int x, int y, int dx, int dy, char[,] map, Stack<(int ox, int oy, int nx, int ny)> stack)
    {
        var nx = x + dx;
        var ny = y + dy;
        var ox = x;
        var oy = y;

        if (stack.Contains((x, y, nx, ny)))
        {
            return true;
        }

        while (map[ny, nx] != '#' && map[ny, nx] != '.')
        {
            stack.Push((ox, oy, nx, ny));
            ox = nx;
            oy = ny;
            nx += dx;
            ny += dy;
        }

        stack.Push((ox, oy, nx, ny));
        return map[ny, nx] == '.';
    }

    private bool VerticalPush(int x, int y, int dx, int dy, char[,] map, Stack<(int ox, int oy, int nx, int ny)> stack)
    {
        var searchQueue = new Queue<(int x, int y)>();
        searchQueue.Enqueue((x, y));

        while (searchQueue.Count > 0)
        {
            var (cx, cy) = searchQueue.Dequeue();
            var nx = cx + dx;
            var ny = cy + dy;

            if (!stack.Contains((cx, cy, nx, ny)))
            {
                stack.Push((cx, cy, nx, ny));
            }

            if (map[ny, nx] == '#')
            {
                return false;
            }

            if (map[ny, nx] == '[')
            {
                searchQueue.Enqueue((nx, ny));
                searchQueue.Enqueue((nx + 1, ny));
            }
            if (map[ny, nx] == ']')
            {
                searchQueue.Enqueue((nx, ny));
                searchQueue.Enqueue((nx - 1, ny));
            }
        }

        return true;

        // var nx = x + dx;
        // var ny = y + dy;

        // if (stack.Contains((x, y, nx, ny)))
        // {
        //     return true;
        // }

        // stack.Push((x, y, nx, ny));

        // if (map[ny, nx] == '.')
        // {
        //     return true;
        // }
        // if (map[ny, nx] == '#')
        // {
        //     return false;
        // }

        // if (map[ny, nx] == '[')
        // {
        //     return VerticalPush(nx, ny, dx, dy, map, stack)
        //         && VerticalPush(nx + 1, ny, dx, dy, map, stack);
        // }
        // if (map[ny, nx] == ']')
        // {
        //     return VerticalPush(nx, ny, dx, dy, map, stack)
        //         && VerticalPush(nx - 1, ny, dx, dy, map, stack);
        // }

        // throw new InvalidOperationException();
    }
}
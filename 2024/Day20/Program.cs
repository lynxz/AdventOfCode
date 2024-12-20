using System.Text;
using Tools;

var day = new Day20();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day20 : DayBase
{
    public Day20() : base("20")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var xMax = data[0].Length;
        var yMax = data.Length;
        var coords = Enumerable.Range(1, yMax - 1).SelectMany(y => Enumerable.Range(1, xMax - 1).Select(x => (x, y))).ToList();
        var start = coords.First(c => data[c.y][c.x] == 'S');
        var end = coords.First(c => data[c.y][c.x] == 'E');

        var (dist, prev) = GetDistance(start, end, data);
        var path = new List<(int x, int y)>();
        var current = end;
        while (current != start)
        {
            path.Add(current);
            current = prev[current];
        }

        path.Add(start);
        path.Reverse();

        var cheatDone = new HashSet<(int x, int y)>();
        var cheats = new List<(int x1, int y1, int x2, int y2, int t)>();
        foreach (var p in path)
        {
            foreach (var offset in new (int x, int y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                var next = (x: p.x + offset.x, y: p.y + offset.y);
                var nn = (x: next.x + offset.x, y: next.y + offset.y);
                if (nn.x < 1 || nn.x >= xMax - 1 || nn.y < 1 || nn.y >= yMax - 1)
                {
                    continue;
                }
                if (!cheatDone.Contains(next) && data[next.y][next.x] == '#' && path.Contains(nn))
                {
                    var nmap = new string[yMax];
                    for (var y = 0; y < yMax; y++)
                    {
                        var sb = new StringBuilder();
                        for (var x = 0; x < xMax; x++)
                        {

                            if (x == next.x && y == next.y)
                            {
                                sb.Append('.');
                            }
                            else
                            {
                                sb.Append(data[y][x]);
                            }
                            nmap[y] = sb.ToString();
                        }
                    }
                    cheatDone.Add(next);
                    var (ndist, nprev) = GetDistance(start, end, nmap);
                    var saved = dist[end] - ndist[end];
                    if (saved > 0)
                    {
                        cheats.Add((next.x, next.y, nn.x, nn.y, saved));
                    }
                }
            }
        }

        foreach (var g in cheats.GroupBy(c => c.t).OrderBy(g => g.Key))
        {
            System.Console.WriteLine($"There are {g.Count()} cheats that saves {g.Key} picoseconds");
        }

        return cheats.Count(c => c.t >= 100).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var xMax = data[0].Length;
        var yMax = data.Length;
        var coords = Enumerable.Range(1, yMax - 1).SelectMany(y => Enumerable.Range(1, xMax - 1).Select(x => (x, y))).ToList();
        var start = coords.First(c => data[c.y][c.x] == 'S');
        var end = coords.First(c => data[c.y][c.x] == 'E');

        var (dist, prev) = GetDistance(start, end, data);
        var path = new List<(int x, int y)>();
        var current = end;
        while (current != start)
        {
            path.Add(current);
            current = prev[current];
        }

        path.Add(start);
        path.Reverse();

        var cheats = new List<(int x1, int y1, int x2, int y2, int t)>();
        var pathVisited = new HashSet<(int x, int y)>();
        var curr = 1;
        foreach (var p in path)
        {
            var searched = new List<((int x, int y), int time)>();
            for (int i = -20; i <= 20; i++)
            {
                for (int j = -20; j <= 20; j++)
                {
                    var distance = Math.Abs(i) + Math.Abs(j);
                    if (distance > 20)
                    {
                        continue;
                    }
                    var key = (p.x + i, p.y + j);
                    if (path.Contains(key))
                    {
                        searched.Add((key, distance));
                    }
                }
            }
            var timeLeft = path.Count - curr;
            foreach (var (pos, cheatTime) in searched)
            {
                var index = path.IndexOf(pos);
                if (index != -1)
                {
                    var cheatOutTime = path.Count - index - 1;
                    var timeSaved = timeLeft - cheatOutTime - cheatTime;
                    cheats.Add((p.x, p.y, pos.x, pos.y, timeSaved));
                }
            }

            curr++;
        }
        return cheats.Count(c => c.t >= 100).ToString();
    }

    (Dictionary<(int x, int y), int> dist, Dictionary<(int x, int y), (int x, int y)> prev) GetDistance((int x, int y) start, (int x, int y) end, string[] data)
    {
        var xMax = data[0].Length;
        var yMax = data.Length;
        var coords = Enumerable.Range(1, yMax - 1).SelectMany(y => Enumerable.Range(1, xMax - 1).Select(x => (x, y))).ToList();
        var visited = new bool[yMax, xMax];
        var dist = new Dictionary<(int x, int y), int>();
        var prev = new Dictionary<(int x, int y), (int x, int y)>();
        var queue = new PriorityQueue<(int x, int y, int distance), int>();
        queue.Enqueue((start.x, start.y, 0), 0);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.x == end.x && current.y == end.y)
            {
                return (dist, prev);
            }
            if (visited[current.y, current.x])
            {
                continue;
            }
            visited[current.y, current.x] = true;
            foreach (var offset in new (int x, int y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                var next = (x: current.x + offset.x, y: current.y + offset.y);
                if (next.x < 1 || next.x >= xMax - 1 || next.y < 1 || next.y >= yMax - 1)
                {
                    continue;
                }
                if (data[next.y][next.x] == '#')
                {
                    continue;
                }
                if (!dist.ContainsKey(next) || dist[next] > current.distance + 1)
                {
                    dist[next] = current.distance + 1;
                    prev[next] = (current.x, current.y);
                    queue.Enqueue((next.x, next.y, current.distance + 1), current.distance + 1);
                }

            }

        }
        return (new Dictionary<(int x, int y), int>(), new Dictionary<(int x, int y), (int x, int y)>());
    }
}
using Tools;

Day17 day = new();
day.OutputSecondStar();

public class Day17 : DayBase
{
    public Day17() : base("17")
    {
    }

    public override string FirstStar()
    {
        var map = GetRowData().Select(x => x.Trim().Select(c => c - '0')).ToMultidimensionalArray();
        var result = Djikstra(map, (0, 0), (map.GetLength(0) - 1, map.GetLength(1) - 1));
        return result.ToString();
    }

    public override string SecondStar()
    {
    //    var day2 = new Day17Test {
    //           Inputs = GetRowData().ToArray()
    //      };
    //      var result = day2.DoWork();
    //     return result.ToString();

        var map = GetRowData().Select(x => x.Trim().Select(c => c - '0')).ToMultidimensionalArray();
        var result = Djikstra2(map, (0, 0), (map.GetLength(0) - 1, map.GetLength(1) - 1));
        return result.ToString();
    }

    int Djikstra(int[,] map, (int y, int x) start, (int y, int x) end)
    {
        var neighbors = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
        var prev = new Dictionary<(int xDist, int yDist, int y, int x), (int xDist, int yDist, int y, int x)>();
        var dist = Enumerable.Range(0, map.GetLength(0)).SelectMany(y => Enumerable.Range(0, map.GetLength(1)).SelectMany(x => Enumerable.Range(0, 4).SelectMany(yDist => Enumerable.Range(0, 4).Select(xDist => (yDist, xDist, y, x))))).ToDictionary(x => x, x => int.MaxValue);
        dist[(0, 0, start.y, start.x)] = 0;
        var visited = new HashSet<(int yDist, int xDist, int y, int x)>();
        var nodes = new PriorityQueue<(int yDist, int xDist, int y, int x), int>(dist.Count);
        Enumerable.Range(0, 4).SelectMany(y => Enumerable.Range(0, 4).Select(x => (y, x))).ToList().ForEach(x => visited.Add((x.y, x.x, start.y, start.x)));
        nodes.Enqueue((0, 0, start.y, start.x), 0);

        var done = false;
        while (nodes.Count > 0 && !done)
        {
            var current = nodes.Dequeue();
            if (current.y == end.y && current.x == end.x)
            {
                return dist[current];
            }
            else
            {
                var p = current == (0, 0, start.y, start.x) ? (0, 0, start.y, start.x) : prev[current];
                var neighbours = neighbors
                    .Select(x => (yDist: x.Item1 == 0 ? 0 : current.yDist + 1, xDist: x.Item2 == 0 ? 0 : current.xDist + 1, y: x.Item1 + current.y, x: x.Item2 + current.x))
                    .Where(x => x.y >= 0
                    && x.y < map.GetLength(0)
                    && x.x >= 0
                    && x.x < map.GetLength(1)
                    && !visited.Contains(x)
                    && (p.y != x.y || p.x != x.x));



                foreach (var neighbour in neighbours)
                {
                    if (neighbour.yDist < 4 && neighbour.xDist < 4)
                    {
                        var alt = dist[current] + map[neighbour.y, neighbour.x];
                        if (alt < dist[neighbour])
                        {
                            nodes.Enqueue(neighbour, alt);
                            visited.Add(neighbour);
                            dist[neighbour] = alt;
                            prev[neighbour] = current;
                        }
                    }
                }
            }
        }
        throw new InvalidOperationException();
    }

    int Djikstra2(int[,] map, (int y, int x) start, (int y, int x) end)
    {
        var neighbors = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

        var prev = new Dictionary<(int xDist, int yDist, int y, int x), (int xDist, int yDist, int y, int x)>();
        var dist = Enumerable.Range(0, map.GetLength(0)).SelectMany(y => Enumerable.Range(0, map.GetLength(1)).SelectMany(x => Enumerable.Range(0, 11).SelectMany(yDist => Enumerable.Range(0, 11).Select(xDist => (yDist, xDist, y, x))))).ToDictionary(x => x, x => int.MaxValue);
        dist[(0, 0, start.y, start.x)] = 0;
        //var visited = new HashSet<(int yDist, int xDist, int y, int x)>();
        var nodes = new PriorityQueue<(int yDist, int xDist, int y, int x), int>(dist.Count);
        //Enumerable.Range(0, 4).SelectMany(y => Enumerable.Range(0, 4).Select(x => (y, x))).ToList().ForEach(x => visited.Add((x.y, x.x, start.y, start.x)));
        nodes.Enqueue((0, 0, start.y, start.x), 0);

        var done = false;
        while (nodes.Count > 0 && !done)
        {
            var current = nodes.Dequeue();
            if (current.y == end.y && current.x == end.x)
            {
                var u = current;
                var dists = new List<int> { dist[current] };
                var path = new List<(int y, int x)>
                {
                    (u.y, u.x)
                };
                do
                {
                    u = prev[u];
                    dists.Insert(0, dist[u]);
                    path.Insert(0, (u.y, u.x));
                } while (prev[u] != (0, 0, start.y, start.x));

                for(int i = 0; i < dists.Count; i++)
                {
                    Console.WriteLine($"{i}: {dists[i]} path: ({path[i].y},{path[i].x})");
                }

                var index = 0;
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        if (path.Contains((y, x)))
                        {
                            index++;
                            Console.Write(index == 199 ? "O" : " ");
                        }
                        else
                        {
                            Console.Write(map[y, x]);
                        }
                    }
                    Console.WriteLine();
                }
                return dist[current];
            }
            else
            {
                var p = current == (0, 0, start.y, start.x) ? (0, 0, start.y, start.x) : prev[current];
                var neighbours = neighbors
                    .Select(x => (yDist: x.Item1 == 0 ? 0 : current.yDist + 1, xDist: x.Item2 == 0 ? 0 : current.xDist + 1, y: x.Item1 + current.y, x: x.Item2 + current.x))
                    .Where(x => x.y >= 0
                    && x.y < map.GetLength(0)
                    && x.x >= 0
                    && x.x < map.GetLength(1)
                    && (p.y != x.y || p.x != x.x));


                foreach (var neighbour in neighbours)
                {
                    var turned = (neighbour.xDist == 0 && current.xDist > 0) || (neighbour.yDist == 0 && current.yDist > 0) || (current == (0, 0, start.y, start.x));
                    if (turned)
                    {
                        var xOff = neighbour.x - current.x;
                        var yOff = neighbour.y - current.y;
                        if (neighbour.x + 3 * xOff < map.GetLength(1)
                            && neighbour.x + 3 * xOff >= 0
                            && neighbour.y + 3 * yOff < map.GetLength(0)
                            && neighbour.y + 3 * yOff >= 0)
                        {
                            var oldNode = current;
                            var d = dist[oldNode];
                            var newNodes = new List<(int alt, (int yDist, int xDist, int y, int x) newNode, (int yDist, int xDist, int y, int x) oldNode)>();
                            for (int i = 0; i < 4; i++)
                            {
                                var newNode = (yDist: neighbour.yDist + Math.Abs(i * yOff), xDist: neighbour.xDist + Math.Abs(i * xOff), y: neighbour.y + i * yOff, x: neighbour.x + i * xOff);
                                var alt = d + map[newNode.y, newNode.x];
                                newNodes.Add((alt, newNode, oldNode));
                                oldNode = newNode;
                                d = alt;
                            }
                            if (newNodes.All(nn => nn.alt < dist[nn.newNode]))
                            {
                                newNodes.ForEach(n =>
                                {
                                    //visited.Add(n.newNode);
                                    dist[n.newNode] = n.alt;
                                    prev[n.newNode] = n.oldNode;
                                });
                                nodes.Enqueue(newNodes.Last().newNode, dist[newNodes.Last().newNode]);
                            }
                        }
                    }
                    else
                    {
                        if (neighbour.yDist < 11 && neighbour.xDist < 11)
                        {
                            var alt = dist[current] + map[neighbour.y, neighbour.x];
                            if (alt < dist[neighbour])
                            {
                                nodes.Enqueue(neighbour, alt);
                                //visited.Add(neighbour);
                                dist[neighbour] = alt;
                                prev[neighbour] = current;
                            } 
                        }
                    }

                }
            }
        }
        throw new InvalidOperationException();
    }

}
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        var verticalRanges = new List<(int Start, int End, int X)>();
        var horizontalRanges = new List<(int Start, int End, int Y)>();
        var directions = new Dictionary<char, (int, int)> {
            { '3', (-1, 0) },
            { '0', (0, 1) },
            { '1', (1, 0) },
            { '2', (0, -1) }
        };

        var directions2 = new Dictionary<char, (int, int)> {
            { 'U', (-1, 0) },
            { 'R', (0, 1) },
            { 'D', (1, 0) },
            { 'L', (0, -1) }
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
                horizontalRanges.Add((currentX + dx, currentX + dx * distance, currentY));
                currentX += dx * distance;
                if (currentX < minX)
                    minX = currentX;
                if (currentX > maxX)
                    maxX = currentX;
            }
            else
            {
                verticalRanges.Add((currentY + dy, currentY + dy * distance, currentX));
                currentY += dy * distance;
                if (currentY < minY)
                    minY = currentY;
                if (currentY > maxY)
                    maxY = currentY;
            }

            // var direction = row[0];
            // var distance = row.GetIntegers().First();

            // var (dy, dx) = directions2[direction];

            // if (direction == 'L' || direction == 'R')
            // {
            //     horizontalRanges.Add((currentX + dx, currentX + dx * distance, currentY));
            //     currentX += dx * distance;
            //     if (currentX < minX)
            //         minX = currentX;
            //     if (currentX > maxX)
            //         maxX = currentX;
            // }
            // else
            // {
            //     verticalRanges.Add((currentY + dy, currentY + dy * distance, currentX));
            //     currentY += dy * distance;
            //     if (currentY < minY)
            //         minY = currentY;
            //     if (currentY > maxY)
            //         maxY = currentY;
            // }
        }

        var sum = 0L;

        var leftIndex = verticalRanges.IndexOf(verticalRanges.First(r => r.End == minY));
        var rightIndex = verticalRanges.IndexOf(verticalRanges.First(r => r.Start == minY + 1));

        var leftRange = verticalRanges[leftIndex];
        var rightRange = verticalRanges[rightIndex];
        var hr = horizontalRanges[(leftIndex + 1) % horizontalRanges.Count].SizeOfRange() + 1;
        var done = false;
        var leftStack = new Stack<(int Start, int End, int X)>();
        var rightStack = new Stack<(int Start, int End, int X)>();
        var hrStack = new Stack<(int Start, int End, int Y)>();

        do
        {

            if (leftRange.Start < rightRange.End)
            {
                sum += hr * leftRange.SizeOfRange();
                rightRange = (leftRange.Start + 1, rightRange.End, rightRange.X);
                leftRange = (leftRange.Start, leftRange.Start, leftRange.X);
            }
            else if (leftRange.Start >= rightRange.End)
            {
                sum += hr * rightRange.SizeOfRange();
                leftRange = (leftRange.Start, rightRange.End + 1, leftRange.X);
                rightRange = (rightRange.End, rightRange.End, rightRange.X);
            }

            if (leftRange.Start == leftRange.End && !done)
            {
                var lhr = (0, 0, 0);
                if (leftStack.Count == 0)
                {
                    leftIndex = leftIndex - 1 < 0 ? verticalRanges.Count - 1 : leftIndex - 1;
                    leftRange = verticalRanges[leftIndex];
                    lhr = horizontalRanges[(leftIndex + 1) % horizontalRanges.Count];
                }
                else
                {
                    leftRange = leftStack.Pop();
                    lhr = hrStack.Pop();
                }

                done = leftIndex == rightIndex;
                if (!done)
                {
                    var prevX = leftRange.X;

                    if (prevX < leftRange.X)
                    {
                        hr -= lhr.SizeOfRange();
                        sum += lhr.SizeOfRange();
                    }
                    else
                        hr += lhr.SizeOfRange();
                }

            }
            if (rightRange.Start == rightRange.End && !done)
            {
                var rhr = (0, 0, 0);
                if (rightStack.Count == 0)
                {
                    rightIndex = (rightIndex + 1) % verticalRanges.Count;
                    rightRange = verticalRanges[rightIndex];
                    rhr = horizontalRanges[rightIndex];
                }
                else
                {
                    rightRange = rightStack.Pop();
                    rhr = hrStack.Pop();
                }

                done = leftIndex == rightIndex;
                if (!done)
                {
                    var prevX = rightRange.X;

                    if (prevX > rightRange.X)
                    {
                        hr -= rhr.SizeOfRange();
                        sum += rhr.SizeOfRange();
                    }
                    else
                        hr += rhr.SizeOfRange();
                }
            }

            while (leftRange.Start < leftRange.End)
            {
                leftStack.Push(leftRange);
                hrStack.Push(horizontalRanges[(leftIndex + 1) % horizontalRanges.Count]);
                leftIndex = leftIndex - 1 < 0 ? verticalRanges.Count - 1 : leftIndex - 1;
                leftRange = verticalRanges[leftIndex];
            }

            while(rightRange.End < rightRange.Start)
            {
                rightStack.Push(rightRange);
                hrStack.Push(horizontalRanges[rightIndex]);
                rightIndex = (rightIndex + 1) % verticalRanges.Count;
                rightRange = verticalRanges[rightIndex];
            }

        } while (!done);

        sum += horizontalRanges[(leftIndex + 1) % horizontalRanges.Count].SizeOfRange() + 1;

        // var vs = new Stack<(int Start, int End, int X)>();
        // var hs = new Stack<(int Start, int End, int Y)>();
        // var index = verticalRanges.IndexOf(verticalRanges.First(r => r.X == minX));
        // vs.Push(verticalRanges[index]);

        // while (vs.Count > 0)
        // {
        //     hs.Push(horizontalRanges[index]);
        //     index = (index + 1) % verticalRanges.Count;
        //     var range = verticalRanges[index];
        //     var pr = 0;
        //     var hr = 0L;

        //     if (range.Start < range.End)
        //     {
        //         vs.Push(range);
        //     }
        //     else
        //     {
        //         var prevRange = vs.Pop();
        //         var prevH = hs.Pop();
        //         var rs = Math.Min(prevRange.SizeOfRange(), range.SizeOfRange());

        //         while (rs != 0)
        //         {

        //             if (prevH.Start > prevH.End)
        //             {
        //                 sum += prevH.SizeOfRange();
        //                 hr -= prevH.SizeOfRange();
        //             }
        //             else if (prevH.Start < prevH.End)
        //             {
        //                 hr += prevH.SizeOfRange() + 1;
        //                 sum += hr;
        //             }


        //             sum += rs * hr;

        //             if (rs < prevRange.SizeOfRange())
        //             {
        //                 prevRange = (prevRange.Start, prevRange.End + (int)rs, prevRange.X);
        //                 prevH = (prevRange.X, prevRange.X, prevH.Y);
        //                 rs = 0;
        //             }
        //             else
        //             {
        //                 // Fix this
        //             }
        //         }

        //     }

        // }


        // var rightIndex = verticalRanges.IndexOf(verticalRanges.First(r => r.Start == minY + 1));
        // var leftIndex = verticalRanges.IndexOf(verticalRanges.First(r => r.End == minY));
        // var rangeIndex = rightIndex + 1;

        // var pr = verticalRanges[rightIndex];
        // var pl = verticalRanges[leftIndex];
        // var dir = pr.SizeOfRange() > pl.SizeOfRange() ? 1 : -1;

        // if (dir == 1) {
        //     var rn = horizontalRanges[(leftIndex + 1) % verticalRanges.Count];
        //     sum += (rn.SizeOfRange() + 1)*(pl.SizeOfRange()+ 1);
        //     pr.End = pl.End;
        // } else {
        //     var rn = horizontalRanges[(leftIndex + 1) % verticalRanges.Count];
        //     sum += (rn.SizeOfRange() + 1)*(pl.SizeOfRange()+ 1);
        //     pl.Start = pr.Start;
        // }

        return sum.ToString();

    }

    long Boxes(int minX, int maxX, int minY, int maxY, List<(int Start, int End, int X)> verticalRanges, List<(int Start, int End, int Y)> horizontalRanges)
    {
        if (minX >= maxX)
            return 0;
        if (minY >= maxY)
            return 0;
        if (verticalRanges.Count == 0 && horizontalRanges.Count == 0)
        {
            return (maxX - minX) * (maxY - minY);
        }
        if (verticalRanges.Count == 0 && horizontalRanges.Count == 1)
        {
            var r = horizontalRanges.Single();
            var min = r.Start < r.End ? r.Y : minY;
            var max = r.Start < r.End ? maxY : r.Y;
            return (maxX - minX) * (max - min);
        }
        if (verticalRanges.Count == 1 && horizontalRanges.Count == 0)
        {
            var r = verticalRanges.Single();
            var min = r.Start < r.End ? r.X : minX;
            var max = r.Start < r.End ? maxX : r.X;
            return (maxY - minY) * (max - min);
        }

        var xRange = (maxX - minX) / 2;
        var yRange = (maxY - minY) / 2;

        var sum = 0L;

        for (int i = 0; i < 2; i++)
        {
            var newMinY = minY + i * yRange;
            var newMaxY = maxY - (1 - i) * yRange;
            for (int j = 0; j < 2; j++)
            {
                var newMinX = minX + i * xRange;
                var newMaxX = maxX - (1 - i) * xRange;

                var newVerticalRanges = new List<(int Start, int End, int X)>();
                var newHorizontalRanges = new List<(int Start, int End, int X)>();
                foreach (var range in verticalRanges)
                {
                    if (range.X > newMaxX || range.X < newMinX) continue;
                    if ((range.Start > newMaxY || range.Start < newMinY) && (range.End > newMaxY || range.End < newMinY)) continue;

                    if (Math.Max(range.End, range.Start) < newMaxY && Math.Min(range.End, range.Start) > newMinY)
                        newVerticalRanges.Add(range);
                    else if (range.Start < range.End && range.End > newMaxY)
                        newVerticalRanges.Add((range.Start, newMaxY, range.X));
                    else if (range.Start < range.End && range.Start < newMinY)
                        newVerticalRanges.Add((newMinY, range.End, range.X));
                    else if (range.Start > range.End && range.Start > newMaxY)
                        newVerticalRanges.Add((newMaxY, range.End, range.X));
                    else if (range.Start > range.End && range.End < newMinY)
                        newVerticalRanges.Add((range.Start, newMinY, range.X));
                }
                foreach (var range in horizontalRanges)
                {
                    if (range.Y > newMaxY || range.Y < newMinY) continue;
                    if ((range.Start > newMaxX || range.Start < newMinX) && (range.End > newMaxX || range.End < newMinX)) continue;

                    if (Math.Max(range.End, range.Start) < newMaxX && Math.Min(range.End, range.Start) > newMinX)
                        newVerticalRanges.Add(range);
                    else if (range.Start < range.End && range.End > newMaxX)
                        newVerticalRanges.Add((range.Start, newMaxX, range.Y));
                    else if (range.Start < range.End && range.Start < newMinX)
                        newVerticalRanges.Add((newMinX, range.End, range.Y));
                    else if (range.Start > range.End && range.Start > newMaxX)
                        newVerticalRanges.Add((newMaxX, range.End, range.Y));
                    else if (range.Start > range.End && range.End < newMinX)
                        newVerticalRanges.Add((range.Start, newMinX, range.Y));
                }

                sum += Boxes(newMinX, newMaxX, newMinY, newMaxY, newVerticalRanges, newHorizontalRanges);
            }
        }

        return sum;
    }


}

public static class Extensions
{

    public static long SizeOfRange(this (int Start, int End, int B) range) => Math.Abs(range.End - range.Start) + 1;

}
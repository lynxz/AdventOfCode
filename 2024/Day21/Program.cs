using System.Numerics;
using System.Text;
using Tools;

var day = new Day21();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day21 : DayBase
{

    Dictionary<char, (int x, int y)> NumpadPositions = new()
    {
        { '9', (2, 0) },
        { '8', (1, 0) },
        { '7', (0, 0) },
        { '6', (2, 1) },
        { '5', (1, 1) },
        { '4', (0, 1) },
        { '3', (2, 2) },
        { '2', (1, 2) },
        { '1', (0, 2) },
        { '0', (1, 3) },
        { 'A', (2, 3) }
    };

    Dictionary<char, (int x, int y)> ArrowPadPositions = new()
    {
        { '^', (1, 0) },
        { 'v', (1, 1) },
        { '>', (2, 1) },
        { '<', (0, 1) },
        { 'A', (2, 0) }
    };

    public Day21() : base("21")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var sum = 0L;
        var optimalDistance = new Dictionary<(int x1, int y1, int x2, int y2, int level), long>();
        foreach (var code in data)
        {

            (int x, int y)[] robots = [
                (2, 0),
                (2, 0),
                (2, 3)
            ];

            var result = 0L;

            foreach (var c in code)
            {
                var values = new List<long>();
                var pos = NumpadPositions[c];
                if (optimalDistance.ContainsKey((robots[2].x, robots[2].y, pos.x, pos.y, 2)))
                {
                    result += optimalDistance[(robots[2].x, robots[2].y, pos.x, pos.y, 2)];
                    robots[2] = pos;
                    continue;
                }

                var alts = NumPad(robots[2], pos);
                foreach (var alt in alts.GroupBy(a => a.Length).First())
                {
                    var lsum = 0L;
                    foreach (var a in alt)
                    {
                        var endPos = ArrowPadPositions[a];
                        var key = GetArrowPadCombos(robots[1], endPos, 1, optimalDistance, robots);
                        lsum += key;
                        robots[1] = endPos;
                    }
                    values.AddRange(lsum);
                }
                optimalDistance.Add((robots[2].x, robots[2].y, pos.x, pos.y, 2), values.Min());
                result += values.Min();
                robots[2] = pos;
            }
            var numPar = int.Parse(string.Join("", code.GetIntegers()));
            sum += result * numPar;
        }

        return sum.ToString();
    }

    long GetArrowPadCombos((int x, int y) start, (int x, int y) end, int level, Dictionary<(int x1, int y1, int x2, int y2, int level), long> optimalDistance, (int x, int y)[] robots)
    {
        if (optimalDistance.ContainsKey((start.x, start.y, end.x, end.y, level)))
        {
            return optimalDistance[(start.x, start.y, end.x, end.y, level)];
        }
        var alts = ArrowPad(start, end);
        if (level == 0)
        {
            return alts.OrderBy(a => a.Length).First().Length;
        }
        var totalCombos = new List<long>();
        foreach (var alt in alts)
        {
            var lsum = 0L;
            foreach (var c in alt)
            {
                var pos = ArrowPadPositions[c];
                var key = GetArrowPadCombos(robots[level - 1], pos, level - 1, optimalDistance, robots);
                lsum += key;
                robots[level - 1] = pos;
            }
            totalCombos.Add(lsum);
        }

        optimalDistance.Add((start.x, start.y, end.x, end.y, level), totalCombos.Min());
        return totalCombos.Min();
    }

    public List<string> NumPad((int x, int y) start, (int x, int y) end)
    {
        var result = new List<string>();
        var dx = end.x > start.x ? 1 : -1;
        var dy = end.y > start.y ? 1 : -1;

        var stack = new Stack<(int x, int y, StringBuilder sb)>();
        stack.Push((start.x, start.y, new StringBuilder()));

        while (stack.Count > 0)
        {
            var (x, y, sb) = stack.Pop();
            if (x == end.x && y == end.y)
            {
                sb.Append("A");
                result.Add(sb.ToString());
                continue;
            }
            if (x == 0 && y == 3)
            {
                continue;
            }

            if (x != end.x)
            {
                stack.Push((x + dx, y, new StringBuilder(sb.ToString()).Append(dx == 1 ? ">" : "<")));
            }

            if (y != end.y)
            {
                stack.Push((x, y + dy, new StringBuilder(sb.ToString()).Append(dy == 1 ? "v" : "^")));
            }
        }

        return result;
    }

    public List<string> ArrowPad((int x, int y) start, (int x, int y) end)
    {
        var result = new List<string>();
        var dx = end.x > start.x ? 1 : -1;
        var dy = end.y > start.y ? 1 : -1;

        var stack = new Stack<(int x, int y, StringBuilder sb)>();
        stack.Push((start.x, start.y, new StringBuilder()));

        while (stack.Count > 0)
        {
            var (x, y, sb) = stack.Pop();
            if (x == end.x && y == end.y)
            {
                sb.Append("A");
                result.Add(sb.ToString());
                continue;
            }
            if (x == 0 && y == 0)
            {
                continue;
            }

            if (x != end.x)
            {
                stack.Push((x + dx, y, new StringBuilder(sb.ToString()).Append(dx == 1 ? ">" : "<")));
            }

            if (y != end.y)
            {
                stack.Push((x, y + dy, new StringBuilder(sb.ToString()).Append(dy == 1 ? "v" : "^")));
            }
        }

        return result;
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var sum = new BigInteger(0);
        var optimalDistance = new Dictionary<(int x1, int y1, int x2, int y2, int level), long>();
        foreach (var code in data)
        {

            var robots = Enumerable.Repeat((x: 2, y: 0), 26).ToArray();
            var top = robots.Length - 1;
            robots[top] = (2, 3);
            var result = 0L;

            foreach (var c in code)
            {
                var lengths = new List<long>();
                var pos = NumpadPositions[c];
                if (optimalDistance.ContainsKey((robots[top].x, robots[top].y, pos.x, pos.y, top)))
                {
                    result += optimalDistance[(robots[top].x, robots[top].y, pos.x, pos.y, top)];
                    robots[top] = pos;
                    continue;
                }

                var alts = NumPad(robots[top], pos);
                foreach (var alt in alts.GroupBy(a => a.Length).First())
                {
                    var lsum = 0L;
                    foreach (var a in alt)
                    {
                        var endPos = ArrowPadPositions[a];
                        var key = GetArrowPadCombos(robots[top-1], endPos, top-1, optimalDistance, robots);
                        lsum += key;
                        robots[top-1] = endPos;
                    }
                    lengths.AddRange(lsum);
                }
                optimalDistance.Add((robots[top].x, robots[top].y, pos.x, pos.y, top), lengths.Min());
                result += lengths.Min(); 
                robots[top] = pos;
            }
            var numPar = int.Parse(string.Join("", code.GetIntegers()));
            sum += new BigInteger(result) * numPar;
        }

        return sum.ToString();
    }
}
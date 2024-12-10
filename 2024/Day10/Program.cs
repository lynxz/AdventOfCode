using Tools;

var day = new Day10();
day.OutputSecondStar();

public class Day10 : DayBase
{
    public Day10() : base("10")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var yMax = data.Length;
        var xMax = data[0].Length;

        var stack = new Stack<(int x, int y)>();
        Enumerable.Range(0, yMax)
            .SelectMany(y => Enumerable.Range(0, xMax)
                .Where(x => data[y][x] == '0')
                .Select(x => (x, y)))
            .ToList()
            .ForEach(stack.Push);

        var steps = new List<(int x, int y)>() {
            (0, 1),
            (1, 0),
            (0, -1),
            (-1, 0)
        };
        var tops = new Dictionary<(int x, int y), HashSet<(int x, int y)>>();
        HashSet<(int x, int y)> currList = null;
        while (stack.Count != 0)
        {
            var pos = stack.Pop();
            if (data[pos.y][pos.x] == '0') {
                currList = new HashSet<(int x, int y)>();
                tops[pos] = currList;
            }
            if (data[pos.y][pos.x] == '9')
            {
                currList.Add(pos);
            }
            else
            {
                steps.ForEach(step =>
                {
                    var x = pos.x + step.x;
                    var y = pos.y + step.y;
                    if (x >= 0 && x < xMax && y >= 0 && y < yMax && (data[y][x] - data[pos.y][pos.x]) == 1)
                    {
                        stack.Push((x, y));
                    }
                });
            }
        }

        return tops.Sum(t => t.Value.Count).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToArray();
        var yMax = data.Length;
        var xMax = data[0].Length;

        var stack = new Stack<(int x, int y)>();
        Enumerable.Range(0, yMax)
            .SelectMany(y => Enumerable.Range(0, xMax)
                .Where(x => data[y][x] == '0')
                .Select(x => (x, y)))
            .ToList()
            .ForEach(stack.Push);

        var steps = new List<(int x, int y)>() {
            (0, 1),
            (1, 0),
            (0, -1),
            (-1, 0)
        };
        var paths = 0;
        while (stack.Count != 0)
        {
            var pos = stack.Pop();

            if (data[pos.y][pos.x] == '9')
            {
                paths++;
            }
            else
            {
                steps.ForEach(step =>
                {
                    var x = pos.x + step.x;
                    var y = pos.y + step.y;
                    if (x >= 0 && x < xMax && y >= 0 && y < yMax && (data[y][x] - data[pos.y][pos.x]) == 1)
                    {
                        stack.Push((x, y));
                    }
                });
            }
        }

        return paths.ToString();
    }
}
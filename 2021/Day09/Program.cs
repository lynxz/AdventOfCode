// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day9("9");
day.OutputSecondStar();

public class Day9 : DayBase
{
    public Day9(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData();
        var array = Enumerable.Range(0, input.Length).Select(y =>
            Enumerable.Range(0, input.First().Length).Select(x =>
                int.Parse(input[y][x].ToString()))).ToMultidimensionalArray();
        var lowPoint = GetLowPoints(array);

        return lowPoint.Sum(d => array[d.y, d.x] + 1).ToString();
    }



    public override string SecondStar()
    {
        var input = GetRowData();
        var array = Enumerable.Range(0, input.Length).Select(y =>
            Enumerable.Range(0, input.First().Length).Select(x =>
                int.Parse(input[y][x].ToString()))).ToMultidimensionalArray();
        var lowPoint = GetLowPoints(array);
        var basins = lowPoint.Select(p => FindBasin(p, new List<(int y, int x)> { p }, array)).ToList();

        return basins.OrderByDescending(b => b.Count).Take(3).Aggregate(1L, (s, b) => s *= b.Count).ToString();
    }

    private List<(int y, int x)> GetLowPoints(int[,] array)
    {
        var yMax = array.GetLength(0);
        var xMax = array.GetLength(1);
        List<(int, int)> lowPoint = new();
        var add = new[] { 1, -1 };
        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                var pointsArround = add.Where(i => x + i >= 0 && x + i < xMax).Select(i => array[y, x + i]).ToList();
                pointsArround.AddRange(add.Where(i => y + i >= 0 && y + i < yMax).Select(i => array[y + i, x]));
                if (pointsArround.All(i => i > array[y, x]))
                    lowPoint.Add((y, x));
            }
        }

        return lowPoint;
    }

    List<(int y, int x)> FindBasin((int y, int x) pos, List<(int y, int x)> basinPoints, int[,] array)
    {
        var add = new[] { (y: 1, x: 0), (y: -1, x: 0), (y: 0, x: 1), (y: 0, x: -1) };
        var possiblePoints = add.Where(p =>
            pos.x + p.x >= 0 &&
            pos.y + p.y >= 0 &&
            pos.x + p.x < array.GetLength(1) &&
            pos.y + p.y < array.GetLength(0)).Select(p => (y: p.y + pos.y, x: p.x + pos.x)).ToList();

        foreach (var p in possiblePoints)
        {
            if (array[p.y, p.x] != 9 && array[pos.y, pos.x] <= array[p.y, p.x] && !basinPoints.Contains(p))
            {
                basinPoints.Add(p);
                FindBasin(p, basinPoints, array);
            }
        }
        return basinPoints;
    }
}

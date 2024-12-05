using System.ComponentModel;
using Tools;

Day04 day = new();
day.OutputFirstStar();
day.OutputSecondStar();
static void Main(string[] args)
{

}


public class Day04 : DayBase
{
    public Day04() : base("4")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.Trim()).ToList();
        var array = data.ToMultidimensionalArray();

        var offsets = new List<List<(int x, int y)>>
        {
            new List<(int x, int y)> { (0, 0), (0, 1), (0, 2), (0, 3) },
            new List<(int x, int y)> { (0, 0), (-1, 1), (-2, 2), (-3, 3) },
            new List<(int x, int y)> { (0, 0), (-1, -1), (-2, -2), (-3, -3) },
            new List<(int x, int y)> { (0, 0), (-1, 0), (-2, 0), (-3, 0) },
        };

        var coords = Enumerable.Range(0, data.Count)
            .SelectMany(y => 
                Enumerable.Range(0, data[0].Length)
                .Where(x => data[y][x] == 'X' || data[y][x] == 'S')
                .Select(x => (x, y))
            ).ToList();
        
        return coords.Sum(c => offsets
            .Where(o => o.All(of => c.x + of.x >= 0 && c.x + of.x < data[0].Length && c.y + of.y >= 0 && c.y + of.y < data.Count))
            .Select(o => new string(o.Select(of => data[c.y + of.y][c.x + of.x]).ToArray()))
            .Count(s => s == "XMAS" || s == "SAMX")
        ).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Trim());
        var array = data.ToMultidimensionalArray();

        var offsets = new List<List<(int x, int y)>>
        {
            new List<(int x, int y)> { (1, 1), (0, 0), (-1, -1)},
            new List<(int x, int y)> { (-1, 1), (0, 0), (1, -1)},
        };

        var xmasCount = 0;
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                if (array[y, x] != 'A')
                    continue;
                if (!offsets.All(offset => offset.All(of => x + of.x >= 0 && x + of.x < array.GetLength(0) && y + of.y >= 0 && y + of.y < array.GetLength(1))))
                    continue;
                var res = offsets.Select(offset => new string(offset.Select(of => array[y + of.y, x + of.x]).ToArray())).ToList();
                if (res.All(s => s == "MAS" || s == "SAM"))
                    xmasCount++;
            }
        }

        return xmasCount.ToString();
    }
}
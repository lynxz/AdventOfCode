using Tools;

Day04 day = new();
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
        var data = GetRowData().Select(r => r.Trim());
        var array = data.ToMultidimensionalArray();

        var offsets = new List<List<(int x, int y)>>
        {
            new List<(int x, int y)> { (0, 0), (0, 1), (0, 2), (0, 3) },
            new List<(int x, int y)> { (0, 0), (-1, 1), (-2, 2), (-3, 3) },
            new List<(int x, int y)> { (0, 0), (-1, -1), (-2, -2), (-3, -3) },
            new List<(int x, int y)> { (0, 0), (-1, 0), (-2, 0), (-3, 0) },
        };

        var xmasCount = 0;
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                if (array[y, x] != 'X' && array[y, x] != 'S')
                    continue;

                foreach (var offset in offsets)
                {
                    if (!offset.All(of => x + of.x >= 0 && x + of.x < array.GetLength(0) && y + of.y >= 0 && y + of.y < array.GetLength(1)))
                        continue;

                    var res = new string(offset.Select(of => array[y + of.y, x + of.x]).ToArray());
                    if (res == "XMAS" || res == "SAMX")
                        xmasCount++;
                }
            }
        }

        return xmasCount.ToString();
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
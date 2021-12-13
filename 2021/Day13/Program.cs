// See https://aka.ms/new-console-template for more information
using System.Text;
using Tools;

Day13 day = new("13");
day.OutputSecondStar();

public class Day13 : DayBase
{
    public Day13(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRawData();
        var parts = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var coords = parts[0].Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.GetIntegers()).ToList();
        var xMax = coords.Max(c => c[0]) + 1;
        var yMax = coords.Max(c => c[1]) + 1;
        var array = new bool[yMax, xMax];
        coords.ForEach(c => array[c[1], c[0]] = true);

        var folds = parts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Substring(11).Split('=', StringSplitOptions.RemoveEmptyEntries))
            .Select(p => (Axis: p[0], Pos: int.Parse(p[1])))
            .ToList();

        array = Fold(folds.First().Axis, folds.First().Pos, array);

        return Enumerable.Range(0, array.GetLength(0)).Sum(y => Enumerable.Range(0, array.GetLength(1)).Count(x => array[y,x])).ToString();
    }

    bool[,] Fold(string axis, int pos, bool[,] arr) =>
        axis switch
        {
            "x" => FoldX(pos, arr),
            "y" => FoldY(pos, arr)
        };
    private bool[,] FoldX(int pos, bool[,] arr)
    {
        var returnArr = new bool[arr.GetLength(0), pos];
        for (int y = 0; y < arr.GetLength(0); y++)
        {
            for (int x = 0; x < arr.GetLength(1); x++)
            {
                if (x < pos)
                    returnArr[y, x] = arr[y, x];
                else if (x > pos)
                    returnArr[y, 2 * pos - x] |= arr[y, x];
            }
        }
        return returnArr;
    }

    private bool[,] FoldY(int pos, bool[,] arr)
    {
        var returnArr = new bool[pos, arr.GetLength(1)];
        for (int y = 0; y < arr.GetLength(0); y++)
        {
            for (int x = 0; x < arr.GetLength(1); x++)
            {
                if (y < pos)
                    returnArr[y, x] = arr[y, x];
                else if (y > pos)
                    returnArr[2 * pos - y, x] |= arr[y, x];
            }
        }
        return returnArr;
    }

    void PrintArray(bool[,] arr)
    {
        for (int y = 0; y < arr.GetLength(0); y++)
        {
            for (int x = 0; x < arr.GetLength(1); x++)
            {
                System.Console.Write(arr[y, x] ? "#" : ".");
            }
            System.Console.WriteLine();
        }

        System.Console.WriteLine();
    }

    public override string SecondStar()
    {
        var input = GetRawData();
        var parts = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var coords = parts[0].Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => s.GetIntegers()).ToList();
        var xMax = coords.Max(c => c[0]) + 1;
        var yMax = coords.Max(c => c[1]) + 1;
        var array = new bool[yMax, xMax];
        coords.ForEach(c => array[c[1], c[0]] = true);

        var folds = parts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Substring(11).Split('=', StringSplitOptions.RemoveEmptyEntries))
            .Select(p => (Axis: p[0], Pos: int.Parse(p[1])))
            .ToList();

        foreach (var fold in folds) {
            array = Fold(fold.Axis, fold.Pos, array);
        }

        PrintArray(array);
        return string.Empty;
    }
}
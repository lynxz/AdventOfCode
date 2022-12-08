using Tools;

Day08 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day08 : DayBase
{
    public Day08() : base("8")
    {
    }
    public override string FirstStar()
    {
        var data = GetRowData().Select(r => r.Select(n => int.Parse(n.ToString())));
        var trees = data.ToMultidimensionalArray();
        var width = trees.GetLength(0);
        var height = trees.GetLength(1);
        var invisible = 0;
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (
                    Enumerable.Range(0, x).Any(i => trees[y, i] >= trees[y, x]) &&
                    Enumerable.Range(x + 1, width - (x + 1)).Any(i => trees[y, i] >= trees[y, x]) &&
                    Enumerable.Range(0, y).Any(i => trees[i, x] >= trees[y, x]) &&
                    Enumerable.Range(y + 1, height - (y + 1)).Any(i => trees[i, x] >= trees[y, x])
                )
                {
                    invisible++;
                }
            }
        }

        return (trees.Length - invisible).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => r.Select(n => int.Parse(n.ToString())));
        var trees = data.ToMultidimensionalArray();
        var width = trees.GetLength(0);
        var height = trees.GetLength(1);
        var max = 0;
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                var left = Enumerable.Range(1, x).FirstOrDefault(i => trees[y, x - i] >= trees[y, x], x);
                var right = Enumerable.Range(1, width - (x + 1)).FirstOrDefault(i => trees[y, x + i] >= trees[y, x], width - (x + 1));
                var up = Enumerable.Range(1, y).FirstOrDefault(i => trees[y - i, x] >= trees[y, x], y);
                var down = Enumerable.Range(1, height - (y + 1)).FirstOrDefault(i => trees[y+i, x] >= trees[y, x], height - (y + 1));
                var prod = left*right*up*down;
                if (prod > max) {
                    max = prod;
                }
            }
        }

        return max.ToString();
    }
}
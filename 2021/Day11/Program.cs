// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day11("11");
day.OutputSecondStar();

public class Day11 : DayBase
{

    (int Y, int X)[] offsets;
    List<int> steps;

    public Day11(string day) : base(day)
    {
        offsets = Enumerable.Range(-1, 3).SelectMany(y => Enumerable.Range(-1, 3).Select(x => (Y: y, X: x))).Where(p => !(p.X == 0 && p.Y == 0)).ToArray();
        steps = Enumerable.Range(0, 10).ToList();
    }

    public override string FirstStar()
    {
        var input = GetRowData();
        var arr = Enumerable.Range(0, 10).Select(y => Enumerable.Range(0, 10).Select(x => int.Parse(input[y][x].ToString()))).ToMultidimensionalArray();
        var flashes = 0;
        for (int i = 0; i < 100; i++)
        {
            steps.ForEach(y => steps.ForEach(x => arr[y, x]++));
            steps.ForEach(y => steps.ForEach(x => Fill(arr, (y,x))));
            flashes += steps.Sum(y => steps.Count(x => arr[y,x] == 0));
        }
        return flashes.ToString();
    }

    public override string SecondStar()
    {
        var input = GetRowData();
        var arr = Enumerable.Range(0, 10).Select(y => Enumerable.Range(0, 10).Select(x => int.Parse(input[y][x].ToString()))).ToMultidimensionalArray();
        var count = 0;
        while(true)
        {
            var newArry = new int[10, 10];
            steps.ForEach(y => steps.ForEach(x => arr[y, x]++));
            steps.ForEach(y => steps.ForEach(x => Fill(arr, (y,x))));
            count++;
            if (steps.All(y => steps.All(x => arr[x,y] == 0)))
                break;
        }
        return count.ToString();
    }

    private void Fill(int[,] arr, (int Y, int X) coord)
    {
        if (arr[coord.Y, coord.X] > 9)
        {
            arr[coord.Y, coord.X] = 0;
            foreach (var offset in offsets)
            {
                var newCoord = (Y: coord.Y + offset.Y, X: coord.X + offset.X);
                if (newCoord.X > -1 && newCoord.X < 10 && newCoord.Y > -1 && newCoord.Y < 10 && arr[newCoord.Y, newCoord.X] != 0)
                {
                    arr[newCoord.Y, newCoord.X]++;
                    Fill(arr, newCoord);
                }
            }
        }
    }
}

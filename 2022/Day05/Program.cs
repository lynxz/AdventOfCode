using Tools;

Day05 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day05 : DayBase
{
    public Day05() : base("5")
    {
    }
    public override string FirstStar() =>
     Crane(true);

    private string Crane(bool reverse)
    {
        const int noStacks = 9;
        var data = GetRowData();
        int row = 0;
        var stacks = Enumerable.Range(1, noStacks).Select(_ => new List<char>()).ToList();
        while (data[row][1] != '1')
        {
            foreach (var i in 0..noStacks)
            {
                if (data[row][(i * 4) + 1] != ' ')
                {
                    stacks[i].Add(data[row][(i * 4) + 1]);
                }
            }
            row++;
        }
        row++;
        foreach (var move in data[row..].Select(d => d.GetIntegers()))
        {
            var carts = stacks[move[1] - 1].Take(move[0]);
            stacks[move[2] - 1].InsertRange(0, reverse ? carts.Reverse() : carts);
            stacks[move[1] - 1].RemoveRange(0, move[0]);
        }
        return new string(stacks.Select(s => s[0]).ToArray());
    }

    public override string SecondStar() =>
        Crane(false);
}
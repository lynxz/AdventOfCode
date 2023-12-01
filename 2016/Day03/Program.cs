using Tools;

Day03 day = new();
day.OutputSecondStar();

public class Day03 : DayBase
{
    public Day03() : base("3")
    {
    }

    public override string FirstStar()
    {
         return  GetRowData()
            .Select(r => r.GetIntegers())
            .Count(r => r[0] + r[1] > r[2] && r[0] + r[2] > r[1] && r[1] + r[2] > r[0])
            .ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData()
            .Select(r => r.GetIntegers())
            .ToArray();

        var count = 0;
        for (int i = 0; i < data.Length; i += 3)
        {
            for (int j = 0; j < 3; j++)
            {
                var r = new int[] { data[i][j], data[i + 1][j], data[i + 2][j] };
                if (r[0] + r[1] > r[2] && r[0] + r[2] > r[1] && r[1] + r[2] > r[0])
                    count++;
            }
        }

        return count.ToString();
    }
}
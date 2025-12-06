using System.Text;
using Tools;

Day06 day = new();
day.OutputFirstStar();
day.OutputSecondStar();;

public class Day06 : DayBase
{
    public Day06() : base("6")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var numbers = Enumerable.Range(0, data.Length - 1).Select(r => data[r].GetIntegers()).ToArray();
        var signs = data.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]).ToArray();

        var totalSum = 0L;
        for (int i = 0; i < numbers[0].Length; i++)
        {
            var sum = signs[i] == '+' ? 0L : 1L;
            for (int j = 0; j < numbers.Length; j++)
            {
                sum = GetSum(sum, signs[i], numbers[j][i]);
            }
           totalSum += sum;
        }

        return totalSum.ToString();
    }

    public long GetSum(long sum, char sign, int value) =>
        sign == '+' ? sum + value : sum * value;

    public override string SecondStar()
    {
        var data = GetRowData();
        var numbers = Enumerable.Range(0, data.Length - 1).Select(r => data[r].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray();
        var signs = data.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]).ToArray();

        var cols = new List<List<int>>();
        var col = new List<int>();
        for (int i = data[0].Length - 1; i >= 0; i--)
        {
            var sb = new StringBuilder();
            for (int j = 0; j < data.Length - 1; j++)
            {
                sb.Append(data[j][i]);
            }
            if (int.TryParse(sb.ToString().Trim(), out var val))
            {
                col.Add(val);
            }
            else
            {
                cols.Add(col);
                col = new List<int>();
            }
        }
        cols.Add(col);

        var totalSum = 0L;

        for (int i= 0; i < cols.Count; i++) 
        {
            var sign = signs[signs.Length - 1 - i];
            var sum = sign == '+' ? 0L : 1L;
            for (int j = 0; j < cols[i].Count; j++)
            {
               sum = GetSum(sum, sign, cols[i][j]);
            }
            totalSum += sum;
        }


        return totalSum.ToString();
    }
}
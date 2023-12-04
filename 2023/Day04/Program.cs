
using Tools;

Day04 day = new Day04();
day.OutputSecondStar();

public class Day04 : DayBase
{
    public Day04() : base("4")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var sum = 0L;
        foreach(var row in data) {
            var p = row.Split(":", StringSplitOptions.RemoveEmptyEntries);
            var p2 = p[1].Split("|", StringSplitOptions.RemoveEmptyEntries);
            var nums = p2[1].GetIntegers();
            var wins = p2[0].GetIntegers();
            var t = nums.Count(n => wins.Contains(n));
            var result = t == 0 ? 0 : Convert.ToInt64(Math.Pow(2, t -1));
            sum += result;
        }
        return sum.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var array = Enumerable.Repeat(1, data.Length).ToArray();
        for (int i = 0; i < data.Length; i++) {
            var p = data[i].Split(":", StringSplitOptions.RemoveEmptyEntries);
            var p2 = p[1].Split("|", StringSplitOptions.RemoveEmptyEntries);
            var nums = p2[1].GetIntegers();
            var wins = p2[0].GetIntegers();
            var t = nums.Count(n => wins.Contains(n));
            for (int j = 1; j <= t; j++) {
                array[i+j] += array[i];
            }
        }
        System.Console.WriteLine(string.Join(" ", array));
        return array.Sum().ToString();
    }
}
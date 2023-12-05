
using Tools;

Day04 day = new Day04();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day04 : DayBase
{
    public Day04() : base("4")
    {
    }

    public override string FirstStar()
    {
        return GetRowData().Select(r => r.Split(":", StringSplitOptions.RemoveEmptyEntries)[1].Split("|", StringSplitOptions.RemoveEmptyEntries))
            .Select(r => r[0].GetIntegers().Intersect(r[1].GetIntegers()).Count())
            .Sum(r => Convert.ToInt64(Math.Pow(2, r - 1)))
            .ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var array = Enumerable.Repeat(1, data.Length).ToArray();
        for (int i = 0; i < data.Length; i++)
        {
            var r = data[i].Split(":", StringSplitOptions.RemoveEmptyEntries)[1].Split("|", StringSplitOptions.RemoveEmptyEntries);
            var t = r[0].GetIntegers().Intersect(r[1].GetIntegers()).Count();
            Enumerable.Range(1, t).ToList().ForEach(j => array[i + j] += array[i]);
        }
        return array.Sum().ToString();
    }
}
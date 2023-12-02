using System.Text.RegularExpressions;
using Tools;

Day01 day = new();
day.OutputSecondStar();

public class Day01 : DayBase
{
    public Day01() : base("1")
    {
    }

    public override string FirstStar()
    {
        return GetRowData().Select(r => r.Where(c => Char.IsNumber(c)).Select(i => int.Parse(i.ToString()))).Sum(r => r.First() * 10 + r.Last()).ToString();
    }

    public override string SecondStar()
    {
        var nums = "123456789";
        var words = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        var intMap = new Dictionary<string, int> {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9}
        };

        var values = GetRowData().Select(r =>
        {
            var result = new List<int>();
            for (int i = 0; i < r.Length; i++)
            {
                if (nums.Contains(r[i]))
                {
                    result.Add(int.Parse(r[i].ToString()));
                }
                else
                {
                    var word = words.Where(w => w.Length + i - 1 < r.Length).FirstOrDefault(w => r.Substring(i, w.Length) == w);
                    if (word != null)
                    {
                        result.Add(intMap[r.Substring(i, word.Length)]);
                    }
                }
            }
            return result.ToArray();
        }).ToList();
        return values.Sum(r => r.First() * 10 + r.Last()).ToString();
    }
}
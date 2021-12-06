// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day6("6");
day.OutputSecondStar();

public class Day6 : DayBase
{
    public Day6(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRawData().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();
        for (int i = 0; i < 80; i++)
        {
            var k = input.Count;
            for (int j = 0; j < k; j++)
            {
                input[j]--;
                if (input[j] == -1)
                {
                    input[j] = 6;
                    input.Add(8);
                }
            }
        }
        return input.Count.ToString();
    }

    public override string SecondStar()
    {
        var input = GetRawData().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToList();
        var l = input.Count;
        var fish = new long[256];
        for (int i = 0; i < 9; i++)
        {
            var k = input.Count;
            for (int j = 0; j < k; j++)
            {
                input[j]--;
                if (input[j] == -1)
                {
                    input[j] = 6;
                    input.Add(8);
                }
            }
            fish[i] = input.Count - k;
        }

        for (int i = 9; i < 256; i++)
        {
            fish[i] = fish[i - 7] + fish[i - 9];
        }

        var d = fish.Sum() + l;

        return (fish.Sum() + l).ToString();
    }
}
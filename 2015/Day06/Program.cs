// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using Tools;

var day = new Day6();
day.OutputSecondStar();

public class Day6 : DayBase
{
    public Day6() : base("6")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var coords = data.Select(r => r.GetIntegers()).ToList();
        var array = new bool[1000, 1000];
        for (int i = 0; i < data.Length; i++)
        {
            for (int x = coords[i][0]; x <= coords[i][2]; x++) {
                for (int y = coords[i][1]; y <= coords[i][3]; y++)
                {
                    if (data[i].StartsWith("toggle"))
                        array[x,y] = !array[x,y];
                    else if (data[i].StartsWith("turn on"))
                        array[x,y] = true;
                    else
                        array[x,y] = false;
                }
            }
        }
        return Enumerable.Range(0, 1000).Sum(x => Enumerable.Range(0, 1000).Count(y => array[x,y])).ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var coords = data.Select(r => r.GetIntegers()).ToList();
        var array = new int[1000, 1000];
        for (int i = 0; i < data.Length; i++)
        {
            for (int x = coords[i][0]; x <= coords[i][2]; x++) {
                for (int y = coords[i][1]; y <= coords[i][3]; y++)
                {
                    if (data[i].StartsWith("toggle"))
                        array[x,y] += 2;
                    else if (data[i].StartsWith("turn on"))
                        array[x,y]++;
                    else if (array[x,y] > 0)
                        array[x,y]--;
                }
            }
        }
        return Enumerable.Range(0, 1000).Sum(x => Enumerable.Range(0, 1000).Sum(y => array[x,y])).ToString();
    }
}

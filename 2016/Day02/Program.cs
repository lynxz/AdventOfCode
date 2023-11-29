using System.Text;
using Tools;

var day = new Day02();
day.PostSecondStar();

public class Day02 : DayBase
{

    public Day02() : base("2")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var values = Enumerable.Range(1, 9).Select(x => x.ToString()).ToArray();
        var result = new StringBuilder();
        foreach (var row in data)
        {
            var x = 1;
            var y = 1;
            foreach (var c in row)
            {
                switch (c)
                {
                    case 'U':
                        y = Math.Max(0, y - 1);
                        break;
                    case 'D':
                        y = Math.Min(2, y + 1);
                        break;
                    case 'L':
                        x = Math.Max(0, x - 1);
                        break;
                    case 'R':
                        x = Math.Min(2, x + 1);
                        break;
                    default:
                        break;
                }
            }

            result.Append(values![y * 3 + x]);
        }

        return result.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var values = new string?[5,5] {
            { null, null, "1", null, null },
            { null, "2", "3", "4", null },
            { "5", "6", "7", "8", "9" },
            { null, "A", "B", "C", null },
            { null, null, "D", null, null }
        };
        var result = new StringBuilder();

        foreach (var row in data)
        {
            var x = 0;
            var y = 2;
            foreach (var c in row)
            {
                switch (c)
                {
                    case 'U':
                        y = values[Math.Max(0, y - 1), x] == null ? y : Math.Max(0, y - 1);
                        break;
                    case 'D':
                        y = values[Math.Min(4, y + 1), x] == null ? y : Math.Min(4, y + 1);
                        break;
                    case 'L':
                        x = values[y, Math.Max(0, x - 1)] == null ? x : Math.Max(0, x - 1);
                        break;
                    case 'R':
                        x = values[y, Math.Min(4, x + 1)] == null ? x : Math.Min(4, x + 1);
                        break;
                    default:
                        break;
                }
            }

            result.Append(values![y, x]);
        }

        return result.ToString();
    }

}
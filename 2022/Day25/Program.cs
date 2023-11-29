using Tools;

Day25 day = new();
day.OutputFirstStar();

public class Day25 : DayBase
{

    public Day25() : base("25")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var max = data.Max(r => r.Length);
        var mults = Enumerable.Range(0, max).Aggregate(new List<long>() { 1L }, (l, _) => AddReturn(5L * l.Last(), l));
        var dict = new Dictionary<char, long> { { '0', 0L }, { '1', 1L }, { '2', 2L }, { '-', -1L }, { '=', -2L } };

        var val = data.Select(r => Enumerable.Range(0, r.Length).Sum(i => mults[i] * dict[r[r.Length - 1 - i]])).ToList();
        var total = val.Sum();

       return SnafuFromDec(total);
    }

    static string SnafuFromDec(long value) {
            List<char> arr = new();
            while (value > 0) {
                switch (value % 5) {
                    case 3:
                        arr.Add('=');
                        value += 5;
                        break;
                    case 4:
                        arr.Add('-');
                        value += 5;
                        break;
                    default:
                        arr.Add((char)((value % 5) + '0'));
                        break;
                }
                value /= 5;
            }
            arr.Reverse();
            return new string(arr.ToArray());
        }

    public static List<T> AddReturn<T>(T val, List<T> list)
    {
        list.Add(val);
        return list;
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
using System.Diagnostics;
using System.Reflection;
using Tools;

Day22 day = new();
day.OutputFirstStar();

public class Day22 : DayBase
{
    public Day22() : base("22")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(b => b.Split('~', StringSplitOptions.RemoveEmptyEntries)).Select(v => (p1: v[0].GetIntegers(), p2: v[1].GetIntegers())).ToList();
        for (int i = 0; i < data.Count; i++)
        {
            for(int j = 0; j < 3; j++) 
                if (data[i].p1[j] > data[i].p2[j])
                    Swap(data[i].p1, data[i].p2, j);
        }
        data = data.OrderBy(b => b.p1[2]).ToList();


        var landed = new List<(int[] p1, int[] p2)>();

        foreach (var b in data)
        {
            if (b.p1[2] == 1)
            {
                landed.Add(b);
                continue;
            }

            var handled = false;
            for (int i = landed.Count - 1; i >= 0; i--)
            {
                var l = landed[i];
                var lCoords = new HashSet<(int x, int y)>( Enumerable.Range(l.p1[0], l.p2[0] - l.p1[0] + 1).SelectMany(x => Enumerable.Range(l.p1[1], l.p2[1] - l.p1[1] + 1).Select(y => (x, y))));

                if (Enumerable.Range(b.p1[0], b.p2[0] - b.p1[0] + 1).Any(x => Enumerable.Range(b.p1[1], b.p2[1] - b.p1[1] + 1).Any(y => lCoords.Contains((x, y)))))
                {
                    handled = true;
                    var diff = b.p1[2] - landed[i].p2[2];
                    Debug.Assert(diff > 0);
                    b.p1[2] = landed[i].p1[2] + 1;
                    b.p2[2] = b.p2[2] - diff + 1;
                    landed.Add(b);
                    break;
                }

            }
            if (!handled)
            {
                var diff = b.p1[2] - 1;
                b.p1[2] = 1;
                b.p2[2] = b.p2[2] - diff;
                landed.Add(b);
            }

        }

        return "";
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }

    private void Swap(int[] a, int[] b, int i)
    {
        var t = a[i];
        a[i] = b[i];
        b[i] = t;;
    }
}
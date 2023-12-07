using System.Data;
using Tools;

Day07 day = new();
day.OutputSecondStar();

public class Day07 : DayBase
{
    public Day07() : base("7")
    {
    }

    public override string FirstStar()
    {
        var cards = "AKQJT98765432".Reverse().ToList();
        var data = GetRowData();

        var dicts = data.Select(row => cards.Select(c => (c, row.Substring(0, 5).Count(h => h == c))).ToDictionary(a => a.c, a => a.Item2)).ToList();
        var order = Enumerable.Range(0, dicts.Count()).Select(i => (i, (data[i].Substring(0,5), dicts[i]))).OrderBy(d => d.Item2, new HandComparer()).ToList();
        var bets = data.Select(row => int.Parse(row.Substring(6))).ToList();

        return Enumerable.Range(1, order.Count()).Select(i => bets[order[i - 1].Item1]*i).Sum().ToString();
    }

    public override string SecondStar()
    {
        var cards = "AKQJT98765432".Reverse().ToList();
        var data = GetRowData();

        var dicts = data.Select(row => cards.Select(c => (c, row.Substring(0, 5).Count(h => h == c))).ToDictionary(a => a.c, a => a.Item2)).ToList();
        var order = Enumerable.Range(0, dicts.Count()).Select(i => (i, (data[i].Substring(0,5), dicts[i]))).OrderBy(d => d.Item2, new HandComparer2()).ToList();
        var bets = data.Select(row => int.Parse(row.Substring(6))).ToList();

        return Enumerable.Range(1, order.Count()).Select(i => bets[order[i - 1].Item1]*i).Sum().ToString();
    }

    public int Strength(Dictionary<char, int> hand) =>
        hand switch {
            var ha when ha.Any(h => h.Value == 5) || (ha.Where(ha => ha.Key != 'J').Max(h => h.Value) + ha['J'] == 5)  => 7,
            var ha when ha.Any(h => h.Value == 4) || (ha.Where(ha => ha.Key != 'J').Max(h => h.Value) + ha['J'] == 4)  => 6,
            var ha when (ha.Any(h => h.Value == 3) && ha.Any(h => h.Value == 2)) || ( ha.Count(h => h.Value == 2) == 2 && ha['J'] > 0)  => 5,
            var ha when ha.Any(h => h.Value == 3) || (ha.Where(ha => ha.Key != 'J').Max(h => h.Value) + ha['J'] == 3) => 4,
            var ha when ha.Count(h => h.Value == 2) == 2 => 3,
            var ha when ha.Any(h => h.Value == 2) || ha['J'] > 0 => 2,
            _ => 1
        };

}

public class HandComparer : IComparer<(string, Dictionary<char, int>)>
{
    readonly List<char> cards = "AKQJT98765432".Reverse().ToList();

    public int Compare((string, Dictionary<char, int>) x, (string, Dictionary<char, int>) y)
    {
        var xStrength = Strength(x.Item2);
        var yStrength = Strength(y.Item2);
        if (xStrength != yStrength)
            return xStrength.CompareTo(yStrength);  

        var index = Enumerable.Range(0, 5).First(i => x.Item1.ElementAt(i) != y.Item1.ElementAt(i));
        return cards.IndexOf(x.Item1.ElementAt(index)).CompareTo(cards.IndexOf(y.Item1.ElementAt(index)));  
    }

    public int Strength(Dictionary<char, int> hand) =>
        hand switch {
            var ha when ha.Any(h => h.Value == 5) => 7,
            var ha when ha.Any(h => h.Value == 4) => 6,
            var ha when ha.Any(h => h.Value == 3) && ha.Any(h => h.Value == 2) => 5,
            var ha when ha.Any(h => h.Value == 3) => 4,
            var ha when ha.Count(h => h.Value == 2) == 2 => 3,
            var ha when ha.Any(h => h.Value == 2) => 2,
            _ => 1
        };
}

public class HandComparer2 : IComparer<(string, Dictionary<char, int>)>
{
    readonly List<char> cards = "AKQT98765432J".Reverse().ToList();

    public int Compare((string, Dictionary<char, int>) x, (string, Dictionary<char, int>) y)
    {
        var xStrength = Strength(x.Item2);
        var yStrength = Strength(y.Item2);
        if (xStrength != yStrength)
            return xStrength.CompareTo(yStrength);  

        if (x.Item1 == y.Item1)
            return 0;

        var index = Enumerable.Range(0, 5).First(i => x.Item1.ElementAt(i) != y.Item1.ElementAt(i));
        return cards.IndexOf(x.Item1.ElementAt(index)).CompareTo(cards.IndexOf(y.Item1.ElementAt(index)));  
    }

    public int Strength(Dictionary<char, int> hand) =>
        hand switch {
            var ha when ha.Any(h => h.Value == 5) || (ha.Where(ha => ha.Key != 'J').Max(h => h.Value) + ha['J'] == 5)  => 7,
            var ha when ha.Any(h => h.Value == 4) || (ha.Where(ha => ha.Key != 'J').Max(h => h.Value) + ha['J'] == 4)  => 6,
            var ha when ha.Any(h => h.Value == 3) && ha.Any(h => h.Value == 2) || ( ha.Count(h => h.Value == 2) == 2 && ha['J'] > 0) => 5,
            var ha when ha.Any(h => h.Value == 3) || (ha.Where(ha => ha.Key != 'J').Max(h => h.Value) + ha['J'] == 3) => 4,
            var ha when ha.Count(h => h.Value == 2) == 2 => 3,
            var ha when ha.Any(h => h.Value == 2) || ha['J'] > 0 => 2,
            _ => 1
        };
}
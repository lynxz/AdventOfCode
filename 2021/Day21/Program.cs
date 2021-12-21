// See https://aka.ms/new-console-template for more information
using Tools;

Day21 day = new Day21("21");
day.OutputSecondStar();

public class Day21 : DayBase
{
    public Day21(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var p = new[] { 2, 8 };
        var points = new[] { 0, 0 };
        var rolls = 0;
        var currentPlayer = 0;
        while (points.All(po => po < 1000))
        {
            var steps = Enumerable.Range(rolls + 1, 3).Sum();
            var pos = (p[currentPlayer] + steps) % 10;
            if (pos == 0)
                pos = 10;
            p[currentPlayer] = pos;
            points[currentPlayer] += pos;

            rolls += 3;
            currentPlayer = ++currentPlayer % 2;
        }

        return (points.Min() * rolls).ToString();
    }

    Dictionary<int, int> Frequency;

    public override string SecondStar()
    {
        Frequency = Enumerable.Range(1, 3).SelectMany(i => Enumerable.Range(1, 3).SelectMany(j => Enumerable.Range(1, 3).Select(k => i + j + k)))
            .GroupBy(i => i)
            .ToDictionary(g => g.Key, g => g.Count());

        var player1Wins = CalculateUniverseToWin(new List<(int Position, long Score)> { (2, 0), (8, 0) }, 0, 0);
        var player2Wins = CalculateUniverseToWin(new List<(int Position, long Score)> { (2, 0), (8, 0) }, 0, 1);

        return Math.Max(player1Wins, player2Wins).ToString();
    }

    long CalculateUniverseToWin(List<(int Position, long Score)> players, int player, int winner)
    {
        var universes = 0L;
        foreach (var key in Frequency.Keys)
        {
            var p = players[player];

            var pos = (p.Position + key) % 10;
            if (pos == 0) pos = 10;
            var score = p.Score + pos;

            if (score >= 21)
                universes += player == winner ? Frequency[key] : 0;
            else
            {
                var newList = players.ToList();
                newList[player] = (pos, score);
                universes += Frequency[key] * CalculateUniverseToWin(newList, (player + 1) % 2, winner);
            }
        }
        return universes;
    }
}

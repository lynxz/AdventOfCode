// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day4("4");
day.PostSecondStar();

public class Day4 : DayBase
{
    public Day4(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var parts = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var nums = parts[0].Split(',').Select(n => int.Parse(n));
        var boards = GenerateBoards(parts);

        foreach (var num in nums)
        {
            MarkNumbers(boards, num);

            var winner = boards.FirstOrDefault(b => Winner(b));

            if (winner != null)
            {
                var tableSum = Enumerable.Range(0, 5).Sum(i => Enumerable.Range(0, 5).Where(j => winner[i, j] != -1).Sum(j => winner[i, j]));
                return (tableSum * num).ToString();
            }
        }

        throw new Exception("Fail");
    }


    public override string SecondStar()
    {
        var parts = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var nums = parts[0].Split(',').Select(n => int.Parse(n));
        var boards = GenerateBoards(parts);
        int[,] lastWinner = new int[5,5];
        int lastNum = 0;

        foreach (var num in nums)
        {
            MarkNumbers(boards, num);

            foreach (var winner in boards.Where(b => Winner(b)).ToList())
            {
                boards.Remove(winner);
                lastWinner = (int[,])winner.Clone();
                lastNum = num;
            }
        }

        var tableSum = Enumerable.Range(0, 5).Sum(i => Enumerable.Range(0, 5).Where(j => lastWinner[i, j] != -1).Sum(j => lastWinner[i, j]));
        return (tableSum * lastNum).ToString();
    }

    private static List<int[,]> GenerateBoards(string[] parts)
    {
        var boards = new List<int[,]>();
        foreach (var board in parts.Skip(1))
        {
            var b = new int[5, 5];
            var rows = board.Split('\n').ToArray();
            for (int i = 0; i < 5; i++)
            {
                var cols = rows[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < 5; j++)
                    b[i, j] = int.Parse(cols[j]);
            }
            boards.Add(b);
        }
        return boards;
    }

    public bool Winner(int[,] board) =>
        Enumerable.Range(0, 5).Any(i => Enumerable.Range(0, 5).All(j => board[i, j] == -1) || Enumerable.Range(0, 5).All(j => board[j, i] == -1));


    private static void MarkNumbers(List<int[,]> boards, int num)
    {
        foreach (var board in boards)
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (board[i, j] == num)
                        board[i, j] = -1;
    }
}
// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day3();
day.PostSecondStar();


public class Day3 : DayBase
{

    public Day3() : base("3")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData();
        var dict = new Dictionary<(int, int), int>();

        dict.Add((0, 0), 1);
        var current = (0, 0);
        foreach (var c in data)
        {
            switch (c)
            {
                case '^':
                    current = (current.Item1, current.Item2 - 1);
                    break;
                case 'v':
                    current = (current.Item1, current.Item2 + 1);
                    break;
                case '>':
                    current = (current.Item1 + 1, current.Item2);
                    break;
                case '<':
                    current = (current.Item1 - 1, current.Item2);
                    break;
            }

            if (!dict.ContainsKey(current))
                dict.Add(current, 0);

            dict[current]++;
        }

        return dict.Count.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRawData();
        var dict = new Dictionary<(int, int), int>();

        dict.Add((0, 0), 2);
        var santa = (0, 0);
        var robot = (0, 0);
        var santaMove = true;
        foreach (var c in data)
        {
            var current = santaMove ? santa : robot;
            switch (c)
            {
                case '^':
                    current = (current.Item1, current.Item2 - 1);
                    break;
                case 'v':
                    current = (current.Item1, current.Item2 + 1);
                    break;
                case '>':
                    current = (current.Item1 + 1, current.Item2);
                    break;
                case '<':
                    current = (current.Item1 - 1, current.Item2);
                    break;
            }

            if (!dict.ContainsKey(current))
                dict.Add(current, 0);
            if (santaMove)
                santa = current;
            else 
                robot = current;

            dict[santa]++;
            santaMove = !santaMove;
        }

        return dict.Count.ToString();
    }
}

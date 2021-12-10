// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day10("10");
day.OutputFirstStar();

public class Day10 : DayBase
{

    Dictionary<char, char> matchingBraces = new Dictionary<char, char> {
        { '(', ')'},
        { '[', ']'},
        { '{', '}'},
        { '<', '>'}
    };



    public Day10(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        Dictionary<char, int> prize = new Dictionary<char, int> {
            { ')', 3},
            { ']', 57},
            { '}', 1197},
            { '>', 25137}
        };
        var input = GetRowData();
        return input.Select(r => CheckLine(r, 0)).Where(a => a.Item3).Sum(a => prize[a.Item1]).ToString();
    }

    public override string SecondStar()
    {
        Dictionary<char, int> prize = new Dictionary<char, int> {
            { ')', 1},
            { ']', 2},
            { '}', 3},
            { '>', 4}
        };
        var input = GetRowData();
        var scores = input
            .Select(r => CheckLine(r, 0))
            .Where(a => !a.Item3)
            .Select(a => a.Item4.Aggregate(0L, (acc, c) => acc * 5 + prize[c]))
            .OrderBy(i => i)
            .ToList();
        return scores[(scores.Count / 2)].ToString();
    }

    (char, int, bool, List<char>) CheckLine(string line, int index)
    {
        while (index < line.Length)
        {
            var currentChar = line[index];
            if (matchingBraces.Values.Any(c => c == currentChar))
                return (currentChar, index, false, new List<char>());

            (char closingChar, index, bool error, List<char> missingChars) = CheckLine(line, ++index);
            if (closingChar == 'e')
            {
                missingChars.Add(matchingBraces[currentChar]);
                return (closingChar, index, error, missingChars);
            }
            else if (error)
                return (closingChar, index, error, missingChars);
            else if (matchingBraces[currentChar] != closingChar)
                return (closingChar, index, true, missingChars);

            index++;
        }
        return ('e', index, false, new List<char>());
    }
}
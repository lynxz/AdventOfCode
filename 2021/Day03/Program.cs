// See https://aka.ms/new-console-template for more information
using Tools;

Console.WriteLine("Hello, World!");

var day = new Day3("3");
day.OutputFirstStar();

public class Day3 : DayBase
{
    public Day3(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData();
        var size = input.First().Length;
        var gamma = Convert.ToInt32(string.Join("", Enumerable.Range(0, size).Select(i => input.Count(d => d[i] == '1') >= input.Length / 2 ? "1" : "0")), 2);
        var epsilon = Convert.ToInt32(string.Join("", Enumerable.Range(0, size).Select(i => input.Count(d => d[i] == '1') < input.Length / 2 ? "1" : "0")), 2);

        return (gamma * epsilon).ToString();
    }

    public override string SecondStar()
    {
        var input = GetRowData();
        var size = input.First().Length;
        var oxNumbers = new List<string>(input);
        var scrubbNumbers = new List<string>(input);


        for (int i = 0; i < size; i++)
        {
            if (oxNumbers.Count > 1)
            {
                var bit = oxNumbers.Count(d => d[i] == '1') >= oxNumbers.Count(d => d[i] == '0') ? '1' : '0';
                oxNumbers = new List<string>(oxNumbers.Where(d => d[i] == bit));
            }
            if (scrubbNumbers.Count > 1)
            {
                var bit = scrubbNumbers.Count(d => d[i] == '1') < scrubbNumbers.Count(d => d[i] == '0') ? '1' : '0';
                scrubbNumbers = new List<string>(scrubbNumbers.Where(d => d[i] == bit));
            }
        }

        var oxygen = Convert.ToInt32(oxNumbers.Single(), 2);
        var scrubb = Convert.ToInt32(scrubbNumbers.Single(), 2);

        return (oxygen * scrubb).ToString();
    }
}

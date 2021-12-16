// See https://aka.ms/new-console-template for more information
using System.Text;
using Tools;

Day16 day = new("16");
day.OutputSecondStar();

public class Day16 : DayBase
{
    public Day16(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var binString = GetBinaryString();
        var package = Package.Parse(binString);
        return package.GetVersionValue().ToString();
    }

    public override string SecondStar()
    {
        var binString = GetBinaryString();
        var package = Package.Parse(binString);
        return package.GetValue().ToString();
    }

    private string GetBinaryString()
    {
        var hex = new Dictionary<char, string>{
            {'0', "0000"},
            {'1', "0001"},
            {'2', "0010"},
            {'3', "0011"},
            {'4', "0100"},
            {'5', "0101"},
            {'6', "0110"},
            {'7', "0111"},
            {'8', "1000"},
            {'9', "1001"},
            {'A', "1010"},
            {'B', "1011"},
            {'C', "1100"},
            {'D', "1101"},
            {'E', "1110"},
            {'F', "1111"}
        };
        var input = GetRawData().Trim();
        return string.Join("", input.Select(c => hex[c]));
    }
}

public class Package
{

    public int Version { get; set; }

    public int TypeId { get; set; }

    public long Value { get; set; }

    public List<Package> SubPackages { get; } = new List<Package>();

    public int GetVersionValue() => Version + SubPackages.Sum(p => p.GetVersionValue());

    public long GetValue()
    {
        switch (TypeId)
        {
            case 0:
                return SubPackages.Sum(p => p.GetValue());
            case 1:
                return SubPackages.Aggregate(1L, (acc, p) => acc * p.GetValue());
            case 2:
                return SubPackages.Min(p => p.GetValue());
            case 3:
                return SubPackages.Max(p => p.GetValue());
            case 4:
                return Value;
            case 5:
                return SubPackages[0].GetValue() > SubPackages[1].GetValue() ? 1L : 0L;
            case 6:
                return SubPackages[0].GetValue() < SubPackages[1].GetValue() ? 1L : 0L;
            case 7:
                return SubPackages[0].GetValue() == SubPackages[1].GetValue() ? 1L : 0L;
        }

        throw new Exception();
    }

    static public Package Parse(string binString)
    {
        int index = 0;
        return Package.Parse(binString, ref index);
    }

    static private Package Parse(string binString, ref int index) {
        Package package = new();
        package.InternalParsing(binString, ref index);
        return package;
    }

    private void InternalParsing(string binString, ref int index)
    {

        Version = Convert.ToInt32(binString.Substring(index, 3), 2);
        TypeId = Convert.ToInt32(binString.Substring(index + 3, 3), 2);

        index += 6;

        if (TypeId == 4)
            index = ParseValue(binString, index);
        else
            index = binString[index++] == '0' ? ParseBitLengthOperator(binString, index) : ParseNumberOfOperator(binString, index);
    }

    private int ParseNumberOfOperator(string binString, int index)
    {
        var noOfPackages = Convert.ToInt32(binString.Substring(index, 11), 2);
        index += 11;
        SubPackages.AddRange(Enumerable.Range(0, noOfPackages).Select(i => Package.Parse(binString, ref index)));
        return index;
    }

    private int ParseBitLengthOperator(string binString, int index)
    {
        var len = Convert.ToInt32(binString.Substring(index, 15), 2);
        index += 15;
        var subPackages = binString.Substring(index, len);
        index += len;
        int newIndexLen = 0;
        while (newIndexLen < len)
            SubPackages.Add(Package.Parse(subPackages, ref newIndexLen));

        return index;
    }

    private int ParseValue(string binString, int index)
    {
        var valString = string.Empty;
        StringBuilder sb = new();
        do
        {
            valString = binString.Substring(index, 5);
            index += 5;
            sb.Append(valString.Substring(1));
        } while (valString.First() == '1');
        Value = Convert.ToInt64(sb.ToString(), 2);
        return index;
    }
}
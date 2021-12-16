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

    private string GetBinaryString() =>
        string.Join("", GetRawData().Trim().Select(c => Convert.ToString(Convert.ToInt64(c.ToString(), 16), 2).PadLeft(4, '0')));

}

public class Package
{

    public int Version { get; set; }

    public int TypeId { get; set; }

    public long Value { get; set; }

    public List<Package> SubPackages { get; } = new List<Package>();

    public int GetVersionValue() => Version + SubPackages.Sum(p => p.GetVersionValue());

    public long GetValue() =>
        TypeId switch
        {
            0 => SubPackages.Sum(p => p.GetValue()),
            1 => SubPackages.Aggregate(1L, (acc, p) => acc * p.GetValue()),
            2 => SubPackages.Min(p => p.GetValue()),
            3 => SubPackages.Max(p => p.GetValue()),
            4 => Value,
            5 => SubPackages[0].GetValue() > SubPackages[1].GetValue() ? 1L : 0L,
            6 => SubPackages[0].GetValue() < SubPackages[1].GetValue() ? 1L : 0L,
            7 => SubPackages[0].GetValue() == SubPackages[1].GetValue() ? 1L : 0L
        };

    static public Package Parse(string binString)
    {
        int index = 0;
        return Package.Parse(binString, ref index);
    }

    static private Package Parse(string binString, ref int index)
    {
        Package package = new();
        package.InternalParsing(binString, ref index);
        return package;
    }

    private void InternalParsing(string binString, ref int index)
    {
        Version = Convert.ToInt32(binString[index..(index + 3)], 2);
        TypeId = Convert.ToInt32(binString[(index + 3)..(index + 6)], 2);
        index += 6;

        if (TypeId == 4)
            index = ParseValue(binString, index);
        else
            index = binString[index++] == '0' ? ParseBitLengthOperator(binString, index) : ParseNumberOfOperator(binString, index);
    }

    private int ParseNumberOfOperator(string binString, int index)
    {
        var noOfPackages = Convert.ToInt32(binString[index..(index + 11)], 2);
        index += 11;
        SubPackages.AddRange(Enumerable.Range(0, noOfPackages).Select(i => Package.Parse(binString, ref index)));
        return index;
    }

    private int ParseBitLengthOperator(string binString, int index)
    {
        var len = Convert.ToInt32(binString[index..(index + 15)], 2);
        var subPackages = binString[(index + 15)..(index + len + 15)];
        index += (len + 15);

        int newIndexLen = 0;
        while (newIndexLen < len)
            SubPackages.Add(Package.Parse(subPackages, ref newIndexLen));

        return index;
    }

    private int ParseValue(string binString, int index)
    {
        StringBuilder sb = new();
        do
        {
            sb.Append(binString[(index + 1)..(index + 5)]);
            index += 5;
        } while (binString[index - 5] == '1');
        Value = Convert.ToInt64(sb.ToString(), 2);
        return index;
    }
}
using Tools;

var day = new Day22();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day22 : DayBase
{
    public Day22() : base("22")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => uint.Parse(r.Trim())).ToList();

        for (int i = 0; i < 2000; i++)
        {
            for (int j = 0; j < data.Count; j++)
            {
                var next = GetNext(data[j]);
                data[j] = next;
            }
        }
        return data.Select(d => Convert.ToInt64(d)).Sum().ToString();
    }

    uint GetNext(uint secret)
    {
        var temp = secret * 64;
        secret = MixAndPrune(secret, temp);
        temp = secret / 32;
        secret = MixAndPrune(secret, temp);
        temp = secret * 2048;
        secret = MixAndPrune(secret, temp);

        return secret;
    }

    uint MixAndPrune(uint secret, uint mix) =>
        (secret ^ mix) % 16777216;

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => uint.Parse(r.Trim())).ToList();

        var sumDictionary = new Dictionary<string, int>();
        foreach (var vendor in data)
        {
            int[] queue = [0, 0, 0, 0];
            var prev = vendor;
            var vendorDict = new Dictionary<string, int>();
            for (int i = 0; i < 1999; i++)
            {
                var next = GetNext(prev);
                var nextValue = next % 10;
                var prevValue = prev % 10;
                var diff = (int)nextValue - (int)prevValue;
                prev = next;
                queue[0] = queue[1];
                queue[1] = queue[2];
                queue[2] = queue[3];
                queue[3] = diff;
                var key = string.Join("", queue);
                if (i >= 3 && !vendorDict.ContainsKey(key))
                {
                    vendorDict[key] = Convert.ToInt32(nextValue);
                }
            }

            foreach (var key in vendorDict.Keys)
            {
                if (!sumDictionary.ContainsKey(key))
                {
                    sumDictionary[key] = vendorDict[key];
                }
                else
                {
                    sumDictionary[key] += vendorDict[key];
                }
            }
        }

        return sumDictionary
            .OrderByDescending(kvp => kvp.Value)
            .First()
            .Value
            .ToString();
    }
}
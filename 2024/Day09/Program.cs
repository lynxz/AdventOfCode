using System.Numerics;
using Tools;

var day = new Day09();
day.OutputFirstStar();

public class Day09 : DayBase
{
    public Day09() : base("9")
    {
    }

    public override string FirstStar()
    {
        var data = GetRawData().Trim();
        var files = Enumerable.Range(0, data.Length).Where(i => i % 2 == 0).Select(i => int.Parse(data[i].ToString())).ToList();
        var space = Enumerable.Range(0, data.Length).Where(i => i % 2 != 0).Select(i => int.Parse(data[i].ToString())).ToList();

        BigInteger sum = new BigInteger(0);
        var diskPos = files[0];
        var filePos = files.Count - 1;
        var i = 1;
        var freeIndex = Enumerable.Range(0, space.Count).First(j => space[j] != 0);
        while (freeIndex < filePos - 1)
        {

            var m = Math.Min(space[freeIndex], files[filePos]);
            Enumerable.Range(diskPos, m).ToList().ForEach(j => sum += j * filePos);
            diskPos += m;
            files[filePos] -= m;
            space[freeIndex] -= m;

            if (files[filePos] == 0) {
                filePos--;
            }
            if (space[freeIndex] == 0)
            {
                Enumerable.Range(diskPos, files[i]).ToList().ForEach(j => sum += i * j);
                diskPos += files[i];
                i++;
                freeIndex++;
            }
            //freeIndex = Enumerable.Range(0, space.Count).First(j => space[j] != 0);
        }
        if (space[freeIndex] != 0 && files[filePos] != 0)
        {
            var m = Math.Min(space[freeIndex], files[filePos]);
            Enumerable.Range(diskPos, m).ToList().ForEach(j => sum += j * filePos);
            diskPos += m;
            files[filePos] -= m;
            space[freeIndex] -= m;
        }
        if (files[filePos] != 0)
        {
            Enumerable.Range(diskPos, files[filePos]).ToList().ForEach(j => sum += filePos * j);
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
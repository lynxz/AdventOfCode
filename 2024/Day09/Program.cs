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

        var disk = new List<int>();


        var diskPos = 0;
        bool done = false;
        for (int i = 0; i < files.Count; i++) {
           disk.AddRange(Enumerable.Repeat(i, files[i]));
            var processed = false;
            var file = (id: files.Count - i - 1, c: files[files.Count - i - 1]);
            while (!processed) {
                var freeIndex = Enumerable.Range(0, space.Count).First(i => space[i] == 0);
                if (freeIndex >= i) {
                    done = true;
                    break;
                }
                if (space[freeIndex] >= file.c) {
                    space[freeIndex] -= file.c;
                    disk.AddRange(Enumerable.Repeat(file.id, file.c));
                    processed = true;
                } else {
                    file.c -= space[freeIndex];
                    disk.AddRange(Enumerable.Repeat(file.id, space[freeIndex]));
                    space[freeIndex] = 0;
                }
                diskPos++;
            }
            if (done) {
                break;
            }
        }

        return Enumerable.Range(0, disk.Count).Sum(i => (long)i*disk[i]).ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
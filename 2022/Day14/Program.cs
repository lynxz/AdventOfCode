using Tools;

Day14 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day14 : DayBase
{
    public Day14() : base("14")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var lines = data.Select(d => d.Split(" -> ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.GetIntegers()).ToList()).ToList();
        var maxX = lines.SelectMany(l => l.Select(x => x[0])).Max();
        var maxY = lines.SelectMany(l => l.Select(y => y[1])).Max();
        var minX = lines.SelectMany(l => l.Select(x => x[0])).Min();

        var m = Enumerable.Range(0, maxY + 1).Select(_ => Enumerable.Range(minX, maxX-minX + 1).Select(_ => '.')).ToMultidimensionalArray() ?? new char[0,0];

        foreach(var line in lines) {
            foreach(var pair in line.Window(2)) {
                var xStart = Math.Min(pair[0][0], pair[1][0]) - minX;
                var xEnd = Math.Max(pair[0][0], pair[1][0]) - minX;
                var yStart = Math.Min(pair[0][1], pair[1][1]);
                var yEnd = Math.Max(pair[0][1], pair[1][1]);
                for (int y = yStart; y <= yEnd; y++) {
                    for(int x = xStart; x <= xEnd; x++){
                        m[y,x] = '#';
                    }
                }
            }
        }

        var count = 0;
        var done = false;
        while (!done) {
            var s = new int[] {500 - minX, 0};
            var innerDone = false;
            while (!innerDone) {
                if (s[1]+1 >= m.GetLength(0)) {
                    innerDone = true;
                    done = true;
                }
                else if (m[s[1]+1, s[0]] == '.') {
                    s[1]++;
                }
                else if(s[0] - 1 < 0) {
                    innerDone = true;
                    done = true;
                }
                else if(m[s[1]+1, s[0]-1] == '.') {
                    s[1]++;
                    s[0]--;
                }
                else if(s[0] + 1 >= m.GetLength(1)) {
                    innerDone = true;
                    done = true;
                }
                else if(m[s[1]+1, s[0]+1] == '.') {
                    s[1]++;
                    s[0]++;
                } else {
                    count++;
                    m[s[1],s[0]] = 'o';
                    innerDone = true;
                }
            }
        }

        return count.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var lines = data.Select(d => d.Split(" -> ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.GetIntegers()).ToList()).ToList();
        var maxY = lines.SelectMany(l => l.Select(y => y[1])).Max() + 2;
        var maxX = (maxY * 2) + 3;
        var offset = maxX / 2;

        var m = Enumerable.Range(0, maxY + 1).Select(_ => Enumerable.Range(0, maxX).Select(_ => '.')).ToMultidimensionalArray() ?? new char[0,0];

        foreach(var line in lines) {
            foreach(var pair in line.Window(2)) {
                var xStart = Math.Min(pair[0][0], pair[1][0]) - 500 + offset;
                var xEnd = Math.Max(pair[0][0], pair[1][0]) - 500 + offset;
                var yStart = Math.Min(pair[0][1], pair[1][1]);
                var yEnd = Math.Max(pair[0][1], pair[1][1]);
                for (int y = yStart; y <= yEnd; y++) {
                    for(int x = xStart; x <= xEnd; x++){
                        m[y,x] = '#';
                    }
                }
            }
        }

        for (int x = 0; x < m.GetLength(1); x++) {
            m[maxY, x] = '#';
        }

        var count = 0;
        var done = false;
        while (!done) {
            var s = new int[] {offset, 0};
            var innerDone = false;
            while (!innerDone) {
                if (m[0, offset] == 'o') {
                    innerDone = true;
                    done = true;
                }
                else if (s[1]+1 >= m.GetLength(0)) {
                    throw new Exception($"Y: {s[1]}, X: {s[0]}");
                }
                else if (m[s[1]+1, s[0]] == '.') {
                    s[1]++;
                }
                else if(s[0] - 1 < 0) {
                    throw new Exception($"Y: {s[1]}, X: {s[0]}");
                }
                else if(m[s[1]+1, s[0]-1] == '.') {
                    s[1]++;
                    s[0]--;
                }
                else if(s[0] + 1 >= m.GetLength(1)) {
                    throw new Exception($"Y: {s[1]}, X: {s[0]}");
                }
                else if(m[s[1]+1, s[0]+1] == '.') {
                    s[1]++;
                    s[0]++;
                } else {
                    count++;
                    m[s[1],s[0]] = 'o';
                    innerDone = true;
                }
            }
        }

        return count.ToString();
    }
}
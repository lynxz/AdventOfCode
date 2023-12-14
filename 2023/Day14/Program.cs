using Tools;

Day14 day = new();
day.OutputSecondStar();

public class Day14 : DayBase
{
    public Day14() : base("14")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().ToMultidimensionalArray();
        var tilted = Enumerable.Range(0, data.GetLength(0)).Select(y => Enumerable.Range(0, data.GetLength(1)).Select(x => data[y, x] == 'O' ? '.' : data[y, x])).ToMultidimensionalArray();

        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                if (data[y, x] == 'O')
                {
                    var yOffset = 0;
                    while (y - yOffset - 1 > -1 && tilted[y - yOffset - 1, x] != 'O' && tilted[y - yOffset - 1, x] != '#')
                    {
                        yOffset++;
                    }
                    tilted[y - yOffset, x] = 'O';
                }
            }
        }
        //tilted.Print();

        return Enumerable.Range(0, data.GetLength(0)).Select(y => Enumerable.Range(0, data.GetLength(1)).Sum(x => (tilted[y, x] == 'O' ? 1 : 0) * (tilted.GetLength(0) - y))).Sum().ToString();

    }

    public override string SecondStar()
    {
        var data = GetRowData().ToMultidimensionalArray();

        for (int c =0; c < 204; c++)
        {
            for (int i = 0; i < 4; i++)
            {
                data = Tilt(data, (Dir)i);
            }

           var v = Enumerable.Range(0, data.GetLength(0)).Select(y => Enumerable.Range(0, data.GetLength(1)).Sum(x => (data[y, x] == 'O' ? 1 : 0) * (data.GetLength(0) - y))).Sum().ToString();
           System.Console.WriteLine(v);
        }
        
        System.Console.WriteLine("");
        return Enumerable.Range(0, data.GetLength(0)).Select(y => Enumerable.Range(0, data.GetLength(1)).Sum(x => (data[y, x] == 'O' ? 1 : 0) * (data.GetLength(0) - y))).Sum().ToString();
    }

    private static char[,]? Tilt(char[,]? data, Dir dir = Dir.North)
    {
        var tilted = Enumerable.Range(0, data.GetLength(0)).Select(y => Enumerable.Range(0, data.GetLength(1)).Select(x => data[y, x] == 'O' ? '.' : data[y, x])).ToMultidimensionalArray();

        if (dir == Dir.North)
        {
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (data[y, x] == 'O')
                    {
                        var yOffset = 0;
                        while (y - yOffset - 1 > -1 && tilted[y - yOffset - 1, x] != 'O' && tilted[y - yOffset - 1, x] != '#')
                        {
                            yOffset++;
                        }
                        tilted[y - yOffset, x] = 'O';
                    }
                }
            }
        }
        else if (dir == Dir.East)
        {
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = data.GetLength(1) -1; x >= 0; x--)
                {
                    if (data[y, x] == 'O')
                    {
                        var xOffset = 0;
                        while (x + xOffset + 1 < data.GetLength(1) && tilted[y, x + xOffset + 1] != 'O' && tilted[y, x + xOffset + 1] != '#')
                        {
                            xOffset++;
                        }
                        tilted[y, x + xOffset] = 'O';
                    }
                }
            }
        }
        else if (dir == Dir.South)
        {
            for (int y = data.GetLength(0)-1; y  >= 0; y--)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (data[y, x] == 'O')
                    {
                       var yOffset = 0;
                        while (y + yOffset + 1 < data.GetLength(0) && tilted[y + yOffset + 1, x] != 'O' && tilted[y + yOffset + 1, x] != '#')
                        {
                            yOffset++;
                        }
                        tilted[y + yOffset, x] = 'O';
                    }
                }
            }
        } else {
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (data[y, x] == 'O')
                    {
                        var xOffset = 0;
                        while (x - xOffset - 1 > -1 && tilted[y, x - xOffset - 1] != 'O' && tilted[y, x - xOffset - 1] != '#')
                        {
                            xOffset++;
                        }
                        tilted[y, x - xOffset] = 'O';
                    }
                }
            }
        }


        return tilted;
    }

    enum Dir { North, West, South, East };
}
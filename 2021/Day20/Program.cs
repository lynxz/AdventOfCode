// See https://aka.ms/new-console-template for more information
using Tools;

Day20 day = new("20");
day.OutputSecondStar();

public class Day20 : DayBase
{
    public Day20(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var enhancement = input[0];
        var rows = input[1].Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var image = Enumerable.Range(0, rows.Length).Select(y => Enumerable.Range(0, rows[0].Length).Select(x => rows[y][x])).ToMultidimensionalArray();

        image = GrowImage(image, 2);
        for (int i = 0; i < 2; i++)
            image = Enhance(image, enhancement);

        return Enumerable.Range(1, image.GetLength(0)-1).Sum(y => Enumerable.Range(1, image.GetLength(1)-1).Count(x => image[y, x] == '#')).ToString();
    }

    public override string SecondStar()
    {
        var input = GetRawData().Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var enhancement = input[0];
        var rows = input[1].Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var image = Enumerable.Range(0, rows.Length).Select(y => Enumerable.Range(0, rows[0].Length).Select(x => rows[y][x])).ToMultidimensionalArray();

        image = GrowImage(image, 2);
        for (int i = 0; i < 50; i++)
            image = Enhance(image, enhancement);

        return Enumerable.Range(1, image.GetLength(0)-1).Sum(y => Enumerable.Range(1, image.GetLength(1)-1).Count(x => image[y, x] == '#')).ToString();
    }

    char[,] Enhance(char[,] image, string enhancement)
    {
        var floodChar = image[0,0] == '.' ? enhancement[0] : enhancement[511];
        var enhancedImage = Enumerable.Range(0, image.GetLength(0)+2).Select(y => Enumerable.Range(0, image.GetLength(1)+2).Select(x => floodChar)).ToMultidimensionalArray();
        for (int y = 1; y < image.GetLength(0) - 1; y++)
        {
            for (int x = 1; x < image.GetLength(1) - 1; x++)
            {
                var pos = Enumerable.Range(-1, 3).SelectMany(yOffset => Enumerable.Range(-1, 3).Select(xOffset => image[y + yOffset, x + xOffset] == '.' ? 0 : 1))
                    .Aggregate(0, (acc, i) => (acc << 1) + i);
                enhancedImage[y+1, x+1] = enhancement[pos];
            }
        }
        return enhancedImage;
    }

    char[,] GrowImage(char[,] array, int size)
    {
        var newArray = Enumerable.Range(0, array.GetLength(0) + 2 * size).Select(y => Enumerable.Range(0, array.GetLength(1) + 2 * size).Select(x => '.')).ToMultidimensionalArray();
        for (int y = 0; y < array.GetLength(0); y++)
            for (int x = 0; x < array.GetLength(1); x++)
                newArray[y + size, x + size] = array[y, x];
        return newArray;
    }

}
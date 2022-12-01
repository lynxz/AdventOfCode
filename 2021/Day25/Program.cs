// See https://aka.ms/new-console-template for more information
using Tools;

Day25 day = new("25");
day.OutputFirstStar();

public class Day25 : DayBase
{
    public Day25(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData().Select(r => r.Trim()).ToArray();
        var matrix = Enumerable.Range(0, input.Length).Select(y => Enumerable.Range(0, input.First().Length).Select(x => input[y][x])).ToMultidimensionalArray();

        var step = 0;
        bool stopped = false;

        while (!stopped)
        {
            step++;
            stopped = true;
            var newMatrix = new char[matrix.GetLength(0), matrix.GetLength(1)];

            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    var cu = matrix[y, x];
                    if (cu == '>')
                    {
                        var nextX = (x + 1) % matrix.GetLength(1);
                        if (matrix[y, nextX] == '.')
                        {
                            newMatrix[y, nextX] = '>';
                            newMatrix[y, x] = '.';
                            x++;
                            stopped = false;
                        }
                        else
                            newMatrix[y, x] = matrix[y, x];
                    }
                    else
                        newMatrix[y, x] = matrix[y, x];
                }
            }

            matrix = newMatrix;
            newMatrix = new char[matrix.GetLength(0), matrix.GetLength(1)];
        
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                for (int y = 0; y < matrix.GetLength(0); y++)
                {
                    var cu = matrix[y, x];
                    if (cu == 'v')
                    {
                        var nextY = (y + 1) % matrix.GetLength(0);
                        if (matrix[nextY, x] == '.')
                        {
                            newMatrix[nextY, x] = 'v';
                            newMatrix[y, x] = '.';
                            y++;
                            stopped = false;
                        }
                        else
                            newMatrix[y, x] = matrix[y, x];
                    }
                    else
                        newMatrix[y, x] = matrix[y, x];
                }
            }

            matrix = newMatrix;
        }

        return step.ToString();
    }

    public override string SecondStar()
    {
        throw new NotImplementedException();
    }
}
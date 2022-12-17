using Tools;

Day17 day = new();
day.OutputSecondStar();

public class Day17 : DayBase
{
    public Day17() : base("17")
    {

    }

    public override string FirstStar()
    {
        return Simulate(2022);
    }

    public override string SecondStar()
    {
       return Simulate(1000000000000);
    }

    private string Simulate(long steps)
    {
        var jets = GetRawData().Trim();
        const int maxDepth = 2022 * 4;
        var chamber = Enumerable.Range(0, 7).Select(__ => Enumerable.Range(0, maxDepth).Select(_ => '.')).ToMultidimensionalArray() ?? new char[0, 0];
        var lastSeen = new int[jets.Length * 32];
        var lastHeight = new int[jets.Length * 32];
        var cycleLength = 0L;
        var lastDiff = 0L;
        var cycleHeight = 0L;
        var rocks = new List<List<int[]>> {
            new List<int[]> { new [] {0,0}, new [] {0,1}, new [] {0,2}, new [] {0,3}},
            new List<int[]> { new [] {0,1}, new [] {1,0}, new [] {1,1}, new [] {1,2}, new [] {2,1} },
            new List<int[]> { new [] {0,2}, new [] {1,2}, new [] {2,0}, new [] {2,1}, new [] {2,2} },
            new List<int[]> { new [] {0,0}, new [] {1,0}, new [] {2,0}, new [] {3,0}},
            new List<int[]> { new [] {0,0}, new [] {0,1}, new [] {1,0}, new [] {1,1}},
            };

        var jetPosition = 0;
        var currentHeight = 0;
        for (int step = 0; step < steps; step++)
        {
            var rockY = chamber.GetLength(1) - currentHeight - 4;
            var rockX = 2;
            var rockSelect = step % 5;
            var rock = rocks[rockSelect];
            var atRest = false;
            while (!atRest)
            {
                var jet = jets[jetPosition];
                if (CanMoveSideways(chamber, rock, rockX, rockY, jet == '<'))
                {
                    rockX += jet == '<' ? -1 : 1;
                }
                atRest = CanMoveDown(chamber, rock, rockX, rockY);
                if (!atRest)
                {
                    rockY++;
                }
                jetPosition = (jetPosition + 1) % jets.Length;
            }

            var b = rock.Max(r => r[0]);
            foreach (var coord in rock)
            {
                chamber[rockX + coord[1], rockY - (b - coord[0])] = '#';
            }
            currentHeight = chamber.GetLength(1) - Enumerable.Range(0, chamber.GetLength(1)).First(y => Enumerable.Range(0, 7).Any(x => chamber[x, y] == '#'));

            var hash = (jetPosition << 5) + rockSelect;
            if (lastSeen[hash] != 0)
            {
                long cycle = step - lastSeen[hash];
                long height = currentHeight - lastHeight[hash];
                if (lastDiff == 0)
                    lastDiff = cycle;
                if (lastDiff == cycle)
                    cycleLength++;
                if (cycleLength > 10 && (step % lastDiff) == ((steps - 1) % lastDiff))
                {
                    var numberOfRounds = (steps - step) / lastDiff;
                    cycleHeight = numberOfRounds * height;
                    break;
                }
            }
            else
            {
                cycleLength = 0L;
                lastDiff = 0L;
            }
            lastSeen[hash] = step;
            lastHeight[hash] = currentHeight;
        }
        return (currentHeight + cycleHeight).ToString();
    }

    void Draw(char[,] chamber, int currentHeight)
    {
        for (int y = chamber.GetLength(1) - currentHeight - 2; y < chamber.GetLength(1); y++)
        {
            for (int x = 0; x < 7; x++)
            {
                Console.Write(chamber[x, y]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    static bool CanMoveSideways(char[,] chamber, List<int[]> rock, int x, int y, bool left)
    {
        var b = rock.Max(r => r[0]);
        var s = left ? 0 : rock.Max(r => r[1]);
        var offset = left ? -1 : 1;
        if (x + s + offset < 0 || x + s + offset >= chamber.GetLength(0))
            return false;
        if (rock.Any(r => chamber[r[1] + x + offset, y - (b - r[0])] == '#'))
            return false;

        return true;
    }

    static bool CanMoveDown(char[,] chamber, List<int[]> rock, int x, int y)
    {
        var b = rock.Max(r => r[0]);
        if (y + 1 >= chamber.GetLength(1))
            return true;
        if (rock.Any(r => chamber[r[1] + x, y - (b - r[0]) + 1] == '#'))
            return true;

        return false;
    }

}

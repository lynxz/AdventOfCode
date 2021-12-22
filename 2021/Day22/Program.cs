// See https://aka.ms/new-console-template for more information
using Tools;

Day22 day = new Day22("22");
day.OutputSecondStar();

public class Day22 : DayBase
{
    public Day22(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r =>
        {
            var parts = r.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return (On: parts[0] == "on", Coords: parts[1].GetIntegers());
        }).Take(20).ToList();

        List<int[]> areas = new();
        foreach (var cubeStates in data)
        {
            if (cubeStates.On)
                TurnOn(areas, cubeStates.Coords);
            else
                TurnOff(areas, cubeStates.Coords);
        }

        return areas.Sum(a => (1 + a[1] - a[0]) * (1 + a[3] - a[2]) * (1 + a[5] - a[4])).ToString();
    }

    private static int[] CalculateEngulfed(int[] coords1, int[] coords2)
    {
        if ((coords1[0] >= coords2[0] && coords1[0] <= coords2[1]) || (coords1[1] >= coords2[0] && coords1[1] <= coords2[1]) &&
            (coords1[2] >= coords2[2] && coords1[2] <= coords2[3]) || (coords1[3] >= coords2[2] && coords1[3] <= coords2[3]) &&
            (coords1[4] >= coords2[4] && coords1[4] <= coords2[5]) || (coords1[5] >= coords2[4] && coords1[5] <= coords2[5]))
        {
            var engulfed = new int[6];
            engulfed[0] = Math.Max(coords1[0], coords2[0]);
            engulfed[1] = Math.Min(coords1[1], coords2[1]);
            engulfed[2] = Math.Max(coords1[2], coords2[2]);
            engulfed[3] = Math.Min(coords1[3], coords2[3]);
            engulfed[4] = Math.Max(coords1[4], coords2[4]);
            engulfed[5] = Math.Min(coords1[5], coords2[5]);
            return engulfed;
        }
        return Enumerable.Empty<int>().ToArray();
    }

    private void TurnOff(List<int[]> areas, int[] coords)
    {
        List<int[]> remove = new();
        List<int[]> newCubes = new();
        foreach (var litArea in areas)
        {
            if ((coords[0] <= litArea[0] && coords[0] >= litArea[1]) && (coords[1] <= litArea[0] && coords[1] >= litArea[1]) &&
                (coords[2] <= litArea[2] && coords[2] >= litArea[3]) && (coords[3] <= litArea[2] && coords[3] >= litArea[3]) &&
                (coords[4] <= litArea[4] && coords[4] >= litArea[5]) && (coords[5] <= litArea[4] && coords[5] >= litArea[5]))
            {
                remove.Add(litArea);
                continue;
            }
            else if (
                    ((coords[0] >= litArea[0] && coords[0] <= litArea[1]) || (coords[1] >= litArea[0] && coords[1] <= litArea[1]) || (coords[0] <= litArea[0] && coords[1] >= litArea[1])) &&
                    ((coords[2] >= litArea[2] && coords[2] <= litArea[3]) || (coords[3] >= litArea[2] && coords[3] <= litArea[3]) || (coords[2] <= litArea[2] && coords[3] >= litArea[3])) &&
                    ((coords[4] >= litArea[4] && coords[4] <= litArea[5]) || (coords[5] >= litArea[4] && coords[5] <= litArea[5]) || (coords[4] <= litArea[4] && coords[5] >= litArea[5])))
            {
                remove.Add(litArea);
                var engulfed = new int[6];
                engulfed[0] = Math.Max(coords[0], litArea[0]);
                engulfed[1] = Math.Min(coords[1], litArea[1]);
                engulfed[2] = Math.Max(coords[2], litArea[2]);
                engulfed[3] = Math.Min(coords[3], litArea[3]);
                engulfed[4] = Math.Max(coords[4], litArea[4]);
                engulfed[5] = Math.Min(coords[5], litArea[5]);

                if (engulfed[0] <= litArea[0] && engulfed[1] >= litArea[1] &&
                    engulfed[2] <= litArea[2] && engulfed[3] >= litArea[3] &&
                    engulfed[4] <= litArea[4] && engulfed[5] >= litArea[5])
                {
                    continue;
                }

                var newXCoords = new List<int[]>();
                newXCoords.Add(engulfed.Take(2).ToArray());
                if (litArea[0] < engulfed[0])
                    newXCoords.Add(new[] { litArea[0], engulfed[0] - 1 });
                if (litArea[1] > engulfed[1])
                    newXCoords.Add(new[] { engulfed[1] + 1, litArea[1] });

                var newYCoords = new List<int[]>();
                newYCoords.Add(engulfed.Skip(2).Take(2).ToArray());
                if (litArea[2] < engulfed[2])
                    newYCoords.Add(new[] { litArea[2], engulfed[2] -1 });
                if (litArea[3] > engulfed[3])
                    newYCoords.Add(new[] { engulfed[3] + 1, litArea[3] });

                var newZCoords = new List<int[]>();
                newZCoords.Add(engulfed.Skip(4).Take(2).ToArray());
                if (litArea[4] < engulfed[4])
                    newZCoords.Add(new[] { litArea[4], engulfed[4] -1 });
                if (litArea[5] > engulfed[5])
                    newZCoords.Add(new[] { engulfed[5] + 1, litArea[5] });

                var newCubesLocal = newXCoords.SelectMany(x => newYCoords.SelectMany(y => newZCoords.Select(z => x.Concat(y).Concat(z).ToArray())))
                    .Where(c => !Enumerable.Range(0, 6).All(i => c[i] == engulfed[i]))
                    .ToList();
                newCubes.AddRange(newCubesLocal);
            }
        }
        areas.AddRange(newCubes);
        remove.ForEach(r => areas.Remove(r));
    }

    private static void TurnOn(List<int[]> areas, int[] coords)
    {
        List<int[]> cubes = new() { coords };
        List<int[]> remove = new();
        foreach (var litArea in areas)
        {
            List<int[]> newCubes = new();
            foreach (var cube in cubes)
            {
                if ((cube[0] >= litArea[0] && cube[0] <= litArea[1]) && (cube[1] >= litArea[0] && cube[1] <= litArea[1]) &&
                    (cube[2] >= litArea[2] && cube[2] <= litArea[3]) && (cube[3] >= litArea[2] && cube[3] <= litArea[3]) &&
                    (cube[4] >= litArea[4] && cube[4] <= litArea[5]) && (cube[5] >= litArea[4] && cube[5] <= litArea[5]))
                {
                    continue;
                }
                else if (
                    ((cube[0] >= litArea[0] && cube[0] <= litArea[1]) || (cube[1] >= litArea[0] && cube[1] <= litArea[1]) || (cube[0] <= litArea[0] && cube[1] >= litArea[1])) &&
                    ((cube[2] >= litArea[2] && cube[2] <= litArea[3]) || (cube[3] >= litArea[2] && cube[3] <= litArea[3]) || (cube[2] <= litArea[2] && cube[3] >= litArea[3])) &&
                    ((cube[4] >= litArea[4] && cube[4] <= litArea[5]) || (cube[5] >= litArea[4] && cube[5] <= litArea[5]) || (cube[4] <= litArea[4] && cube[5] >= litArea[5])))
                {
                    var engulfed = new int[6];
                    engulfed[0] = Math.Max(cube[0], litArea[0]);
                    engulfed[1] = Math.Min(cube[1], litArea[1]);
                    engulfed[2] = Math.Max(cube[2], litArea[2]);
                    engulfed[3] = Math.Min(cube[3], litArea[3]);
                    engulfed[4] = Math.Max(cube[4], litArea[4]);
                    engulfed[5] = Math.Min(cube[5], litArea[5]);

                    if (engulfed[0] <= litArea[0] && engulfed[1] >= litArea[1] &&
                        engulfed[2] <= litArea[2] && engulfed[3] >= litArea[3] &&
                        engulfed[4] <= litArea[4] && engulfed[5] >= litArea[5])
                    {
                        newCubes.Add(cube);
                        remove.Add(litArea);
                        continue;
                    }

                    var newXCoords = new List<int[]>();
                    newXCoords.Add(engulfed.Take(2).ToArray());
                    if (cube[0] < engulfed[0])
                        newXCoords.Add(new[] { cube[0], engulfed[0] - 1 });
                    if (cube[1] > engulfed[1])
                        newXCoords.Add(new[] { engulfed[1] + 1, cube[1] });

                    var newYCoords = new List<int[]>();
                    newYCoords.Add(engulfed.Skip(2).Take(2).ToArray());
                    if (cube[2] < engulfed[2])
                        newYCoords.Add(new[] { cube[2], engulfed[2] -1 });
                    if (cube[3] > engulfed[3])
                        newYCoords.Add(new[] { engulfed[3] + 1, cube[3] });

                    var newZCoords = new List<int[]>();
                    newZCoords.Add(engulfed.Skip(4).Take(2).ToArray());
                    if (cube[4] < engulfed[4])
                        newZCoords.Add(new[] { cube[4], engulfed[4] -1 });
                    if (cube[5] > engulfed[5])
                        newZCoords.Add(new[] { engulfed[5] + 1, cube[5] });

                    var newCubesLocal = newXCoords.SelectMany(x => newYCoords.SelectMany(y => newZCoords.Select(z => x.Concat(y).Concat(z).ToArray())))
                        .Where(c => !Enumerable.Range(0, 6).All(i => c[i] == engulfed[i]))
                        .ToList();
                    newCubes.AddRange(newCubesLocal);
                }
                else
                {
                    newCubes.Add(cube);
                }
            }
            cubes = newCubes;
        }
        areas.AddRange(cubes);
        remove.ForEach(r => areas.Remove(r));
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r =>
        {
            var parts = r.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return (On: parts[0] == "on", Coords: parts[1].GetIntegers());
        }).ToList();

        List<int[]> areas = new();
        foreach (var cubeStates in data)
        {
            if (cubeStates.On)
                TurnOn(areas, cubeStates.Coords);
            else
                TurnOff(areas, cubeStates.Coords);
        }
        
        return areas.Aggregate(0UL, (acc, a) => acc + (1UL + (ulong)a[1] - (ulong)a[0]) * (1UL + (ulong)a[3] - (ulong)a[2]) * (1UL + (ulong)a[5] - (ulong)a[4])).ToString();
    }
}
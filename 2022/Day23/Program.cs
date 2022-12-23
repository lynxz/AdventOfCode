using Tools;

Day23 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day23 : DayBase
{

    public Day23() : base("23")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var elves = Enumerable.Range(0, data.Length).SelectMany(y => Enumerable.Range(0, data[0].Length).Select(x => (Y: y, X: x)))
            .Where(p => data[p.Y][p.X] == '#').ToList();

        var rules = new List<List<int[]>>
        {
            new List<int[]> {  new[] {-1, 0}, new[] {-1, -1}, new[] {-1, 1}, new[] {-1, 0} },
            new List<int[]> {  new[] {1, 0}, new[] {1, -1}, new[] {1, 1}, new[] {1, 0} },
            new List<int[]> {  new[] {0, -1}, new[] {-1, -1}, new[] {1, -1}, new[] {0, -1} },
            new List<int[]> {  new[] {0, 1}, new[] {-1, 1}, new[] {1, 1}, new[] {0, 1} },
        };
        var rule = 0;

        var adj = Enumerable.Range(0, 3).SelectMany(y => Enumerable.Range(0, 3).Select(x => (Y: y - 1, X: x - 1))).Where(p => !(p.Y == 0 && p.X == 0)).ToList();

        for (int i = 0; i < 10; i++)
        {
            var suggestedMoves = new List<((int Y, int X) Old, (int Y, int X) New)>();
            foreach (var elf in elves)
            {
                bool ruleAdded = false;
                var ch = new (int Y, int X)[3];
                if (elves.Any(e => adj.Any(p => e.Y == elf.Y + p.Y && e.X == elf.X + p.X)))
                {
                    for (int r = 0; r < 4; r++)
                    {
                        var curRule = (rule + r) % 4;
                        ch[0] = (Y: elf.Y + rules[curRule][0][0], X: elf.X + rules[curRule][0][1]);
                        ch[1] = (Y: elf.Y + rules[curRule][1][0], X: elf.X + rules[curRule][1][1]);
                        ch[2] = (Y: elf.Y + rules[curRule][2][0], X: elf.X + rules[curRule][2][1]);
                        if (!elves.Any(e => (e.Y == ch[0].Y && e.X == ch[0].X) ||
                                            (e.Y == ch[1].Y && e.X == ch[1].X) ||
                                            (e.Y == ch[2].Y && e.X == ch[2].X)))
                        {
                            var su = (elf.Y + rules[curRule][3][0], elf.X + rules[curRule][3][1]);
                            suggestedMoves.Add((elf, su));
                            ruleAdded = true;
                            break;
                        }
                    }
                    if (!ruleAdded)
                        suggestedMoves.Add((elf, elf));

                }
                else
                {
                    suggestedMoves.Add((elf, elf));
                }
            }
            var newPos = new List<(int Y, int X)>();
            foreach (var sm in suggestedMoves)
            {
                if (suggestedMoves.Count(m => m.New.Y == sm.New.Y && m.New.X == sm.New.X) < 2)
                {
                    newPos.Add(sm.New);
                }
                else
                {
                    newPos.Add(sm.Old);
                }
            }
            rule++;
            elves = newPos;
        }

        var minX = elves.Min(e => e.X);
        var maxX = elves.Max(e => e.X);
        var minY = elves.Min(e => e.Y);
        var maxY = elves.Max(e => e.Y);

        var absX = Math.Abs(maxX - minX)+1;
        var absY = Math.Abs(maxY - minY)+1;

        return ((absX * absY) - elves.Count).ToString();
    }

    void Print(List<(int Y, int X)> elves)
    {
        var minX = elves.Min(e => e.X);
        var maxX = elves.Max(e => e.X);
        var minY = elves.Min(e => e.Y);
        var maxY = elves.Max(e => e.Y);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Console.Write(elves.Any(e => e.Y == y && e.X == x) ? '#' : '.');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public override string SecondStar()
    {
         var data = GetRowData();
        var elves = Enumerable.Range(0, data.Length).SelectMany(y => Enumerable.Range(0, data[0].Length).Select(x => (Y: y, X: x)))
            .Where(p => data[p.Y][p.X] == '#').ToList();

        var rules = new List<List<int[]>>
        {
            new List<int[]> {  new[] {-1, 0}, new[] {-1, -1}, new[] {-1, 1}, new[] {-1, 0} },
            new List<int[]> {  new[] {1, 0}, new[] {1, -1}, new[] {1, 1}, new[] {1, 0} },
            new List<int[]> {  new[] {0, -1}, new[] {-1, -1}, new[] {1, -1}, new[] {0, -1} },
            new List<int[]> {  new[] {0, 1}, new[] {-1, 1}, new[] {1, 1}, new[] {0, 1} },
        };
        var rule = 0;

        var adj = Enumerable.Range(0, 3).SelectMany(y => Enumerable.Range(0, 3).Select(x => (Y: y - 1, X: x - 1))).Where(p => !(p.Y == 0 && p.X == 0)).ToList();

        var done = false;
        var round = 0;
        while (!done)
        {
            var suggestedMoves = new List<((int Y, int X) Old, (int Y, int X) New)>();
            foreach (var elf in elves)
            {
                bool ruleAdded = false;
                var ch = new (int Y, int X)[3];
                if (elves.Any(e => adj.Any(p => e.Y == elf.Y + p.Y && e.X == elf.X + p.X)))
                {
                    for (int r = 0; r < 4; r++)
                    {
                        var curRule = (rule + r) % 4;
                        ch[0] = (Y: elf.Y + rules[curRule][0][0], X: elf.X + rules[curRule][0][1]);
                        ch[1] = (Y: elf.Y + rules[curRule][1][0], X: elf.X + rules[curRule][1][1]);
                        ch[2] = (Y: elf.Y + rules[curRule][2][0], X: elf.X + rules[curRule][2][1]);
                        if (!elves.Any(e => (e.Y == ch[0].Y && e.X == ch[0].X) ||
                                            (e.Y == ch[1].Y && e.X == ch[1].X) ||
                                            (e.Y == ch[2].Y && e.X == ch[2].X)))
                        {
                            var su = (elf.Y + rules[curRule][3][0], elf.X + rules[curRule][3][1]);
                            suggestedMoves.Add((elf, su));
                            ruleAdded = true;
                            break;
                        }
                    }
                    if (!ruleAdded)
                        suggestedMoves.Add((elf, elf));

                }
                else
                {
                    suggestedMoves.Add((elf, elf));
                }
            }

            var newPos = new List<(int Y, int X)>();
            foreach (var sm in suggestedMoves)
            {
                if (suggestedMoves.Count(m => m.New.Y == sm.New.Y && m.New.X == sm.New.X) < 2)
                {
                    newPos.Add(sm.New);
                }
                else
                {
                    newPos.Add(sm.Old);
                }
            }
            rule++;
            elves = newPos;
            done = suggestedMoves.All(sm => sm.Old.X == sm.New.X && sm.Old.Y == sm.New.Y);
            round++;
        }

        return round.ToString();
    }
}
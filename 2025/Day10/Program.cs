using System.Text.RegularExpressions;
using Tools;
using Microsoft.Z3;

Day10 day = new();
day.OutputSecondStar();

public class Day10 : DayBase
{
    public Day10() : base("10")
    {
    }

    public override string FirstStar()
    {
        var rows = GetRowData();
        var sum = 0;

        var index = 0;
        foreach (var row in rows)
        {
            var lights = row.Take(row.IndexOf("]")).Skip(1).Select(c => c == '#').ToArray();
            var buttons = Regex.Matches(row, @"\((\d+,)*\d+\)").Select(m => m.Value.GetIntegers()).ToList();

            var presses = PressButtons(lights, new bool[lights.Length], buttons, 0, new Dictionary<string, int>());
            Console.WriteLine($"{index}: {presses}");
            index++;
            sum += presses;
        }

        return sum.ToString();
    }

    public override string SecondStar()
    {
        var rows = GetRowData();
        var total = 0;
        var index = 0;
        foreach (var row in rows)
        {
            var joltage = Regex.Match(row, @"\{(\d+,)*\d+\}").Value.GetIntegers();
            var buttons = Regex.Matches(row, @"\((\d+,)*\d+\)").Select(m => m.Value.GetIntegers()).ToList();

            using var ctx = new Context();
            var variables = buttons
                .Select(b => ctx.MkIntConst(string.Join(",", b)))
                .ToList();

            var opt = ctx.MkOptimize();
            for (int i = 0; i < joltage.Length; i++)
            {
                List<IntExpr> vars = [];
                for (int j = 0; j < buttons.Count; j++)
                {
                    if (buttons[j].Contains(i))
                    {
                        vars.Add(variables[j]);
                    }
                }

                opt.Add(ctx.MkEq(ctx.MkAdd(vars), ctx.MkInt(joltage[i])));
            }

            var sumExpr = ctx.MkAdd(variables);
            opt.MkMinimize(sumExpr);
            foreach (var v in variables)
                opt.Add(ctx.MkGe(v, ctx.MkInt(0)));

            var result = opt.Check();
            if (result != Status.SATISFIABLE)
                throw new Exception("No solution found");

            var model = opt.Model;
            total += variables.Sum(v => ((IntNum)model.Evaluate(v)).Int);
            
            // var presses = PressButtons2(joltage, buttons, 0, new Dictionary<string, int>());
            // Console.WriteLine($"{index}: {presses}");
            // index++;
            // sum += presses;
        }

        return total.ToString();
    }

    int PressButtons2(int[] state, List<int[]> buttons, int minStart, Dictionary<string, int> memo = null)
    {
        if (state.Any(s => s < 0))
        {
            return int.MaxValue;
        }
        if (buttons.Count == 0)
        {
            return int.MaxValue;
        }

        var index = Array.IndexOf(state, state.Max());
        var max = 0;
        var selectedButtons = buttons.Where(b => b.Contains(index)).ToList();
        if (selectedButtons.Count == 0)
            return int.MaxValue;

        var newButtons = buttons.Except(selectedButtons).ToList();
        var pushes = new int[selectedButtons.Count];

        for (int i = 0; i < selectedButtons.Count; i++)
        {
            do {
                pushes[i]++;
                var newState = (int[])state.Clone();
                foreach (var idx in selectedButtons[i])
                    newState[idx] -= pushes[i];

                if (newState.All(s => s == 0))
                {
                    Console.WriteLine("FOUND!");
                    return pushes[i];
                }

                var result = PressButtons2(newState, newButtons, minStart, memo);
                if (result != int.MaxValue && result + pushes[i] < minStart)
                {
                    minStart = result + pushes[i];
                }
            } while (pushes[i] >= 0);
        }

        int[]? button;
        do
        {
            button = newButtons.Where(b => b.Contains(index)).OrderByDescending(b => b.Length).FirstOrDefault();
            if (button == null) 
                return int.MaxValue;

            max = button.Select(i => state[i]).Min();
            newButtons = newButtons.Where(b => b != button).ToList();
        } while (max == 0);

        var min = int.MaxValue;
        for (int i = max; i >= 0; i--)
        {
            var newState = (int[])state.Clone();
            foreach (var idx in button)
                newState[idx] -= i;

            if (newState.All(s => s == 0))
            {
                Console.WriteLine("FOUND!");
                return i;
            }

            var result = PressButtons2(newState, newButtons, minStart, memo);
            if (result != int.MaxValue && result + i < min)
            {
                min = result + i;
            }
        }

        return min;
    }

    int PressButtons(int[] state, List<int[]> buttons, int presses, Dictionary<string, int> memo = null)
    {
        if (state.Any(s => s < 0))
        {
            return int.MaxValue;
        }

        var stateRep = string.Join(",", state);
        if (memo.TryGetValue(stateRep, out var cached))
        {
            if (cached <= presses)
            {
                return int.MaxValue;
            }
        }
        memo[stateRep] = presses;
        Console.WriteLine(memo.Count);

        var newStates = buttons.Select(b =>
        {
            var newState = (int[])state.Clone();
            foreach (var index in b)
            {
                newState[index]--;
            }
            return newState;
        }).ToList();

        if (newStates.Any(s => s.All(i => i == 0)))
        {
            Console.WriteLine("FOUND!");
            return presses + 1;
        }
        var min = newStates
        .OrderBy(s => s.Window(2).Sum(w => Math.Abs(w[0] - w[1])))
        .Select(s => PressButtons(s, buttons, presses + 1, memo))
        .Min();

        return min;
    }

    int PressButtons(bool[] lights, bool[] state, List<int[]> buttons, int presses, Dictionary<string, int> memo = null)
    {
        if (presses == 15) return int.MaxValue;

        var stateRep = new string(state.Select(s => s ? '#' : '.').ToArray());
        if (memo.TryGetValue(stateRep, out var cached))
        {
            return cached + presses + 1;
        }

        var min = int.MaxValue;
        var newStates = buttons.Select(b =>
        {
            var newState = (bool[])state.Clone();
            foreach (var index in b)
            {
                newState[index] = !newState[index];
            }
            return newState;
        }).ToList();

        if (newStates.Any(s => s.SequenceEqual(lights)))
        {
            return presses + 1;
        }
        min = newStates.Select(s => PressButtons(lights, s, buttons, presses + 1, memo)).Min();

        if (min != int.MaxValue)
            memo[stateRep] = min - presses - 1;

        return min;
    }
}
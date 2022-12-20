using Tools;

Day19 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day19 : DayBase
{

    public Day19() : base("19")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData();
        var blueprints = data.Select(l => l.GetIntegers().Skip(1).ToArray()).ToList();
        var result = Enumerable.Range(1, blueprints.Count).AsParallel()
            .Select(i => i * MaxProduceGeode(blueprints[i -1], 24)).Sum();

        return result.ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData();
        var blueprints = data.Select(l => l.GetIntegers().Skip(1).ToArray()).ToList();
        var result = Enumerable.Range(1, 3).AsParallel()
            .Select(i => MaxProduceGeode(blueprints[i -1], 32)).Aggregate((v,i) => v*i);
        return result.ToString();
    }

    delegate void ActionRef1<T1, T2>(ref T1 arg1, T2 arg2);

    int MaxProduceGeode(int[] blueprint, int duration)
    {
        HashSet<((int ore, int clay, int obsidian, int geodes) resources, (int oreBot, int clayBot, int obsidianBot, int geodeBot) botCounts)> allStates = new() {
                ((0, 0, 0, 0), (1, 0, 0, 0))
            };
        HashSet<((int ore, int clay, int obsidian, int geodes) resources, (int oreBot, int clayBot, int obsidianBot, int geodeBot) botCounts)> nextStates = new() { };
        List<ActionRef1<((int, int, int, int), (int, int, int, int)), int[]>> buildableBots = new();
        int bestGeodesSoFar = 0;
        int bestGeoBotsSoFar = 0;
        int bestBotsSoFar = 0;
        int maxOreCost = Math.Max(blueprint[0], Math.Max(blueprint[1], Math.Max(blueprint[2], blueprint[4])));

        for (int time = 1; time <= duration; time++)
        {
            foreach (var state in allStates.ToList())
            {
                buildableBots.Clear();

                if (state.resources.ore >= blueprint[4] && state.resources.obsidian >= blueprint[5])
                {
                    buildableBots.Add(BuildGeodeBot);
                }
                if (
                    state.resources.ore >= blueprint[2] &&
                    state.resources.clay >= blueprint[3] &&
                    state.botCounts.obsidianBot < blueprint[5]
                )
                {
                    buildableBots.Add(BuildObsidianBot);
                }
                if (state.resources.ore >= blueprint[1] && state.botCounts.oreBot < blueprint[3])
                {
                    buildableBots.Add(BuildClayBot);
                }
                if (state.resources.ore >= blueprint[0] && state.botCounts.oreBot < maxOreCost)
                {
                    buildableBots.Add(BuildOreBot);
                }

                int currStateBots = state.botCounts.clayBot + state.botCounts.obsidianBot + state.botCounts.geodeBot;
                if (bestBotsSoFar <= currStateBots || bestBotsSoFar - currStateBots < 10)
                {
                    nextStates.Add(((
                        state.resources.ore + state.botCounts.oreBot,
                        state.resources.clay + state.botCounts.clayBot,
                        state.resources.obsidian + state.botCounts.obsidianBot,
                        state.resources.geodes + state.botCounts.geodeBot
                    ), (
                        state.botCounts.oreBot,
                        state.botCounts.clayBot,
                        state.botCounts.obsidianBot,
                        state.botCounts.geodeBot
                    )));
                }

                bestGeodesSoFar = Math.Max(bestGeodesSoFar, state.resources.geodes);
                bestGeoBotsSoFar = Math.Max(bestGeoBotsSoFar, state.botCounts.geodeBot);
                bestBotsSoFar = Math.Max(bestBotsSoFar, currStateBots);

                foreach (var func in buildableBots)
                { 
                    ((int ore, int clay, int obsidian, int geodes) resources, (int, int, int, int) botCounts) newState = (
                        (state.resources.ore, state.resources.clay, state.resources.obsidian, state.resources.geodes),
                        (state.botCounts.oreBot, state.botCounts.clayBot, state.botCounts.obsidianBot, state.botCounts.geodeBot)
                    );

                    newState.resources.ore += state.botCounts.oreBot;
                    newState.resources.clay += state.botCounts.clayBot;
                    newState.resources.obsidian += state.botCounts.obsidianBot;
                    newState.resources.geodes += state.botCounts.geodeBot;

                    func(ref newState, blueprint);
                    int newStateBots = state.botCounts.clayBot + state.botCounts.obsidianBot + state.botCounts.geodeBot;
                    if (bestBotsSoFar <= newStateBots || bestBotsSoFar - newStateBots < 10)
                    {
                        nextStates.Add(newState);
                    }
                } 
            }
            allStates = nextStates;
            nextStates = new();
            allStates.RemoveWhere(state =>
                bestGeodesSoFar - state.resources.geodes >= 2 ||
                bestGeoBotsSoFar - state.botCounts.geodeBot >= 2
            );
        }

        var bestState = allStates.MaxBy(x => x.resources.geodes);
        return bestState.resources.geodes;
    }

    protected static void BuildOreBot(ref ((int ore, int, int, int) resources, (int oreBot, int, int, int) botCounts) state, int[] blueprint)
    {
        state.resources.ore -= blueprint[0];
        state.botCounts.oreBot++;
    }

    protected static void BuildClayBot(ref ((int ore, int, int, int) resources, (int, int clayBot, int, int) botCounts) state, int[] blueprint)
    {
        state.resources.ore -= blueprint[1];
        state.botCounts.clayBot++;
    }

    protected static void BuildObsidianBot(ref ((int ore, int clay, int, int) resources, (int, int, int obsidianBot, int) botCounts) state, int[] blueprint)
    {
        state.resources.ore -= blueprint[2];
        state.resources.clay -= blueprint[3];
        state.botCounts.obsidianBot++;
    }

    protected static void BuildGeodeBot(ref ((int ore, int, int obsidian, int) resources, (int, int, int, int geodeBot) botCounts) state, int[] blueprint)
    {
        state.resources.ore -= blueprint[4];
        state.resources.obsidian -= blueprint[5];
        state.botCounts.geodeBot++;
    }


    
}
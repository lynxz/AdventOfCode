using System.Text.RegularExpressions;
using Tools;

Day16 day = new();
day.OutputFirstStar();
day.OutputSecondStar();

public class Day16 : DayBase
{
    public Day16() : base("16")
    {
    }

    public override string FirstStar()
    {
        var regex = new Regex(@"tunnels? leads? to valves?\s(?<valves>.*)");
        var data = GetRowData();
        var valves = data.ToDictionary(l => l[6..8], l => l.GetIntegers()[0]);
        var tunnels = data.ToDictionary(l => l[6..8], l => regex.Match(l).Groups["valves"].Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToList());
        var paths = tunnels.Keys.ToDictionary(t => t, t => tunnels.Keys.Where(k => k != t).ToDictionary(e => e, e => Dijkstra(tunnels, t, e)));

        var result = Move(paths, valves, new List<string>(), "AA", 0, 30);

        return result.ToString();
    }

    public override string SecondStar()
    {
        var regex = new Regex(@"tunnels? leads? to valves?\s(?<valves>.*)");
        var data = GetRowData();
        var valves = data.ToDictionary(l => l[6..8], l => l.GetIntegers()[0]);
        var tunnels = data.ToDictionary(l => l[6..8], l => regex.Match(l).Groups["valves"].Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToList());
        var paths = tunnels.Keys.ToDictionary(t => t, t => tunnels.Keys.Where(k => k != t).ToDictionary(e => e, e => Dijkstra(tunnels, t, e)));
        var openValves = valves.Where(v => v.Value == 0).Select(v => v.Key).ToList();

        var result = Move2(paths, valves, openValves, "AA", "AA", 0, 26, 26, 0);

        return result.ToString();
    }

    int Move(Dictionary<string, Dictionary<string, List<string>>> paths, Dictionary<string, int> valves, List<string> openValves, string source, int score, int minutes)
    {
        var scores = paths[source].Keys
            .Where(k => !openValves.Contains(k) && k != source)
            .Select(t => (Valve: t, Score: (minutes - paths[source][t].Count - 1) * valves[t], Steps: paths[source][t].Count))
            .OrderByDescending(t => t.Score)
            .ToList();

        if (scores[0].Score <= 0)
            return score;

        return scores.Where(s => s.Score > 0).Max(s => Move(paths, valves, openValves.Append(s.Valve).ToList(), s.Valve, score + s.Score, minutes - s.Steps - 1));
    }

    int Move2(Dictionary<string, Dictionary<string, List<string>>> paths, Dictionary<string, int> valves, List<string> openValves, string source1, string source2, int score, int minutes1, int minutes2, int depth)
    {
        if (valves.Where(kvp => kvp.Value > 0).All(kvp => openValves.Contains(kvp.Key)))
            return score;

        var scores1 = paths[source1].Keys
            .Where(k => !openValves.Contains(k) && k != source1)
            .Select(t => (Valve: t, Score: (minutes1 - paths[source1][t].Count - 1) * valves[t], Steps: paths[source1][t].Count))
            .OrderByDescending(t => t.Score)
            .ToList();
        var scores2 = paths[source2].Keys
            .Where(k => !openValves.Contains(k) && k != source1)
            .Select(t => (Valve: t, Score: (minutes2 - paths[source2][t].Count - 1) * valves[t], Steps: paths[source2][t].Count))
            .OrderByDescending(t => t.Score)
            .ToList();

        if (scores1[0].Score <= 0 && scores2[0].Score <= 0)
            return score;

        var max = -1;
        if (!scores2.Any(s => s.Score > 0) && !scores1.Any(s => s.Score > 0))
        {
            return score;
        }
        else if (!scores1.Any(s => s.Score > 0))
        {
            foreach (var s in scores2.Where(s => s.Score > 0))
            {
                var result = Move2(paths, valves, openValves.Append(s.Valve).ToList(), source1, s.Valve, score + s.Score, minutes1, minutes2 - s.Steps - 1, depth + 1);
                if (result > max)
                {
                    max = result;
                }
            }
        }
        else if (!scores2.Any(s => s.Score > 0))
        {
            foreach (var s in scores1.Where(s => s.Score > 0))
            {
                var result = Move2(paths, valves, openValves.Append(s.Valve).ToList(), s.Valve, source2, score + s.Score, minutes1 - s.Steps - 1, minutes2, depth + 1);
                if (result > max)
                {
                    max = result;
                }
            }
        }
        else
        {
            foreach (var score1 in scores1.Where(s => s.Score > 0))
            {
                foreach (var score2 in scores2.Where(s => s.Score > 0))
                {
                    if (score1.Valve != score2.Valve)
                    {
                        if (score1.Score <= 0 && score2.Score <= 0)
                        {
                            continue;
                        }
                        var newOpen = openValves.ToList();
                        var newSource1 = score1.Score <= 0 ? source1 : score1.Valve;
                        var newMin1 = score1.Score <= 0 ? 0 : minutes1 - score1.Steps - 1;
                        var s1 = Math.Max(0, score1.Score);
                        if (score1.Score > 0)
                        {
                            newOpen.Add(score1.Valve);
                        }
                        var newSource2 = score2.Score <= 0 ? source2 : score2.Valve;
                        var newMin2 = score2.Score <= 0 ? 0 : minutes2 - score2.Steps - 1;
                        var s2 = Math.Max(0, score2.Score);
                        if (score2.Score > 0)
                        {
                            newOpen.Add(score2.Valve);
                        }
                        var result = Move2(paths, valves, newOpen, newSource1, newSource2, score + s1 + s2, newMin1, newMin2, depth + 1);
                        if (result > max)
                        {
                            max = result;
                        }
                    }
                }
            }
        }

        return max;
    }

    static List<string> Dijkstra(Dictionary<string, List<string>> graph, string source, string target)
    {
        var nodes = graph.Keys.ToList();

        var dist = nodes.ToDictionary(k => k, _ => 10000);
        var prev = new Dictionary<string, string>();
        dist[source] = 0;

        while (nodes.Count > 0)
        {
            var u = nodes.OrderBy(n => dist[n]).First();
            nodes.Remove(u);
            if (u == target)
            {
                var result = new List<string>();
                while (prev.ContainsKey(u))
                {
                    result.Insert(0, u);
                    u = prev[u];
                }

                return result;
            }

            foreach (var v in graph[u])
            {
                var alt = dist[u] + 1;
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        throw new InvalidOperationException("Never reached target");
    }
}
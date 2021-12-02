// See https://aka.ms/new-console-template for more information
using Tools;

var day = new Day2("2");
day.OutputSecondStar();

public class Day2 : DayBase
{
    public Day2(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var input = GetRowData().Select(d => d.Split(' ')).Select(p => (Command: p[0], Step: int.Parse(p[1]))).ToList();
        var res = input.GroupBy(d => d.Command).ToDictionary(g => g.Key, g => g.Sum(x => x.Step));
        return ((res["down"] - res["up"]) * res["forward"]).ToString();
    }

    public override string SecondStar()
    {
        var input = GetRowData().Select(d => d.Split(' ')).Select(p => (Command: p[0], Step: int.Parse(p[1]))).ToList();
        (int depth, int pos, int aim) = input.Aggregate((Depth: 0, Pos: 0, Aim: 0), (p, m) =>
            m.Command switch 
            {
                "forward" => (p.Depth + (p.Aim * m.Step), p.Pos + m.Step, p.Aim),
                "down" => (p.Depth, p.Pos, p.Aim + m.Step),
                "up" => (p.Depth, p.Pos, p.Aim - m.Step)
            });

        return (depth * pos).ToString();
    }
}
// See https://aka.ms/new-console-template for more information
using Tools;

Day18 day = new("18");
day.OutputSecondStar();

public class Day18 : DayBase
{
    public Day18(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(r => SnailNumber.Parse(r)).ToList();
        data.Reverse();
        var stack = new Stack<SnailNumber>(data);

        while (stack.Count > 1)
        {
            var first = stack.Pop();
            var second = stack.Pop();
            var sum = first + second;
            sum.Reduce();
            stack.Push(sum);
        }

        return stack.Pop().Magnitude().ToString();
    }

    public override string SecondStar()
    {
        var data = GetRowData().Select(r => SnailNumber.Parse(r)).ToList();
        var max = 0;
        for (int i = 0; i < data.Count-1; i++)
        {
            for (int j = i; j < data.Count; j++)
            {
                var sum = data[i].Copy() + data[j].Copy();
                sum.Reduce();
                var magnitude = sum.Magnitude();
                if (magnitude > max) 
                    max = magnitude;
                sum = data[j].Copy() + data[i].Copy();
                sum.Reduce();
                magnitude = sum.Magnitude();
                if (magnitude > max) 
                    max = magnitude;   
            }
        }
        return max.ToString();
    }


}

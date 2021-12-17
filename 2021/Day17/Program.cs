// See https://aka.ms/new-console-template for more information
using Tools;

Day17 day = new("17");
day.OutputSecondStar();

public class Day17 : DayBase
{
    public Day17(string day) : base(day)
    {
    }

    public override string FirstStar()
    {
        return GeoSum(139).ToString();
    }

    public override string SecondStar()
    {
        var xMax = 164;
        var xMin = 117;
        var yMin = -140;
        var yMax = -89;
        var xPos = Enumerable.Range(1, xMax).Where(i => Enumerable.Range(1, i).Any(j => GeoSum(i) - GeoSum(j) >= xMin && GeoSum(i) - GeoSum(j) <= xMax)).ToList();
        var pairs = xPos.SelectMany(x => Enumerable.Range(yMin, 2 * (-yMin)).Where(y =>
        {
            var vx = x;
            var vy = y;
            var px = 0;
            var py = 0;
            while (px <= xMax && py >= yMin)
            {
                px += vx;
                py += vy;
                vx = Math.Max(0, vx - 1);
                vy -= 1;
                if (px >= xMin && px <= xMax && py >= yMin && py <= yMax)
                    return true;
            }

            return false;
        }).Select(y => (Y: y, X: x))).ToList();

        return pairs.Count.ToString();
    }

    int GeoSum(int i) => i * (i + 1) / 2;


}
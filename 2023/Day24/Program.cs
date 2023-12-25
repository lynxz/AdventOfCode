using Microsoft.Z3;
using Tools;

Day24 day = new();
day.OutputSecondStar();

public class Day24 : DayBase
{
    public Day24() : base("24")
    {
    }

    public override string FirstStar()
    {
        var data = GetRowData().Select(d => d.GetLongs().Select(l => Convert.ToDouble(l)).ToArray()).ToArray();
        var max = 400000000000000.0;
        var min = 200000000000000.0;

        // var sum = 0;

        var combinations = data.DifferentCombinations(2).ToList();
        
        var sum = combinations.Count(c => Intersect(c.First(), c.Last(), min, max));
      
      
        // Figure out why didn't this work
        // var sum = 0;
        // var step = 0;
        // foreach(var comb in combinations) {
        //     step++;
        //     var l1 = comb.First();
        //     var l2 = comb.Last();
        //     var t1 = Math.Min(l1[3] < 0 ? (min-l1[0])/l1[3] : (max-l1[0])/l1[3], l1[4] < 0 ? (min-l1[1])/l1[4] : (max-l1[1])/l1[4]);
        //     var x2 = l1[0] + l1[3] * t1;
        //     var y2 = l1[1] + l1[4] * t1;
        //     var t2 = Math.Min(l2[3] < 0 ? (min-l2[0])/l2[3] : (max-l2[0])/l2[3], l2[4] < 0 ? (min-l2[1])/l2[4] : (max-l2[1])/l2[4]);
        //     var x4 = l2[0] + l2[3] * t2;
        //     var y4 = l2[1] + l2[4] * t2;

        //     var d = (l1[0]-x2)*(l2[1]-y4)-(l1[1]-y2)*(l2[0]-x4);
        //     var t = (l1[0] - l2[0])*(l2[1]-y4)-(l1[1]-l2[1])*(l2[0]-x4);
        //     var u = (l1[0]-l2[0])*(l1[1]-y2)-(l1[1]-l2[1])*(l1[0]-x2);

            

        //     if (t >= 0 && t <= d && u >= 0 && u <= d) {
                
        //         Debug.Assert(Math.Abs(l2[0]+u/d*(x4-l2[0])-(l1[0]+t/d*(x2-l1[0]))) < 10.0);
        //         Debug.Assert(Math.Abs(l2[1]+u/d*(y4-l2[1])-(l1[1]+t/d*(y2-l1[1]))) < 10.0);
        //         // Debug.Assert(l2[0]+u/d*(x4-l2[0]) >= min && l2[0]+u/d*(x4-l2[0]) <= max);
        //         // Debug.Assert(l2[1]+u/d*(y4-l2[1]) >= min && l2[1]+u/d*(y4-l2[1]) <= max);
        //         // System.Console.WriteLine($"t: {t/(double)d}, u: {u/(double)d}");
        //         // System.Console.WriteLine($"l1: x = {l1[0]+t/(double)d*(x2-l1[0])}, y = {l1[1]+t/(double)d*(y2-l1[1])}");
        //         // System.Console.WriteLine($"l2: x = {l2[0]+u/(double)d*(x4-l2[0])}, y = {l2[1]+u/(double)d*(y4-l2[1])}");
        //         // System.Console.WriteLine();
        //         if (l2[1]+u/d*(y4-l2[1]) >= min && l2[1]+u/d*(y4-l2[1]) <= max && l2[0]+u/d*(x4-l2[0]) >= min && l2[0]+u/d*(x4-l2[0]) <= max) {
        //             Debug.Assert(Intersect(l1, l2, min, max));
        //             sum++;
        //         }
                    
        //     }
        // }

        return sum.ToString(); 
    }

    static bool Intersect(double[] h1, double[] h2, double min, double max) {
        var a1 = h1[4]/h1[3];
        var b1 = h1[1] - a1 * h1[0];
        var a2 = h2[4]/h2[3];
        var b2 = h2[1] - a2 * h2[0];

        if (Math.Abs(a1-a2) < 0.0001)
            return Math.Abs(b1-b2) < 0.0001;

        var cx = (b2-b1)/(a1-a2);
	    var cy = cx*a1 + b1;
	    var in_future = (cx > h1[0]) == (h1[3] > 0) && (cx > h2[0]) == (h2[3] > 0);

        return in_future && cx >= min && cx <= max && cy >= min && cy <= max;
    }

    public override string SecondStar()
    {
        // Stole this solution.
        var data = GetRowData().Select(d => d.GetLongs().Select(l => Convert.ToDouble(l)).ToArray()).ToList();
        return Solve(data).ToString();
    }

    static long Solve(List<double[]> hails)
{
    const int X = 0;
    const int Y = 1;
    const int Z = 2; 
    const int VX = 3;
    const int VY = 4;
    const int VZ = 5;

    var ctx = new Context();
    var solver = ctx.MkSolver();
 
    // Coordinates of the stone
    var x = ctx.MkIntConst("x");
    var y = ctx.MkIntConst("y");
    var z = ctx.MkIntConst("z");
 
    // Velocity of the stone
    var vx = ctx.MkIntConst("vx");
    var vy = ctx.MkIntConst("vy");
    var vz = ctx.MkIntConst("vz");
 
    for (var i = 0; i < 3; i++)
    {
        var t = ctx.MkIntConst($"t{i}"); // time for the stone to reach the hail
        var hail = hails[i];
 
        var px = ctx.MkInt(Convert.ToInt64(hail[X]));
        var py = ctx.MkInt(Convert.ToInt64(hail[Y]));
        var pz = ctx.MkInt(Convert.ToInt64(hail[Z]));
        
        var pvx = ctx.MkInt(Convert.ToInt64(hail[VX]));
        var pvy = ctx.MkInt(Convert.ToInt64(hail[VY]));
        var pvz = ctx.MkInt(Convert.ToInt64(hail[VZ]));
        
        var xLeft = ctx.MkAdd(x, ctx.MkMul(t, vx)); // x + t * vx
        var yLeft = ctx.MkAdd(y, ctx.MkMul(t, vy)); // y + t * vy
        var zLeft = ctx.MkAdd(z, ctx.MkMul(t, vz)); // z + t * vz
 
        var xRight = ctx.MkAdd(px, ctx.MkMul(t, pvx)); // px + t * pvx
        var yRight = ctx.MkAdd(py, ctx.MkMul(t, pvy)); // py + t * pvy
        var zRight = ctx.MkAdd(pz, ctx.MkMul(t, pvz)); // pz + t * pvz
 
        solver.Add(t >= 0); // time should always be positive - we don't want solutions for negative time
        solver.Add(ctx.MkEq(xLeft, xRight)); // x + t * vx = px + t * pvx
        solver.Add(ctx.MkEq(yLeft, yRight)); // y + t * vy = py + t * pvy
        solver.Add(ctx.MkEq(zLeft, zRight)); // z + t * vz = pz + t * pvz
    }
 
    solver.Check();
    var model = solver.Model;
 
    var rx = model.Eval(x);
    var ry = model.Eval(y);
    var rz = model.Eval(z);
 
    return Convert.ToInt64(rx.ToString()) + Convert.ToInt64(ry.ToString()) + Convert.ToInt64(rz.ToString());
}
}
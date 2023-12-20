namespace Tools;

public static class Algo
{
    public static long LCM(long[] numbers)
    {
        return numbers.Aggregate(lcm);
    }
    public static long lcm(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }
    public static long GCD(long a, long b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}
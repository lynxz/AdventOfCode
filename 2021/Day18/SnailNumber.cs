// See https://aka.ms/new-console-template for more information
public abstract class SnailNumber
{

    protected bool _changed = true;

    public static SnailNumber Parse(string number)
    {
        var index = 0;
        return Parse(number, ref index);
    }

    static SnailNumber Parse(string number, ref int i)
    {
        i++;
        var firstNumber = char.IsNumber(number[i]) ?
            new LiteralSnailNumber(int.Parse(number[i++].ToString())) :
            Parse(number, ref i);
        i++;
        var secondNumber = char.IsNumber(number[i]) ?
            new LiteralSnailNumber(int.Parse(number[i++].ToString())) :
            Parse(number, ref i);
        i++;
        return new PairSnailNumber(firstNumber, secondNumber);
    }

    public void Reduce()
    {

        do
        {
            do
            {
                Explode(0);
            } while (Changed());

            Split();
        } while (Changed());
    }

    public abstract SnailNumber Explode(int level);

    public abstract SnailNumber Split();

    public static SnailNumber operator +(SnailNumber first, SnailNumber second) => new PairSnailNumber(first, second);

    public abstract bool Changed();

    public abstract SnailNumber Copy();

    public abstract int Magnitude();
}

// See https://aka.ms/new-console-template for more information
public class LiteralSnailNumber : SnailNumber
{

    private readonly int _value;

    public LiteralSnailNumber(int value)
    {
        _value = value;
    }

    public int Value => _value;

    public static SnailNumber operator +(LiteralSnailNumber first, LiteralSnailNumber second) => new LiteralSnailNumber(first.Value + second.Value);

    public override int Magnitude() => _value;

    public override SnailNumber Explode(int level) => new NanSnailNumber();

    public override SnailNumber Split()
    {
        if (Value < 10)
            return this;

        var remainder = _value % 2;
        return new PairSnailNumber(new LiteralSnailNumber(_value / 2), new LiteralSnailNumber(remainder + _value / 2), true);
    }

    public override bool Changed() => false;

    public override string ToString() => Value.ToString();

    public override SnailNumber Copy() => new LiteralSnailNumber(Value);
}

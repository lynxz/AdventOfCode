// See https://aka.ms/new-console-template for more information
public class PairSnailNumber : SnailNumber
{

    public PairSnailNumber(SnailNumber first, SnailNumber second, bool changed = false)
    {
        _changed = changed;
        First = first;
        Second = second;
    }

    public SnailNumber First { get; set; }

    public SnailNumber Second { get; set; }

    public override SnailNumber Explode(int level)
    {
        _changed = false;

        if (level > 3)
        {
            return this;
        }

        var result = First.Explode(level + 1);
        if (result is PairSnailNumber pair)
        {
            _changed = true;
            if (level == 3)
                First = new LiteralSnailNumber(0);
            if (pair.Second is LiteralSnailNumber literal)
            {
                if (Second is LiteralSnailNumber secLiteral)
                {
                    Second = literal + secLiteral;
                }
                else if (Second is PairSnailNumber secPair)
                {
                    while (secPair.First is PairSnailNumber) {
                        secPair = secPair.First as PairSnailNumber;
                    }
                    secPair.First = literal + (secPair.First as LiteralSnailNumber);
                }
            }
            return new PairSnailNumber(pair.First, new NanSnailNumber());
        }

        result = Second.Explode(level + 1);
        if (result is PairSnailNumber secondPair)
        {
            _changed = true;
            if (level == 3)
                Second = new LiteralSnailNumber(0);
            if (secondPair.First is LiteralSnailNumber literal)
            {
                if (First is LiteralSnailNumber secLiteral)
                {
                    First = literal + secLiteral;
                }
                else if (First is PairSnailNumber secPair)
                {
                    while (secPair.Second is PairSnailNumber) {
                        secPair = secPair.Second as PairSnailNumber;
                    }
                    secPair.Second = literal + (secPair.Second as LiteralSnailNumber);
                }
            }
            return new PairSnailNumber(new NanSnailNumber(), secondPair.Second);
        }

        return new NanSnailNumber();
    }

    public override SnailNumber Split()
    {
        First = First.Split();
        if (Changed())
            return this;
        Second = Second.Split();
        return this;
    }

    public override bool Changed() => First.Changed() || Second.Changed() || _changed;

    public override int Magnitude() => 3 * First.Magnitude() + 2 * Second.Magnitude();

    public override string ToString() => $"[{First.ToString()}, {Second.ToString()}]";

    public override SnailNumber Copy() => new PairSnailNumber(First.Copy(), Second.Copy());
    
}

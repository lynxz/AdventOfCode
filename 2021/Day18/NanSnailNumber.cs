// See https://aka.ms/new-console-template for more information
public class NanSnailNumber : SnailNumber
{
    public override SnailNumber Explode(int level) => throw new NotImplementedException();

    public override SnailNumber Split() => throw new NotImplementedException();

    public override bool Changed() => false;

    public override int Magnitude() => 0;

    public override string ToString() => "NaN";

    public override SnailNumber Copy() => throw new NotImplementedException();
    
}

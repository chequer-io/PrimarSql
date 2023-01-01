namespace PrimarSql.Engine.Functions;

public class CountFunction : AggregateFunction
{
    private int _count;

    // TODO: Structured classes
    public override void Process(object?[] data)
    {
        _count++;
    }

    public override object Get()
    {
        return _count;
    }
}

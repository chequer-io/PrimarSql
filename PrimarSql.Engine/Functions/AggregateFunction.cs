using System.Dynamic;

namespace PrimarSql.Engine.Functions;

public abstract class AggregateFunction
{
    // TODO: Structured classes
    public abstract void Process(object?[] data);

    public abstract object Get();
}

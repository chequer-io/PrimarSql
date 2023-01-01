using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

// FUNCTION (*) filter? over?
public class AggregateFunctionCall : Expression
{
    // TODO: filter, over

    public QualifiedName Name { get; }

    public AggregateFunctionCall(QualifiedName name)
    {
        Name = name;
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield break;
    }
}

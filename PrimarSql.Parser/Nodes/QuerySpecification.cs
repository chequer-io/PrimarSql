using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class QuerySpecification : QueryBody
{
    public NodeValue<Select> Select { get; }

    public NullableValue<Relation> Relation { get; } = new();

    public NullableValue<Expression> Where { get; } = new();

    public QuerySpecification(Select select)
    {
        Select = new NodeValue<Select>(select);
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield break;
    }
}

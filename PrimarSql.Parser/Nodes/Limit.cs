using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class Limit : Node
{
    public NodeValue<Expression> RowCount { get; }

    public Limit(Expression rowCount)
    {
        RowCount = new NodeValue<Expression>(rowCount);
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield return RowCount.Value;
    }
}

using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class Offset : Node
{
    public NodeValue<Expression> RowCount { get; }

    public Offset(Expression rowCount)
    {
        RowCount = new NodeValue<Expression>(rowCount);
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield return RowCount.Value;
    }
}

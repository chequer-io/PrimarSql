using System.Collections.Generic;
using PrimarSql.Parser.Utilities;

namespace PrimarSql.Parser.Nodes;

public class SingleColumn : SelectItem
{
    public NullableValue<Identifier> Alias { get; } = new();

    public NodeValue<Expression> Expression { get; }

    public SingleColumn(Expression expression)
    {
        Expression = new NodeValue<Expression>(expression);
    }

    public override IEnumerable<Node> GetChildren()
        => NodeUtility.YieldNodes(Alias, Expression);
}

using System.Collections.Generic;
using PrimarSql.Parser.Utilities;

namespace PrimarSql.Parser.Nodes;

public class UnaryExpression : Expression
{
    public UnaryOperator Operator { get; set; }

    public NodeValue<Expression> Expression { get; }

    public UnaryExpression(Expression expression, UnaryOperator op)
    {
        Expression = new NodeValue<Expression>(expression);
        Operator = op;
    }

    public override IEnumerable<Node> GetChildren()
        => NodeUtility.YieldNodes(Expression);

    public enum UnaryOperator
    {
        Not
    }
}

using System.Collections.Generic;
using PrimarSql.Parser.Utilities;

namespace PrimarSql.Parser.Nodes;

public class Comparison : Expression
{
    public NodeValue<Expression> Left { get; }

    public NodeValue<Expression> Right { get; }

    public ComparisonOperator Operator { get; set; }

    public Comparison(Expression left, Expression right, ComparisonOperator op)
    {
        Left = new NodeValue<Expression>(left);
        Right = new NodeValue<Expression>(right);

        Operator = op;
    }

    public override IEnumerable<Node> GetChildren()
        => NodeUtility.YieldNodes(Left, Right);

    public enum ComparisonOperator
    {
        Equal,
        NotEqual,
        LessThan,
        LessThanEqual,
        GreaterThan,
        GreaterThanEqual
    }
}

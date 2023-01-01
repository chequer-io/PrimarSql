using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class LogicalBinary : Expression
{
    public NodeValue<Expression> Left { get; }

    public NodeValue<Expression> Right { get; }

    public BinaryOperator Operator { get; }

    public LogicalBinary(Expression left, Expression right, BinaryOperator op)
    {
        Left = new NodeValue<Expression>(left);
        Right = new NodeValue<Expression>(right);
        Operator = op;
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield return Left.Value;
        yield return Right.Value;
    }

    public enum BinaryOperator
    {
        And,
        Or
    }
}

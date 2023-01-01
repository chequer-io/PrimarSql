using System.Collections.Generic;
using PrimarSql.Parser.Utilities;

namespace PrimarSql.Parser.Nodes;

public class ArithmeticBinary : Expression
{
    public NodeValue<Expression> Left { get; }

    public NodeValue<Expression> Right { get; }

    public ArithmeticOperator Operator { get; set; }

    public ArithmeticBinary(Expression left, Expression right)
    {
        Left = new NodeValue<Expression>(left);
        Right = new NodeValue<Expression>(right);
    }

    public override IEnumerable<Node> GetChildren()
        => NodeUtility.YieldNodes(Left, Right);

    public enum ArithmeticOperator
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Mod
    }
}

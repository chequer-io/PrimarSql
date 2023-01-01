using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class TargetInList : Expression
{
    public NodeValue<Expression> Target { get; }

    public Expression[] Expressions { get; }

    public bool IsNot { get; }

    public TargetInList(Expression expression, Expression[] expressions, bool isNot)
    {
        Target = new NodeValue<Expression>(expression);
        Expressions = expressions;
        IsNot = isNot;
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield return Target.Value;

        foreach (var expression in Expressions)
            yield return expression;
    }
}

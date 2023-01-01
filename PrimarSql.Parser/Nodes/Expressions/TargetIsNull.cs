using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class TargetIsNull : Expression
{
    public NodeValue<Expression> Target { get; }

    public bool IsNot { get; } // NOT NULL

    public TargetIsNull(Expression target, bool isNot)
    {
        Target = new NodeValue<Expression>(target);
        IsNot = isNot;
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield return Target.Value;
    }
}

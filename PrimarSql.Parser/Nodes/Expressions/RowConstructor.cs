using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class RowConstructor : Expression
{
    public Expression[] Expressions { get; }

    public RowConstructor(Expression[] expressions)
    {
        Expressions = expressions;
    }

    public override IEnumerable<Node> GetChildren()
    {
        return Expressions;
    }
}

using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class InlineTable : QueryBody
{
    public Expression[] Expressions { get; }

    public InlineTable(Expression[] expressions)
    {
        Expressions = expressions;
    }

    public override IEnumerable<Node> GetChildren()
    {
        return Expressions;
    }
}

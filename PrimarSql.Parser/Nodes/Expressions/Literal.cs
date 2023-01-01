using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class Literal : Expression
{
    public LiteralType LiteralType { get; }

    public object? Value { get; }

    public Literal(LiteralType literalType, object? value)
    {
        LiteralType = literalType;
        Value = value;
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield break;
    }
}

using PrimarSql.Common.Data;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Columns;

public class LiteralColumn : DataColumn
{
    public override string Name { get; }

    private readonly Literal _literal;

    public LiteralColumn(Literal literal)
    {
        _literal = literal;
        Name = literal.Value?.ToString() ?? "(anonymous)";
    }

    public override object? Get(int index)
    {
        return _literal.Value;
    }

    public override DataColumn Skip(int count)
    {
        return this;
    }

    public override DataColumn Take(int count)
    {
        return this;
    }
}

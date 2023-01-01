using System.Collections.Generic;
using System.Linq;

namespace PrimarSql.Parser.Nodes;

public class AliasedRelation : Relation
{
    public NodeValue<Relation> Relation { get; }

    public NodeValue<Identifier> Alias { get; }

    public Identifier[]? ColumnAliases { get; }

    public AliasedRelation(Relation relation, Identifier alias, Identifier[]? columnAliases)
    {
        Relation = new NodeValue<Relation>(relation);
        Alias = new NodeValue<Identifier>(alias);
        ColumnAliases = columnAliases;
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield return Relation.Value;
        yield return Alias.Value;

        foreach (var columnAlias in ColumnAliases ?? Enumerable.Empty<Identifier>())
            yield return columnAlias;
    }
}

using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class InsertInto : Statement
{
    public QualifiedName Target { get; }

    public NodeValue<Query> Query { get; }

    public Identifier[]? ColumnAliases { get; set; }

    public InsertInto(QualifiedName target, Query query)
    {
        Target = target;
        Query = new NodeValue<Query>(query);
    }

    public override IEnumerable<Node> GetChildren()
    {
        if (ColumnAliases is { } columnAliases)
        {
            foreach (var columnAlias in columnAliases)
                yield return columnAlias;
        }

        yield return Query.Value;
    }
}

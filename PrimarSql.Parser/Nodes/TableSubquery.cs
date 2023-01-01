using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class TableSubquery : QueryBody
{
    public NodeValue<Query> Query { get; }

    public TableSubquery(Query query)
    {
        Query = new NodeValue<Query>(query);
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield return Query.Value;
    }
}

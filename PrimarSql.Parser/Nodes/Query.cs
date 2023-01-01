using System.Collections.Generic;
using PrimarSql.Parser.Utilities;

namespace PrimarSql.Parser.Nodes;

public class Query : Statement
{
    public NullableValue<Node> With { get; } = new();

    public NodeValue<QueryBody> QueryBody { get; }

    public NullableValue<Node> OrderBy { get; } = new();

    public NullableValue<Offset> Offset { get; } = new();

    public NullableValue<Limit> Limit { get; } = new();

    public Query(QueryBody queryBody)
    {
        QueryBody = new NodeValue<QueryBody>(queryBody);
    }

    public override IEnumerable<Node> GetChildren()
        => NodeUtility.YieldNodes(With, QueryBody, OrderBy, Offset, Limit);
}

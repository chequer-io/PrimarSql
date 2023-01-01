using PrimarSql.Common;
using PrimarSql.Common.Metadata;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Analyzers;

internal class Analysis
{
    public Dictionary<Table, TableInfo> Tables { get; } = new();

    public Dictionary<Node, Scope> Scopes { get; } = new();

    public Dictionary<Limit, int> Limits { get; } = new();

    public Dictionary<Offset, int> Offsets { get; } = new();

    public Dictionary<Table, ObjectName> TableNames { get; } = new();

    public Dictionary<Relation, QualifiedName> RelationNames { get; } = new();

    public Dictionary<Field, Expression> Expressions { get; } = new();

    public Dictionary<Node, bool> IsAggregateRelation { get; } = new();

    public Dictionary<AllColumns, Field[]> AllColumnsFields { get; } = new();

    public Dictionary<SingleColumn, Field> SingleColumnFields { get; } = new();

    public Dictionary<InsertInto, ObjectName> InsertTargets { get; } = new();

    public Scope GetScope(Node relation)
    {
        return Scopes.TryGetValue(relation, out var value) ? value : throw new PrimarSqlException("Scope is null.");
    }

    public Expression? GetExpression(Field field)
    {
        return Expressions.TryGetValue(field, out var expression) ? expression : null;
    }
}

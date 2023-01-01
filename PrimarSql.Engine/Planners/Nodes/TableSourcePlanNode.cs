using PrimarSql.Common.Data;
using PrimarSql.Common.Metadata;
using PrimarSql.Engine.Analyzers;

namespace PrimarSql.Engine.Planners.Nodes;

internal class TableSourcePlanNode : PlanNode
{
    private readonly TableInfo _table;

    public TableSourcePlanNode(TableInfo table, Scope scope) : base(scope)
    {
        _table = table;
    }

    public override string Name => "Table Scan";

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield break;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        return session.SourceProvider.GetTableDataPage(_table);
    }
}

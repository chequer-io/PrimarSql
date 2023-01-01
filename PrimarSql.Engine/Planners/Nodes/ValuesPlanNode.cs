using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.PageProviders;

namespace PrimarSql.Engine.Planners.Nodes;

internal class ValuesPlanNode : PlanNode
{
    public override string Name => "VALUES";

    private readonly object?[][] _data;

    public ValuesPlanNode(Scope scope, object?[][] data) : base(scope)
    {
        _data = data;
    }

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield break;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        return new LiteralProvider(_data);
    }
}

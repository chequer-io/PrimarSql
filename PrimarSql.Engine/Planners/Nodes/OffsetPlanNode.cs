using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.PageProviders;

namespace PrimarSql.Engine.Planners.Nodes;

internal class OffsetPlanNode : PlanNode
{
    public override string Name => "Offset";

    private readonly PlanNode _source;
    private readonly int _offsetCount;

    public OffsetPlanNode(Scope scope, PlanNode source, int offsetCount)
        : base(scope)
    {
        _source = source;
        _offsetCount = offsetCount;
    }

    public OffsetPlanNode(PlanId planId, Scope scope, PlanNode source, int offsetCount)
        : base(planId, scope)
    {
        _source = source;
        _offsetCount = offsetCount;
    }

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield return _source;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        return new OffsetProvider(_source.Execute(session), _offsetCount);
    }
}

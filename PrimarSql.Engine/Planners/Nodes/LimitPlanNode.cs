using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.PageProviders;

namespace PrimarSql.Engine.Planners.Nodes;

internal class LimitPlanNode : PlanNode
{
    public override string Name => "Limit";

    private readonly PlanNode _source;
    private readonly int _limitCount;

    public LimitPlanNode(Scope scope, PlanNode source, int limitCount) : base(scope)
    {
        _source = source;
        _limitCount = limitCount;
    }

    public LimitPlanNode(PlanId planId, Scope scope, PlanNode source, int limitCount)
        : base(planId, scope)
    {
        _source = source;
        _limitCount = limitCount;
    }

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield return _source;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        return new LimitProvider(_source.Execute(session), _limitCount);
    }
}

using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.Functions;
using PrimarSql.Engine.PageProviders;

namespace PrimarSql.Engine.Planners.Nodes;

internal class AggregatePlanNode : PlanNode
{
    private readonly string _name;
    private readonly PlanNode _source;
    private readonly AggregateFunction[] _functions;

    public override string Name => "Aggergate";

    public AggregatePlanNode(Scope scope, string name, PlanNode source, AggregateFunction[] functions) : base(scope)
    {
        _name = name;
        _source = source;
        _functions = functions;
    }

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield return _source;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        return new AggregateProvider(_source.Execute(session), _functions, _name);
    }
}

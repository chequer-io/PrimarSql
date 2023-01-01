using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.PageProviders;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Planners.Nodes;

internal class FilterPlanNode : PlanNode
{
    public override string Name => "Filter";

    private readonly FieldMapper _mapper;
    private readonly PlanNode _source;
    private readonly Expression _expression;

    public FilterPlanNode(Scope scope, FieldMapper mapper, PlanNode source, Expression expression) : base(scope)
    {
        _mapper = mapper;
        _source = source;
        _expression = expression;
    }

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield return _source;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        return new FilterProvider(_source.Execute(session), _mapper, Scope, _expression);
    }
}

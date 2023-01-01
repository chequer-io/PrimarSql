using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.PageProviders;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Planners.Nodes;

internal class ProjectionPlanNode : PlanNode
{
    public override string Name => "Projection";

    private readonly FieldMapper _mapper;
    private readonly PlanNode _source;
    private readonly string _sourceName;
    private readonly Expression?[] _expressions;

    public ProjectionPlanNode(Scope scope, FieldMapper mapper, PlanNode source, string sourceName, Expression?[] expressions) : base(scope)
    {
        _mapper = mapper;
        _source = source;
        _sourceName = sourceName;
        _expressions = expressions;
    }

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield return _source;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        return new ProjectionProvider(_source.Execute(session), _mapper, _sourceName, Scope, _expressions);
    }
}

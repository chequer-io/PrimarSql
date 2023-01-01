using PrimarSql.Common;
using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.PageProviders;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Planners.Nodes;

internal class InsertPlanNode : PlanNode
{
    private readonly ObjectName _objectName;
    private readonly Identifier[]? _aliases;
    private readonly PlanNode _planNode;

    public override string Name => "Insert";

    public InsertPlanNode(Scope scope, ObjectName objectName, Identifier[]? aliases, PlanNode planNode) : base(scope)
    {
        _objectName = objectName;
        _aliases = aliases;
        _planNode = planNode;
    }

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield break;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        var reader = new DataPageReader(_planNode.Execute(session), _aliases?.Select(alias => alias.Unescape()).ToArray());

        return new EmptyProvider(
            session.SourceProvider.InsertToTable(
                _objectName,
                reader)
        );
    }
}

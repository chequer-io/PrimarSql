using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;

namespace PrimarSql.Engine.Planners.Nodes;

internal class EmptyRelationPlanNode : PlanNode
{
    public EmptyRelationPlanNode(Scope scope) : base(scope)
    {
    }

    public override string Name => "Empty Relation";

    public override IEnumerable<PlanNode> GetChildren()
    {
        yield break;
    }

    protected override DataPageProvider CreateProvider(Session session)
    {
        return new EmptyPageProvider();
    }

    private class EmptyPageProvider : DataPageProvider
    {
        public override int ColumnCount => 0;

        public override string[] Columns => Array.Empty<string>();

        public override DataPage? Current => _current;

        private bool _isClosed;
        private DataPage? _current;

        public override bool Next()
        {
            if (_isClosed)
            {
                _current = null;
                return false;
            }

            _current = new DataPage("", Array.Empty<DataColumn>(), 1);
            _isClosed = true;

            return true;
        }

        public override void Dispose()
        {
        }
    }
}

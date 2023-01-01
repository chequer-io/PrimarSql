using PrimarSql.Common;
using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;

namespace PrimarSql.Engine.Planners.Nodes;

internal abstract class PlanNode
{
    public Scope Scope { get; }

    public abstract string Name { get; }

    public PlanId Id
    {
        get => _id;
        set
        {
            // TODO: Check can detect default PlanId
            if (value.Equals(default(PlanId)))
                throw new PrimarSqlException("Cannot update PlanId to null.");

            _id = value;
        }
    }

    private PlanId _id;

    protected PlanNode(PlanId id, Scope scope)
    {
        Id = id;
        Scope = scope;
    }

    protected PlanNode(Scope scope) : this(PlanId.Create(), scope)
    {
    }

    public abstract IEnumerable<PlanNode> GetChildren();

    protected abstract DataPageProvider CreateProvider(Session session);

    // TODO: Combine with CreateProvider..?
    public DataPageProvider Execute(Session session)
    {
        return CreateProvider(session);
    }
}

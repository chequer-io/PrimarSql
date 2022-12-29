namespace PrimarSql.Parser.Tree;

public abstract class QueryBody : Relation
{
    protected QueryBody(NodePosition position) : base(position)
    {
    }
}

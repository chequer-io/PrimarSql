namespace PrimarSql.Parser.Tree;

public abstract class Expression : Node
{
    protected Expression(NodePosition position) : base(position)
    {
    }
}

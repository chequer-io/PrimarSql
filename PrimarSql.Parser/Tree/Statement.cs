namespace PrimarSql.Parser.Tree;

public abstract class Statement : Node
{
    protected Statement(NodePosition position) : base(position)
    {
    }
}

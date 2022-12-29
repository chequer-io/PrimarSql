namespace PrimarSql.Parser.Tree;

public class AllColumns : SelectItem
{
    public Expression? Target { get; }

    public Identifier[] Aliases { get; }

    public AllColumns(NodePosition position, Expression? target, Identifier[] aliases) : base(position)
    {
        Target = target;
        Aliases = aliases;
    }
}

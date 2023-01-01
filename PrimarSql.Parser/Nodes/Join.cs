using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class Join : Relation
{
    public JoinType JoinType { get; }

    public Relation Left { get; }

    public Relation Right { get; }

    // Caiteria (ex. On, Natural, Using ...)

    public Join(JoinType joinType, Relation left, Relation right)
    {
        JoinType = joinType;
        Left = left;
        Right = right;
    }

    public override IEnumerable<Node> GetChildren()
    {
        throw new System.NotImplementedException();
    }
}

public enum JoinType
{
    Cross,
    Inner,
    Left,
    Right,
    Full,
    Implicit
}

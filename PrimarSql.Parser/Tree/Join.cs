using System.Collections.Generic;

namespace PrimarSql.Parser.Tree;

public class Join : Relation
{
    public JoinType JoinType { get; }

    public Relation Left { get; }

    public Relation Right { get; }

    // Caiteria (ex. On, Natural, Using ...)

    public Join(NodePosition position, JoinType joinType, Relation left, Relation right) : base(position)
    {
        JoinType = joinType;
        Left = left;
        Right = right;
    }

    public override IEnumerable<T> GetChildrens<T>()
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

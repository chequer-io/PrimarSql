using System.Collections.Generic;

namespace PrimarSql.Parser.Tree;

public abstract class Relation : Node
{
    protected Relation(NodePosition position) : base(position)
    {
    }
}

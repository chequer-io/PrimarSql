using System.Collections.Generic;

namespace PrimarSql.Parser.Tree;

public abstract class SelectItem : Node
{
    protected SelectItem(NodePosition position) : base(position)
    {
    }

    public override IEnumerable<T> GetChildrens<T>()
    {
        yield break;
    }
}

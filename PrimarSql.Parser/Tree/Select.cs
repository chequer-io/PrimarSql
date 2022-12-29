using System.Collections.Generic;

namespace PrimarSql.Parser.Tree;

public class Select : Node
{
    public bool Distinct { get; }

    public SelectItem[] SelectItems { get; }

    public Select(NodePosition position, bool distinct, SelectItem[] selectItems) : base(position)
    {
        Distinct = distinct;
        SelectItems = selectItems;
    }

    public override IEnumerable<T> GetChildrens<T>()
    {
        yield break;
    }
}

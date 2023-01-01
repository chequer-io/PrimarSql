using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class Select : Node
{
    public bool Distinct { get; }

    public SelectItem[] SelectItems { get; }

    public Select(bool distinct, SelectItem[] selectItems)
    {
        Distinct = distinct;
        SelectItems = selectItems;
    }

    public override IEnumerable<Node> GetChildren()
    {
        return SelectItems;
    }
}

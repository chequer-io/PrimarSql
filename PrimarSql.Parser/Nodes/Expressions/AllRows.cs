using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class AllRows : Expression
{
    public override IEnumerable<Node> GetChildren()
    {
        yield break;
    }
}

using System.Collections.Generic;
using System.Linq;

namespace PrimarSql.Parser.Nodes;

public class Table : QueryBody
{
    public QualifiedName Name { get; }

    public Table(QualifiedName name)
    {
        Name = name;
    }

    public override IEnumerable<Node> GetChildren()
    {
        return Name.Identifiers.Select(identifier => identifier);
    }
}

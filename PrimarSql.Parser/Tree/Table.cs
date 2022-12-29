using System.Collections.Generic;
using System.Linq;

namespace PrimarSql.Parser.Tree;

public class Table : QueryBody
{
    public QualifiedName Name { get; }

    public Table(NodePosition position, QualifiedName name) : base(position)
    {
        Name = name;
    }

    public override IEnumerable<Node> GetChildrens<Node>()
    {
        return Name.Identifiers.Select(identifier => identifier).OfType<Node>();
    }
}

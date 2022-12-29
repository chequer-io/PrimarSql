using System.Linq;

namespace PrimarSql.Parser.Tree;

public class QualifiedName
{
    public Identifier[] Identifiers { get; }

    public string[] IdentifierStrings => _identifierStrings ??= Identifiers.Select(i => i.Value).ToArray();

    private string? _name;
    private string[]? _identifierStrings;

    public QualifiedName(Identifier[] identifiers)
    {
        Identifiers = identifiers;
    }

    public override string ToString()
    {
        return _name ??= string.Join(".", IdentifierStrings);
    }
}

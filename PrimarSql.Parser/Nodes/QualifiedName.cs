using System.Linq;
using PrimarSql.Common;

namespace PrimarSql.Parser.Nodes;

public class QualifiedName
{
    public Identifier[] Identifiers { get; }

    public int Length => Identifiers.Length;

    public string[] Strings => _strings ??= Identifiers.Select(i => i.Unescape()).ToArray();

    private string? _name;
    private string[]? _strings;

    public QualifiedName(Identifier[] identifiers)
    {
        Identifiers = identifiers;
    }

    public static QualifiedName From(string name)
    {
        return From(new Identifier(name));
    }

    public static QualifiedName From(Identifier identifier)
    {
        return new QualifiedName(new[] { identifier });
    }

    public override string ToString()
    {
        return _name ??= string.Join(".", Strings);
    }

    public QualifiedName SubIdentifierOffset(int offset)
    {
        return new QualifiedName(Identifiers.Skip(offset).ToArray());
    }

    public QualifiedName SubIdentifier(int length)
    {
        return new QualifiedName(Identifiers.Take(length).ToArray());
    }

    public QualifiedName SubIdentifier(int offset, int length)
    {
        return new QualifiedName(Identifiers.Skip(offset).Take(length).ToArray());
    }

    public ObjectName ToObjectName()
    {
        return Strings.Length switch
        {
            1 => new ObjectName(Strings[0]),
            2 => new ObjectName(Strings[0], Strings[1]),
            3 => new ObjectName(Strings[0], Strings[1], Strings[2]),
            _ => throw new PrimarSyntaxException("ObjectName length must be 1 to 3.")
        };
    }
}

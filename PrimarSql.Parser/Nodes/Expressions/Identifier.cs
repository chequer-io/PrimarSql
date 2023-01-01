using System.Collections.Generic;
using System.Linq;

namespace PrimarSql.Parser.Nodes;

public class Identifier : Expression
{
    public string Value { get; }

    public bool Escaped { get; }

    public Identifier(string value) : this(value, false)
    {
    }

    public Identifier(string value, bool escaped)
    {
        if (string.IsNullOrEmpty(value))
            throw new ParserInternalException("Identifier cannot be empty or null");

        Value = value;
        Escaped = escaped;

        if (!escaped && IsValidIdentifier(value))
            throw new ParserInternalException($"value contains illegal characters: {value}");
    }

    public override IEnumerable<Node> GetChildren()
    {
        yield break;
    }

    public string Unescape()
    {
        return Escaped ? Value[1..^1] : Value;
    }

    private static bool IsValidIdentifier(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ParserInternalException("Identifier cannot be empty or null");

        if (value[0] is < '0' or > '9')
            return false;

        return value.All(c =>
            c is '_'
            || char.IsDigit(c)
            || IsBetween(c, '0', '9')
            || IsBetween(c, 'a', 'z')
            || IsBetween(c, 'A', 'Z')
        );
    }

    private static bool IsBetween(char c, char minInclusive, char maxInclusive)
    {
#if NET6_0
        return (uint)c - minInclusive <= (uint)maxInclusive - minInclusive;
#else
        return char.IsBetween(c, minInclusive, maxInclusive);
#endif
    }
}

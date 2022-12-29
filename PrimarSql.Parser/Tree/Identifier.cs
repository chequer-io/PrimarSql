using System.Collections.Generic;
using System.Linq;

namespace PrimarSql.Parser.Tree;

public class Identifier : Expression
{
    public string Value { get; }

    public bool Delimited { get; }

    public Identifier(NodePosition position, string value, bool delimited) : base(position)
    {
        if (string.IsNullOrEmpty(value))
            throw new ParserException("Identifier cannot be empty or null");

        Value = value;
        Delimited = delimited;

        if (!delimited && IsValidIdentifier(value))
            throw new ParserException($"value contains illegal characters: {value}");
    }

    public override IEnumerable<T> GetChildrens<T>()
    {
        yield break;
    }

    private static bool IsValidIdentifier(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ParserException("Identifier cannot be empty or null");

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

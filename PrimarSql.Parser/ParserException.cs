using System;

namespace PrimarSql.Parser;

public class ParserException : Exception
{
    public ParserException(string? message) : base(message)
    {
    }
}

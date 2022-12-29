using System;

namespace PrimarSql.Parser;

public class ParserInternalException : Exception
{
    public ParserInternalException(string message) : base($"Internal error occured while parsing query (message: {message})")
    {
    }
}

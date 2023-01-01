using PrimarSql.Common;

namespace PrimarSql.Parser;

public class ParserInternalException : PrimarSqlException
{
    public ParserInternalException(string message) : base(ErrorCode.Internal, $"Internal error occured while parsing query (message: {message})")
    {
    }
}

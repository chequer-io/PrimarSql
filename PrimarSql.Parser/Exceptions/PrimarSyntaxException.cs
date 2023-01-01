using PrimarSql.Common;

namespace PrimarSql.Parser;

public class PrimarSyntaxException : PrimarSqlException
{
    public PrimarSyntaxException(string errorMessage) : base(ErrorCode.Syntax, errorMessage)
    {
    }
}

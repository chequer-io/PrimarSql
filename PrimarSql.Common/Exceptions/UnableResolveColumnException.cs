namespace PrimarSql.Common;

public class UnableResolveColumnException : PrimarSqlException
{
    public UnableResolveColumnException(string columnName) 
        : base(ErrorCode.UnableResolveColumn, $"Unable to resolve column '{columnName}'.")
    {
    }
}

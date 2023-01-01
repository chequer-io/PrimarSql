namespace PrimarSql.Common;

public class UnableResolveTableException : PrimarSqlException
{
    public UnableResolveTableException(ObjectName name)
        : base(ErrorCode.UnableResolveTable, $"Unable to resolve table '{name}'.")
    {
    }
}

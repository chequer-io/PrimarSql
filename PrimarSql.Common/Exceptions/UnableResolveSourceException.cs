namespace PrimarSql.Common;

public class UnableResolveSourceException : PrimarSqlException
{
    public UnableResolveSourceException(ObjectName name)
        : base(ErrorCode.UnableResolveSource, $"Unable to resolve source '{name}'.")
    {
    }
}

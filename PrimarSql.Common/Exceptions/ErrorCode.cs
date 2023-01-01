namespace PrimarSql.Common;

public enum ErrorCode
{
    Unknown,
    Vendor,
    Internal,
    Syntax,
    NotSupportedFeature,
    UnableResolveSource,
    UnableResolveTable,
    UnableResolveColumn,
}

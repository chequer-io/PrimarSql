namespace PrimarSql.Common.Metadata;

public class DynamicColumnInfo : ColumnInfo
{
    public DynamicColumnInfo(string columnName) : base(columnName, ColumnType.Dynamic)
    {
    }
}

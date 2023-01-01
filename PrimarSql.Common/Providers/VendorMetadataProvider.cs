using PrimarSql.Common.Metadata;

namespace PrimarSql.Common.Providers;

public abstract class VendorMetadataProvider : IMetadataProvider
{
    public virtual TableInfo? GetTableMetadata(ObjectName name)
        => throw new NotSupportedFeatureException("Get table metadata from name");

    public virtual ViewInfo? GetViewMetadata(ObjectName name)
        => throw new NotSupportedFeatureException("Get view metadata from name");

    public virtual IndexInfo? GetIndexMetadata(TableInfo table)
        => throw new NotSupportedFeatureException("Get index metadata from table");

    public virtual IEnumerable<ColumnInfo>? GetTableColumns(ObjectName table)
        => throw new NotSupportedFeatureException("Get columns metadata from table");

    public virtual string GetDatabaseName()
        => throw new PrimarSqlException(ErrorCode.NotSupportedFeature, "this vendor does not supported database.");

    public virtual string GetSchemaName()
        => throw new PrimarSqlException(ErrorCode.NotSupportedFeature, "this vendor does not supported schema.");

    public virtual void SetDatabaseName(string database)
        => throw new PrimarSqlException(ErrorCode.NotSupportedFeature, "this vendor does not supported database.");

    public virtual void SetSchemaName(string schema)
        => throw new PrimarSqlException(ErrorCode.NotSupportedFeature, "this vendor does not supported schema.");

    public virtual ObjectName ResolveObjectName(ObjectName name)
    {
        return name;
    }
}

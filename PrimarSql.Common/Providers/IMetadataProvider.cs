using PrimarSql.Common.Metadata;

namespace PrimarSql.Common.Providers;

public interface IMetadataProvider
{
    TableInfo? GetTableMetadata(ObjectName name);

    ViewInfo? GetViewMetadata(ObjectName name);

    IndexInfo? GetIndexMetadata(TableInfo table);

    IEnumerable<ColumnInfo>? GetTableColumns(ObjectName table);

    ObjectName ResolveObjectName(ObjectName name);

    string GetDatabaseName();

    string GetSchemaName();

    void SetDatabaseName(string database);

    void SetSchemaName(string schema);

    // TODO: MaterializedView / Synonym etc..
}

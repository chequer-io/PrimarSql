using PrimarSql.Common.Providers;

namespace PrimarSql.Common.Metadata;

public class TableInfo : IMetadata
{
    public ObjectName Name { get; }

    public long EstimatedCount { get; }

    private readonly IMetadataProvider _provider;

    public TableInfo(IMetadataProvider provider, ObjectName name, long estimatedCount)
    {
        _provider = provider;
        Name = name;
        EstimatedCount = estimatedCount;
    }

    public IEnumerable<ColumnInfo> GetColumns()
    {
        if (_provider.GetTableColumns(Name) is { } columns)
            return columns;

        // TODO: is it a.. exception?
        return Enumerable.Empty<ColumnInfo>();
    }
}

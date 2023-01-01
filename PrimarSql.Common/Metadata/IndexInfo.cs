namespace PrimarSql.Common.Metadata;

public class IndexInfo
{
    public string IndexName { get; }

    public TableInfo Table { get; }

    public IndexInfo(TableInfo table, string indexName)
    {
        IndexName = indexName;
        Table = table;
    }
}

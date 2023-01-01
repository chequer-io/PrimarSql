using PrimarSql.Common.Data;
using PrimarSql.Common.Metadata;

namespace PrimarSql.Common.Providers;

public class VendorSourceProvider : ISourceProvider
{
    public virtual DataPageProvider GetTableDataPage(TableInfo table)
        => throw new NotSupportedFeatureException("Get datapage from table");

    public virtual DataPageProvider GetIndexDataPage(IndexInfo index)
        => throw new NotSupportedFeatureException("Get datapage from index");

    public virtual DataPageProvider GetViewDataPage(ViewInfo view)
        => throw new NotSupportedFeatureException("Get datapage from view");

    public virtual int InsertToTable(ObjectName name, DataPageReader reader)
        => throw new NotSupportedFeatureException("Insert data to table");
}

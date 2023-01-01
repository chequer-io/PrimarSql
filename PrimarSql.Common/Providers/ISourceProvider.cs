using PrimarSql.Common.Data;
using PrimarSql.Common.Metadata;

namespace PrimarSql.Common.Providers;

public interface ISourceProvider
{
    DataPageProvider GetTableDataPage(TableInfo table);

    DataPageProvider GetIndexDataPage(IndexInfo index);

    DataPageProvider GetViewDataPage(ViewInfo view);

    // TODO: int to ActionResult
    int InsertToTable(ObjectName name, DataPageReader reader);
}

using PrimarSql.Common.Data;

namespace PrimarSql.Engine.PageProviders;

public class EmptyProvider : DataPageProvider
{
    public override int Affected { get; }

    public override int ColumnCount => 0;

    public override string[] Columns => Array.Empty<string>();

    public override IDataPage? Current => null;

    public EmptyProvider(int affected)
    {
        Affected = affected;
    }

    public override bool Next()
    {
        return false;
    }

    public override void Dispose()
    {
    }
}

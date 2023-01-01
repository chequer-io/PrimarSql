namespace PrimarSql.Common.Data;

public abstract class DataPageProvider
{
    public abstract int ColumnCount { get; }

    public abstract string[] Columns { get; }

    public abstract IDataPage? Current { get; }

    public virtual int Affected => -1;

    public abstract bool Next();

    public abstract void Dispose();
}

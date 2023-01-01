namespace PrimarSql.Common.Data;

public interface IDataPage
{
    string Name { get; }

    int DataCount { get; }

    int ColumnCount { get; }

    IDataPage Skip(int count);

    IDataPage Take(int count);

    DataColumn GetColumn(int columnIndex);

    object?[] Get(int index);
}

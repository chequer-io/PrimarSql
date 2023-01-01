namespace PrimarSql.Common.Data;

public class FilteredColumn : DataColumn
{
    public override string Name => _dataColumn.Name;

    public int ItemCount => _itemCount - _filtered.Count;

    private readonly DataColumn _dataColumn;
    private readonly int _itemCount;
    private readonly HashSet<int> _filtered = new();

    public FilteredColumn(DataColumn dataColumn, int itemCount)
    {
        _dataColumn = dataColumn;
        _itemCount = itemCount;
    }

    public override object? Get(int index)
    {
        if (index >= ItemCount)
            throw new PrimarSqlException($"Cannot Get item. (ItemCount: {_itemCount}, index: {index})");

        int count = -1;

        for (int i = 0; i < _itemCount; i++)
        {
            if (_filtered.Contains(i))
                continue;

            count++;

            if (index == count)
                return _dataColumn.Get(i);
        }

        throw new PrimarSqlException($"Cannot Get item. (ItemCount: {_itemCount}, index: {index})");
    }

    public void SetFilterIndex(int index)
    {
        _filtered.Add(index);
    }

    public override DataColumn Skip(int count)
    {
        return new ObjectColumn(Name, BuildItems().Skip(count).ToArray());
    }

    public override DataColumn Take(int count)
    {
        return new ObjectColumn(Name, BuildItems().Take(count).ToArray());
    }

    private IEnumerable<object?> BuildItems()
    {
        for (int i = 0; i > _itemCount; i++)
        {
            if (_filtered.Contains(i))
                continue;

            yield return _dataColumn.Get(i);
        }
    }
}

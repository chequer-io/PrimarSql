using PrimarSql.Common.Data;

namespace PrimarSql.Engine.PageProviders;

internal class LimitProvider : DataPageProvider
{
    public override int ColumnCount => _source.ColumnCount;

    public override string[] Columns => _source.Columns;

    public override IDataPage? Current => _current;

    private readonly DataPageProvider _source;
    private IDataPage? _current;
    private int _remain;

    public LimitProvider(DataPageProvider source, int limit)
    {
        _remain = limit;
        _source = source;
    }

    public override bool Next()
    {
        if (_remain == 0 || !_source.Next() || _source.Current is not { } current)
        {
            _current = null;
            return false;
        }

        var page = current;

        if (_remain is -1)
        {
            _current = page;
            return true;
        }

        // Process limit

        if (current.DataCount > _remain)
            page = current.Take(_remain);

        _remain -= current.DataCount;
        _current = page;

        return true;
    }

    public override void Dispose()
    {
        _source.Dispose();
        GC.SuppressFinalize(this);
    }
}

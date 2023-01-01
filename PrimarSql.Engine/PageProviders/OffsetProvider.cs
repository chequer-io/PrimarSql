using PrimarSql.Common.Data;

namespace PrimarSql.Engine.PageProviders;

internal class OffsetProvider : DataPageProvider
{
    public override int ColumnCount => _source.ColumnCount;

    public override string[] Columns => _source.Columns;

    public override IDataPage? Current => _current;

    private readonly DataPageProvider _source;
    private IDataPage? _current;
    private int _remainOffset;

    public OffsetProvider(DataPageProvider source, int offset)
    {
        _remainOffset = offset;
        _source = source;
    }

    public override bool Next()
    {
        if (!_source.Next() || _source.Current is not { } current)
        {
            _current = null;
            return false;
        }

        var page = current;

        if (_remainOffset != 0)
        {
            // Process offset
            while (true)
            {
                if (page.DataCount <= _remainOffset)
                {
                    _remainOffset -= page.DataCount;
                }
                else
                {
                    page = page.Skip(_remainOffset);
                    _remainOffset = 0;
                    break;
                }

                if (!_source.Next())
                {
                    _current = null;
                    return false;
                }

                page = _source.Current;
            }
        }

        _current = page;

        return true;
    }

    public override void Dispose()
    {
        _source.Dispose();
        GC.SuppressFinalize(this);
    }
}

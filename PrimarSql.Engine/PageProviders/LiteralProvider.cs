using PrimarSql.Common.Data;

namespace PrimarSql.Engine.PageProviders;

public class LiteralProvider : DataPageProvider
{
    public override int ColumnCount => _data[0].Length;

    public override string[] Columns { get; }

    public override IDataPage? Current => _current;

    private readonly object?[][] _data;
    private IDataPage? _current;
    private bool _isClosed = false;

    public LiteralProvider(object?[][] data)
    {
        _data = data;
        Columns = _data[0].Select(_ => "(anonymous)").ToArray();
    }

    public override bool Next()
    {
        if (_isClosed)
        {
            _current = null;
            return false;
        }

        _current = new DataPage(
            "(anonymous)",
            Columns.Select((c, i) => new ObjectColumn(c, _data.Select(d => d[i]).ToArray())).OfType<DataColumn>().ToArray(),
            _data.Length
        );

        _isClosed = true;
        return true;
    }

    public override void Dispose()
    {
    }
}

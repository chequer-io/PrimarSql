using PrimarSql.Common.Data;
using PrimarSql.Engine.Functions;

namespace PrimarSql.Engine.PageProviders;

internal class AggregateProvider : DataPageProvider
{
    public string Name { get; }

    private readonly DataPageProvider _source;
    private readonly AggregateFunction[] _functions;
    private IDataPage? _current;
    private bool _isClosed = false;

    // TODO: do not fix AggregateFunction only. refactor to can use another fields.
    public AggregateProvider(DataPageProvider source, AggregateFunction[] functions, string name)
    {
        _source = source;
        _functions = functions;
        Name = name;

        // TODO: Fix!
        Columns = _functions.Select(f => "(anonymous)").ToArray();
    }

    public override int ColumnCount => Columns.Length;

    public override string[] Columns { get; }

    public override IDataPage? Current => _current;

    public override bool Next()
    {
        if (_isClosed)
        {
            _current = null;
            return false;
        }

        while (_source.Next() && _source.Current is { })
        {
            for (int i = 0; i < _source.Current.DataCount; i++)
            {
                foreach (var function in _functions)
                    function.Process(_source.Current.Get(i));
            }
        }

        // TODO: Seperate for GROUP BY!
        _current = new DataPage(
            Name,
            _functions.Select(f => new ObjectColumn("(anonymous)", new[] { f.Get() })).OfType<DataColumn>().ToArray(),
            1
        );

        _isClosed = true;
        return true;
    }

    public override void Dispose()
    {
        _source.Dispose();
    }
}

using PrimarSql.Common;
using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.Planners.Nodes;

namespace PrimarSql.Engine.Evaluators;

internal class EvaluateValueProvider
{
    private readonly FieldMapper _mapper;
    private readonly IDataPage _source;
    private readonly int _index;

    public EvaluateValueProvider(FieldMapper mapper, IDataPage source, int index)
    {
        _mapper = mapper;
        _source = source;
        _index = index;
    }

    public object? Get(Field field)
    {
        var fieldIndex = _mapper.Find(field);

        if (fieldIndex == -1)
            throw new PrimarSqlException($"Cannot Resolve Field {field.Name}");

        return _source.GetColumn(fieldIndex).Get(_index);
    }
}

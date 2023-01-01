using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.Evaluators;
using PrimarSql.Engine.Planners.Nodes;
using PrimarSql.Parser;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.PageProviders;

internal class FilterProvider : DataPageProvider
{
    public override int ColumnCount => _scope.RelationFrame.Fields.Length;

    public override string[] Columns => _source.Columns;

    public override IDataPage? Current => _current;

    private readonly DataPageProvider _source;
    private readonly FieldMapper _fieldMapper;
    private readonly Scope _scope;
    private readonly Expression _filterExpression;
    private IDataPage? _current;
    private readonly ExpressionEvaluator _evaluator;

    public FilterProvider(
        DataPageProvider source,
        FieldMapper fieldMapper,
        Scope scope,
        Expression filterExpression)
    {
        _source = source;
        _fieldMapper = fieldMapper;
        _scope = scope;
        _filterExpression = filterExpression;

        _evaluator = new ExpressionEvaluator(fieldMapper.Scope);
    }

    public override bool Next()
    {
        while (true)
        {
            if (!_source.Next() || _source.Current is null)
            {
                _current = null;
                return false;
            }

            _current = Filter(_source.Current);

            if (_current.DataCount != 0)
                return true;
        }
    }

    public override void Dispose()
    {
        _source.Dispose();
    }

    private IDataPage Filter(IDataPage source)
    {
        int dataCount = source.DataCount;

        FilteredColumn[] filteredColumns = Enumerable.Range(0, source.ColumnCount)
            .Select(source.GetColumn)
            .Select(c => new FilteredColumn(c, source.DataCount))
            .ToArray();

        for (int i = 0; i < source.DataCount; i++)
        {
            var result = _evaluator.Process(_filterExpression, new EvaluateValueProvider(_fieldMapper, source, i));

            if (result is not bool matched)
                throw new PrimarSyntaxException("Filter expression result is not boolean");

            if (!matched)
            {
                foreach (var filteredColumn in filteredColumns)
                    filteredColumn.SetFilterIndex(i);

                dataCount--;
            }
        }

        return new DataPage(source.Name, filteredColumns.OfType<DataColumn>().ToArray(), dataCount);
    }
}

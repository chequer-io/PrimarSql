using PrimarSql.Common;
using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.Columns;
using PrimarSql.Engine.Planners.Nodes;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.PageProviders;

internal class ProjectionProvider : DataPageProvider
{
    public override int ColumnCount => _scope.RelationFrame.Fields.Length;

    public override string[] Columns { get; }

    public override DataPage? Current => _current;

    private readonly DataPageProvider _source;
    private readonly FieldMapper _fieldMapper;
    private readonly Scope _scope;
    private readonly Expression?[] _expressions;
    private readonly DataPage.Builder _pageBuilder;
    private DataPage? _current;

    public ProjectionProvider(
        DataPageProvider source,
        FieldMapper fieldMapper,
        string name,
        Scope scope,
        Expression?[] expressions)
    {
        _source = source;
        _fieldMapper = fieldMapper;
        _scope = scope;
        _expressions = expressions;

        Columns = _scope.RelationFrame.Fields.Select((f, i) =>
        {
            if (f.Name is { })
                return f.Name;

            if (_expressions[i] is Literal literal)
                return literal.Value?.ToString() ?? "(anonymous)";

            return "(anonymous)";
        }).ToArray();

        _pageBuilder = new DataPage.Builder()
            .SetName(name);
    }

    public override bool Next()
    {
        if (!_source.Next() || _source.Current is null)
        {
            _current = null;
            return false;
        }

        var sourceCurrent = _source.Current;
        _pageBuilder.ClearColumns();
        int index = 0;

        foreach (var fieldIndex in _scope.RelationFrame.Fields.Select(f => _fieldMapper.Find(f)))
        {
            if (fieldIndex == -1)
            {
                if (_expressions[index] is not { } expression)
                    throw new PrimarSqlException("Expression is null");

                if (expression is Literal literal)
                {
                    _pageBuilder.AddColumn(new LiteralColumn(literal));
                }
                else
                {
                    _pageBuilder.AddColumn(new ExpressionColumn(expression, _fieldMapper, sourceCurrent, "(anonymous)"));
                }
            }
            else
            {
                _pageBuilder.AddColumn(sourceCurrent.GetColumn(fieldIndex));
            }

            index++;
        }

        _current = _pageBuilder.SetDataCount(sourceCurrent.DataCount).Build();
        return true;
    }

    public override void Dispose()
    {
        _source.Dispose();
    }
}

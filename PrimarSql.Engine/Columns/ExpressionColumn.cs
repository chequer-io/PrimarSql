using PrimarSql.Common.Data;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.Evaluators;
using PrimarSql.Engine.Planners.Nodes;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Columns;

internal class ExpressionColumn : DataColumn
{
    public override string Name { get; }

    private readonly Expression _expression;
    private readonly FieldMapper _fieldMapper;
    private readonly IDataPage _sourceCurrent;
    private readonly ExpressionEvaluator _evaluator;

    public ExpressionColumn(Expression expression, FieldMapper fieldMapper, IDataPage sourceCurrent, string name)
    {
        _expression = expression;
        _fieldMapper = fieldMapper;
        _sourceCurrent = sourceCurrent;
        _evaluator = new ExpressionEvaluator(fieldMapper.Scope);
        Name = name;
    }

    public override object? Get(int index)
    {
        return _evaluator.Process(_expression, new EvaluateValueProvider(_fieldMapper, _sourceCurrent, index));
    }

    public override DataColumn Skip(int count)
    {
        return this;
    }

    public override DataColumn Take(int count)
    {
        return this;
    }
}

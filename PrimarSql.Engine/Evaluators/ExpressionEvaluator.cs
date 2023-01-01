using System.Collections;
using PrimarSql.Common;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Parser.Nodes;
using PrimarSql.Parser.Visitors;
using static PrimarSql.Parser.Nodes.Comparison;

namespace PrimarSql.Engine.Evaluators;

internal class ExpressionEvaluator : NodeVisitorBase<object, EvaluateValueProvider>
{
    private readonly Scope _scope;

    public ExpressionEvaluator(Scope scope)
    {
        _scope = scope;
    }

    protected override object? VisitLiteral(Literal node, EvaluateValueProvider? context)
    {
        return node.Value;
    }

    protected override object? VisitArithmeticBinary(ArithmeticBinary node, EvaluateValueProvider? context)
    {
        object? left = Visit(node.Left.Value, context);
        object? right = Visit(node.Right.Value, context);

        if (left is not { } || right is not { })
            return null;

        if (left is double lNum && right is double rNum)
        {
            var value = node.Operator switch
            {
                ArithmeticBinary.ArithmeticOperator.Divide => lNum / rNum,
                ArithmeticBinary.ArithmeticOperator.Minus => lNum - rNum,
                ArithmeticBinary.ArithmeticOperator.Mod => lNum % rNum,
                ArithmeticBinary.ArithmeticOperator.Multiply => lNum * rNum,
                ArithmeticBinary.ArithmeticOperator.Plus => lNum + rNum,
                _ => 0
            };

            return Math.Round(value, 2);
        }

        if (node.Operator != ArithmeticBinary.ArithmeticOperator.Plus)
            throw new InvalidOperationException($"Cannot concat string with operator :{node.Operator}");

        return left.ToString() + right;
    }

    protected override object? VisitLogicalBinary(LogicalBinary node, EvaluateValueProvider? context)
    {
        object? left = Visit(node.Left.Value, context);
        object? right = Visit(node.Right.Value, context);

        if (left is not bool leftBool)
            leftBool = left is not null;

        if (right is not bool rightBool)
            rightBool = right is not null;

        return node.Operator switch
        {
            LogicalBinary.BinaryOperator.And => leftBool && rightBool,
            LogicalBinary.BinaryOperator.Or => leftBool || rightBool,
            _ => false
        };
    }

    protected override object VisitTargetInList(TargetInList node, EvaluateValueProvider? context)
    {
        var target = Visit(node.Target.Value, context);

        IEnumerable<int> compareResults = node.Expressions
            .Select(expr => Visit(expr, context))
            .Select(compareValue => Comparer.Default.Compare(target, compareValue));

        return node.IsNot
            ? compareResults.All(r => r != 0)
            : compareResults.Any(r => r == 0);
    }

    protected override object VisitTargetIsNull(TargetIsNull node, EvaluateValueProvider? context)
    {
        var target = Visit(node.Target.Value, context);

        return node.IsNot
            ? target != null
            : target == null;
    }

    protected override object VisitComparison(Comparison node, EvaluateValueProvider? context)
    {
        var left = Visit(node.Left.Value, context);
        var right = Visit(node.Right.Value, context);

        var compareResult = Comparer.Default.Compare(left, right);

        return compareResult switch
        {
            -1 => node.Operator switch
            {
                ComparisonOperator.Equal => false,
                ComparisonOperator.NotEqual => true,
                ComparisonOperator.GreaterThan => false,
                ComparisonOperator.GreaterThanEqual => false,
                ComparisonOperator.LessThan => true,
                ComparisonOperator.LessThanEqual => true,
                _ => false
            },
            0 => node.Operator switch
            {
                ComparisonOperator.Equal => true,
                ComparisonOperator.NotEqual => false,
                ComparisonOperator.GreaterThan => false,
                ComparisonOperator.GreaterThanEqual => true,
                ComparisonOperator.LessThan => false,
                ComparisonOperator.LessThanEqual => true,
                _ => false
            },
            1 => node.Operator switch
            {
                ComparisonOperator.Equal => false,
                ComparisonOperator.NotEqual => true,
                ComparisonOperator.GreaterThan => true,
                ComparisonOperator.GreaterThanEqual => true,
                ComparisonOperator.LessThan => false,
                ComparisonOperator.LessThanEqual => false,
                _ => false
            },
            _ => false
        };
    }

    // Type Priority
    // 1. Boolean
    // 2. String
    // 3. Numeric (int, long, double)
    // TODO: Impl
    private (object? left, object? right) HandleValuesType(object? left, object? right)
    {
        if (left is null && right is null)
            return (null, null);

        if (left is string)
        {
            return right switch
            {
                null => (left, null),
                int i => (left, i.ToString()),
                long l => (left, l.ToString()),
                double d => (left, d.ToString()),
                string s => (left, right),
            };
        }

        return (left, right);
    }

    protected override object? VisitDereference(Dereference node, EvaluateValueProvider? context)
    {
        var name = node.GetQualifiedName();

        IEnumerable<Field> fields = _scope.RelationFrame.Resolve(name);

        if (fields.FirstOrDefault() is not { } field)
            throw new UnableResolveColumnException(name.ToString());

        return context?.Get(field);
    }

    protected override object? VisitIdentifier(Identifier node, EvaluateValueProvider? context)
    {
        var name = QualifiedName.From(node.Unescape());

        IEnumerable<Field> fields = _scope.RelationFrame.Resolve(name);

        if (fields.FirstOrDefault() is not { } field)
            throw new UnableResolveColumnException(name.ToString());

        return context?.Get(field);
    }
}

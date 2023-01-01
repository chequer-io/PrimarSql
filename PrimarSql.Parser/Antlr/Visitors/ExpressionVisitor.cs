using System;
using System.Linq;
using Antlr4.Runtime.Tree;
using PrimarSql.Common;
using PrimarSql.Parser.Nodes;
using static PrimarSql.Parser.Internal.PrimarSqlParser;

namespace PrimarSql.Parser.Antlr.Visitors;

internal partial class PrimarSqlVisitor
{
    public override Expression VisitPredicated(PredicatedContext context)
    {
        var expr = Visit<Expression>(context.valueExpression());

        if (context.predicate() is { } predicate)
        {
            switch (predicate)
            {
                case ComparisonContext comparison:
                {
                    if (comparison.comparisonOperator().children[0] is not ITerminalNode opNode)
                        throw new ParserInternalException("comparison.comparisonOperator().children[0] is not ITerminalNode");

                    var op = opNode.Symbol.Type switch
                    {
                        EQ => Comparison.ComparisonOperator.Equal,
                        NEQ => Comparison.ComparisonOperator.NotEqual,
                        LT => Comparison.ComparisonOperator.LessThan,
                        LTE => Comparison.ComparisonOperator.LessThanEqual,
                        GT => Comparison.ComparisonOperator.GreaterThan,
                        GTE => Comparison.ComparisonOperator.GreaterThanEqual,
                        _ => throw new ParserInternalException($"Unknown ComparisonOperator token type: {opNode.Symbol.Type}")
                    };

                    expr = new Comparison(expr, Visit<Expression>(comparison.right), op);
                    break;
                }

                case InListContext inList:
                {
                    expr = new TargetInList(expr, Visit<Expression>(inList.expression()).ToArray(), inList.HasToken(NOT));
                    break;
                }

                case NullPredicateContext nullPredicate:
                {
                    expr = new TargetIsNull(expr, nullPredicate.HasToken(NOT));
                    break;
                }
            }
        }

        return expr.With(context);
    }

    public override Node VisitFunctionCall(FunctionCallContext context)
    {
        if (context.HasToken(ASTERISK))
        {
            return new AggregateFunctionCall(GetQualifiedName(context.qualifiedName()));
        }

        throw new NotSupportedFeatureException("function call");
    }

    #region Identifier
    public override Identifier VisitUnquotedIdentifier(UnquotedIdentifierContext context)
    {
        return new Identifier(context.GetText().ToUpper(), false).With(context);
    }

    public override Node VisitQuotedIdentifier(QuotedIdentifierContext context)
    {
        return new Identifier(context.GetText(), true).With(context);
    }

    public override Node VisitBackQuotedIdentifier(BackQuotedIdentifierContext context)
    {
        return new Identifier(context.GetText(), true).With(context);
    }
    #endregion

    #region Literal
    public override Literal VisitNullLiteral(NullLiteralContext context)
    {
        return new Literal(LiteralType.Null, null).With(context);
    }

    public override Literal VisitNumericLiteral(NumericLiteralContext context)
    {
        return new Literal(LiteralType.Numeric, double.Parse(context.number().GetText())).With(context);
    }

    public override Literal VisitBasicStringLiteral(BasicStringLiteralContext context)
    {
        return new Literal(LiteralType.String, context.GetText()[1..^1].Replace("''", "'")).With(context);
    }

    public override Node VisitBooleanValue(BooleanValueContext context)
    {
        return new Literal(LiteralType.Boolean, context.TRUE() is { });
    }
    #endregion

    public override Node VisitRowConstructor(RowConstructorContext context)
    {
        return new RowConstructor(Visit<Expression>(context.expression()).ToArray()).With(context);
    }

    public override Node VisitArithmeticBinary(ArithmeticBinaryContext context)
    {
        var node = new ArithmeticBinary(Visit<Expression>(context.left), Visit<Expression>(context.right))
        {
            Operator = context.@operator.Type switch
            {
                ASTERISK => ArithmeticBinary.ArithmeticOperator.Multiply,
                PLUS => ArithmeticBinary.ArithmeticOperator.Plus,
                MINUS => ArithmeticBinary.ArithmeticOperator.Minus,
                SLASH => ArithmeticBinary.ArithmeticOperator.Divide,
                PERCENT => ArithmeticBinary.ArithmeticOperator.Mod,
                _ => throw new ParserInternalException($"Unknown operator token type: {context.@operator.Type}")
            }
        };

        return node.With(context);
    }

    public override Node VisitDereference(DereferenceContext context)
    {
        return new Dereference(
            Visit<Expression>(context.expr),
            Visit<Identifier>(context.fieldName)
        );
    }

    private QualifiedName GetQualifiedName(QualifiedNameContext context)
    {
        return new QualifiedName(Visit<Identifier>(context.identifier()).ToArray());
    }

    public override UnaryExpression VisitLogicalNot(LogicalNotContext context)
    {
        return new UnaryExpression(
            Visit<Expression>(context.booleanExpression()),
            UnaryExpression.UnaryOperator.Not
        ).With(context);
    }

    public override Node VisitLogicalBinary(LogicalBinaryContext context)
    {
        return new LogicalBinary(
            Visit<Expression>(context.left),
            Visit<Expression>(context.right),
            GetOperator()
        ).With(context);

        LogicalBinary.BinaryOperator GetOperator()
        {
            switch (context.@operator.Type)
            {
                case AND:
                    return LogicalBinary.BinaryOperator.And;

                case OR:
                    return LogicalBinary.BinaryOperator.Or;

                default:
                    throw new ParserInternalException($"Cannot Get BinaryOperator Type: {context.@operator.Type}");
            }
        }
    }
}

using System;
using System.Linq;
using PrimarSql.Common;
using PrimarSql.Parser.Internal;
using PrimarSql.Parser.Nodes;
using static PrimarSql.Parser.Internal.PrimarSqlParser;

namespace PrimarSql.Parser.Antlr.Visitors;

internal partial class PrimarSqlVisitor : PrimarSqlBaseVisitor<Node>
{
    public override Node VisitQuery(QueryContext context)
    {
        if (context.with() is { })
            throw new NotImplementedException("With (CTE)");

        return Visit(context.queryNoWith());
    }

    public override Node VisitQueryNoWith(QueryNoWithContext context)
    {
        var result = Visit(context.queryTerm());

        var query = result switch
        {
            QueryBody queryBody => new Query(queryBody),
            Query queryResult => queryResult,
            _ => throw new ParserInternalException($"Visit(context.queryTerm()) returns not QueryBody, Query. type: {result.GetType().Name}")
        };

        // TODO: ORDER BY, FETCH
        if (context.offset is { } offset)
        {
            Expression rowCount;

            if (offset.HasToken(INTEGER_VALUE))
            {
                rowCount = new Literal(LiteralType.Numeric, int.Parse(offset.GetText())).With(offset);
            }
            else
            {
                throw new NotSupportedFeatureException("Question Parameter");
            }

            query.Offset.Value = new Offset(rowCount).With(context.OFFSET().Symbol, context.offset.Stop);
        }

        if (context.limit is { } limit)
        {
            Expression rowCount;

            if (limit.ALL() != null)
            {
                rowCount = new AllRows().With(context.limit.ALL());
            }
            else if (limit.rowCount().HasToken(INTEGER_VALUE))
            {
                rowCount = new Literal(LiteralType.Numeric, int.Parse(limit.GetText())).With(limit);
            }
            else
            {
                throw new NotSupportedFeatureException("Question Parameter");
            }

            query.Limit.Value = new Limit(rowCount).With(context.LIMIT().Symbol, context.limit.Stop);
        }

        return query.With(context);
    }

    public override Node VisitQueryTermDefault(QueryTermDefaultContext context)
    {
        return Visit(context.queryPrimary());
    }

    public override Node VisitSetOperation(SetOperationContext context)
    {
        throw new NotImplementedException("INTERSECT / UNION / EXCEPT");
    }

    public override Node VisitInlineTable(InlineTableContext context)
    {
        return new InlineTable(Visit<Expression>(context.expression()).ToArray()).With(context);
    }

    public override QuerySpecification VisitQuerySpecification(QuerySpecificationContext context)
    {
        Relation? from = null;
        RelationContext? firstContext = null;

        foreach (var relation in Visit<Relation>(context.relation()))
        {
            if (from is null)
            {
                from = relation;
                firstContext = context.relation(0);
            }
            else
            {
                if (firstContext is not { } || relation is not { })
                    throw new ParserInternalException("firstContext or relation is null.");

                // TODO: Check NodePosition
                from = new Join(JoinType.Implicit, from, relation).With(firstContext);
            }
        }

        var select = new Select(
            context.setQuantifier().HasToken(DISTINCT), // Distnict
            Visit<SelectItem>(context.selectItem()).ToArray() // Columns
        ).With(context);

        var specification = new QuerySpecification(select);

        if (from is not null)
            specification.Relation.Value = from; // FROM <source>

        if (context.where is not null)
            specification.Where.Value = Visit<Expression>(context.where);

        return specification.With(context);
    }

    public override SingleColumn VisitSelectSingle(SelectSingleContext context)
    {
        var singleColumn = new SingleColumn(
            Visit<Expression>(context.expression())
        );

        if (context.identifier() is { })
            singleColumn.Alias.Value = Visit<Identifier>(context.identifier());

        return singleColumn.With(context);
    }

    public override AllColumns VisitSelectAll(SelectAllContext context)
    {
        Identifier[] identifiers = Visit<Identifier>(context.columnAliases()?.identifier()).ToArray();
        var node = new AllColumns(identifiers);

        if (VisitIfNotNull<Expression>(context.primaryExpression()) is { } target)
        {
            node.Target.Value = target;
        }

        return node.With(context);
    }

    public override Node VisitJoinRelation(JoinRelationContext context)
    {
        return base.VisitJoinRelation(context);
    }

    public override Node VisitSampledRelation(SampledRelationContext context)
    {
        var child = Visit<Relation>(context.patternRecognition());

        // TODO: Return SampledRelation if TABLESAMPLE is not null

        return child;
    }

    public override Node VisitPatternRecognition(PatternRecognitionContext context)
    {
        var child = Visit<Relation>(context.aliasedRelation());

        // TODO: Return PatternRecognition if MATCH_RECOGNIZE is not null or Remove spec

        return child;
    }

    public override Relation VisitAliasedRelation(AliasedRelationContext context)
    {
        var child = Visit<Relation>(context.relationPrimary());

        if (context.identifier() is not { } aliasContext)
            return child;

        var alias = Visit<Identifier>(aliasContext);
        Identifier[]? columnAliases = null;

        if (context.columnAliases() is { } columnAliasesContext)
            columnAliases = Visit<Identifier>(columnAliasesContext.identifier()).ToArray();

        return new AliasedRelation(child, alias, columnAliases).With(context);
    }

    public override Node VisitTableName(TableNameContext context)
    {
        return new Table(GetQualifiedName(context.qualifiedName())).With(context);
    }

    public override Node VisitSubqueryRelation(SubqueryRelationContext context)
    {
        return new TableSubquery(Visit<Query>(context.query()));
    }

    public override Node VisitSubquery(SubqueryContext context)
    {
        return Visit<Node>(context.queryNoWith());
    }
}

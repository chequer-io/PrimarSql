using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using PrimarSql.Parser.Internal;
using PrimarSql.Parser.Tree;
using static PrimarSql.Parser.Internal.PrimarSqlParser;

namespace PrimarSql.Parser;

internal partial class PrimarSqlVisitor : PrimarSqlBaseVisitor<Node>
{
    public override QuerySpecification VisitQuerySpecification(QuerySpecificationContext context)
    {
        Relation? from = null;
        RelationContext firstContext = null;

        foreach (var relation in context.relation().Select(Visit).Select(n => n.To<Relation>()!))
        {
            if (from is null)
            {
                from = relation;
                firstContext = context.relation(0);
            }
            else
            {
                from = new Join(firstContext!.GetPosition(), JoinType.Implicit, from, relation);
            }
        }

        return new QuerySpecification(
            context.GetPosition(),
            new Select(
                context.SELECT().GetPosition(),
                context.setQuantifier().IsDistinct(), // Distnict
                Visit<SelectItem>(context.selectItem()).ToArray() // Columns
            ),
            from // FROM <source>
        );
    }

    public override Node VisitSelectSingle(SelectSingleContext context)
    {
        return base.VisitSelectSingle(context);
    }

    public override AllColumns VisitSelectAll(SelectAllContext context)
    {
        var expression = VisitIfNotNull<Expression>(context.primaryExpression());
        Identifier[] identifiers = Visit<Identifier>(context.columnAliases()?.identifier()).ToArray();

        return new AllColumns(context.GetPosition(), expression, identifiers);
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

    public override Node VisitAliasedRelation(AliasedRelationContext context)
    {
        var child = Visit<Relation>(context.relationPrimary());

        // TODO: Return aliasedRelation if identifier is not null

        return child;
    }

    public override Node VisitTableName(TableNameContext context)
    {
        return new Table(context.GetPosition(), GetQualifiedName(context.qualifiedName()));
    }

    private T? VisitIfNotNull<T>(ParserRuleContext? context) where T : Node
    {
        return context is null
            ? null
            : Visit<T>(context);
    }

    private T Visit<T>(ParserRuleContext context) where T : Node
    {
        return Visit(context).To<T>()!;
    }

    private IEnumerable<T> Visit<T>(IEnumerable<ParserRuleContext>? contexts) where T : Node
    {
        return contexts?.Select(Visit<T>).ToArray() ?? Enumerable.Empty<T>();
    }
}

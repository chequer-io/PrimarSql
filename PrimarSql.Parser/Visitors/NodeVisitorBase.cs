using PrimarSql.Parser.Nodes;

namespace PrimarSql.Parser.Visitors;

public abstract class NodeVisitorBase<TValue, TContext> where TValue : class where TContext : class
{
    public virtual TValue? Process(Node node)
    {
        return Process(node, null);
    }

    public virtual TValue? Process(Node node, TContext? context)
    {
        return node.Accept(this, context);
    }

    public virtual TValue? Visit(Node node, TContext? context)
    {
        return node switch
        {
            #region Statement
            InsertInto insertInto => VisitInsertInto(insertInto, context),
            Query query => VisitQuery(query, context),
            Statement statement => VisitStatement(statement, context),
            #endregion

            #region Relation
            InlineTable inlineTable => VisitInlineTable(inlineTable, context),
            AliasedRelation aliasedRelation => VisitAliasedRelation(aliasedRelation, context),
            QuerySpecification querySpecification => VisitQuerySpecification(querySpecification, context),
            Table table => VisitTable(table, context),
            TableSubquery tableSubquery => VisitTableSubquery(tableSubquery, context),
            QueryBody queryBody => VisitQueryBody(queryBody, context),
            Relation relation => VisitRelation(relation, context),
            #endregion

            #region Expression
            ArithmeticBinary arithmeticBinary => VisitArithmeticBinary(arithmeticBinary, context),
            LogicalBinary logicalBinary => VisitLogicalBinary(logicalBinary, context),
            Comparison comparison => VisitComparison(comparison, context),
            TargetInList targetInList => VisitTargetInList(targetInList, context),
            TargetIsNull targetIsNull => VisitTargetIsNull(targetIsNull, context),
            Identifier identifier => VisitIdentifier(identifier, context),
            Dereference dereference => VisitDereference(dereference, context),
            Literal literal => VisitLiteral(literal, context),
            UnaryExpression unaryExpression => VisitUnaryExpression(unaryExpression, context),
            Expression expression => VisitExpression(expression, context),
            #endregion

            _ => VisitNode(node, context)
        };
    }

    protected virtual TValue? VisitNode(Node node, TContext? context) => null;

    #region Statement
    protected virtual TValue? VisitStatement(Statement node, TContext? context) => VisitNode(node, context);

    protected virtual TValue? VisitQuery(Query node, TContext? context) => VisitStatement(node, context);

    protected virtual TValue? VisitInsertInto(InsertInto node, TContext? context) => VisitStatement(node, context);
    #endregion

    #region Relation
    protected virtual TValue? VisitRelation(Relation node, TContext? context) => VisitNode(node, context);

    protected virtual TValue? VisitAliasedRelation(AliasedRelation node, TContext? context) => VisitRelation(node, context);

    protected virtual TValue? VisitQueryBody(QueryBody node, TContext? context) => VisitRelation(node, context);

    protected virtual TValue? VisitQuerySpecification(QuerySpecification node, TContext? context) => VisitQueryBody(node, context);

    protected virtual TValue? VisitTable(Table node, TContext? context) => VisitQueryBody(node, context);

    protected virtual TValue? VisitTableSubquery(TableSubquery node, TContext? context) => VisitQueryBody(node, context);

    protected virtual TValue? VisitInlineTable(InlineTable node, TContext? context) => VisitQueryBody(node, context);
    #endregion

    #region Expression
    protected virtual TValue? VisitExpression(Expression node, TContext? context) => VisitNode(node, context);

    protected virtual TValue? VisitComparison(Comparison node, TContext? context) => VisitExpression(node, context);

    protected virtual TValue? VisitIdentifier(Identifier node, TContext? context) => VisitExpression(node, context);

    protected virtual TValue? VisitDereference(Dereference node, TContext? context) => VisitExpression(node, context);

    protected virtual TValue? VisitLiteral(Literal node, TContext? context) => VisitExpression(node, context);

    protected virtual TValue? VisitUnaryExpression(UnaryExpression node, TContext? context) => VisitExpression(node, context);

    protected virtual TValue? VisitArithmeticBinary(ArithmeticBinary node, TContext? context) => VisitExpression(node, context);

    protected virtual TValue? VisitTargetIsNull(TargetIsNull node, TContext? context) => VisitExpression(node, context);

    protected virtual TValue? VisitTargetInList(TargetInList node, TContext? context) => VisitExpression(node, context);

    protected virtual TValue? VisitLogicalBinary(LogicalBinary node, TContext? context) => VisitExpression(node, context);
    #endregion
}

public abstract class NodeVisitorBase<TValue> : NodeVisitorBase<TValue, Empty> where TValue : class
{
}

public class Empty
{
    public static readonly Empty Value = new();

    private Empty()
    {
    }
}

using PrimarSql.Common;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.Evaluators;
using PrimarSql.Engine.Functions;
using PrimarSql.Engine.Planners.Nodes;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Planners;

internal class ExecutePlanner
{
    private readonly Analysis _analysis;

    public ExecutePlanner(Analysis analysis)
    {
        _analysis = analysis;
    }

    public PlanNode Plan(Statement statement)
    {
        switch (statement)
        {
            case Query query:
                return PlanQuery(query);

            case InsertInto insertInto:
                return PlanInsertInto(insertInto);
        }

        throw new NotSupportedFeatureException($"Planning from {statement.GetType().Name} node");
    }

    protected PlanNode PlanInsertInto(InsertInto insertInto)
    {
        return new InsertPlanNode(
            new Scope(RelationFrame.Empty),
            _analysis.InsertTargets[insertInto],
            insertInto.ColumnAliases,
            PlanQuery(insertInto.Query.Value)
        );
    }

    protected PlanNode PlanQuery(Query node)
    {
        var plan = PlanQueryBody(node.QueryBody.Value);

        plan = PlanOffset(plan, node.Offset);
        plan = PlanLimit(plan, node.Limit);

        // TODO: Query another (etc..)

        return plan;
    }

    protected PlanNode PlanLimit(PlanNode plan, NullableValue<Limit> limit)
    {
        return limit.Value is not { }
            ? plan
            : new LimitPlanNode(plan.Scope, plan, _analysis.Limits[limit.Value]);
    }

    protected PlanNode PlanOffset(PlanNode plan, NullableValue<Offset> offset)
    {
        return offset.Value is not { }
            ? plan
            : new OffsetPlanNode(plan.Scope, plan, _analysis.Offsets[offset.Value]);
    }

    protected PlanNode PlanRelation(Relation node)
    {
        switch (node)
        {
            case QueryBody queryBody:
                return PlanQueryBody(queryBody);

            case AliasedRelation aliasedRelation:
                return PlanAliasedRelation(aliasedRelation);

            case Join join:
                throw new NotSupportedNodeToPlanException(node);

            default:
                throw new NotSupportedNodeToPlanException(node);
        }
    }

    protected PlanNode PlanQueryBody(QueryBody node)
    {
        switch (node)
        {
            case QuerySpecification querySpecification:
                return PlanQuerySpecification(querySpecification);

            case Table table:
                return PlanTable(table);

            case TableSubquery tableSubquery:
                return PlanTableSubquery(tableSubquery);

            case InlineTable inlineTable:
                return PlanInlineTable(inlineTable);

            default:
                throw new NotSupportedNodeToPlanException(node);
        }
    }

    protected PlanNode PlanQuerySpecification(QuerySpecification node)
    {
        var scope = _analysis.Scopes[node];
        var name = _analysis.RelationNames[node].ToString();

        var relationPlan = node.Relation.Value is { } relation
            ? PlanRelation(relation)
            : new EmptyRelationPlanNode(new Scope(RelationFrame.Empty));

        if (_analysis.IsAggregateRelation.TryGetValue(node, out var isAggregate) && isAggregate)
        {
            return new AggregatePlanNode(scope, name, relationPlan, node.Select.Value.SelectItems.Select(ConvertToFunction).ToArray());
        }

        var mapper = new FieldMapper(relationPlan.Scope, scope.RelationFrame.Fields);

        PlanNode planNode = new ProjectionPlanNode(
            scope,
            mapper,
            relationPlan,
            name,
            scope.RelationFrame.Fields.Select(f => _analysis.GetExpression(f)).ToArray()
        );

        if (node.Where.Value is { } where)
            planNode = new FilterPlanNode(scope, mapper, planNode, where);

        return planNode;
    }

    protected PlanNode PlanAliasedRelation(AliasedRelation node)
    {
        var relationPlan = node.Relation.Value is { } relation
            ? PlanRelation(relation)
            : new EmptyRelationPlanNode(new Scope(RelationFrame.Empty));

        var scope = _analysis.Scopes[node];

        return new RelationPlanNode(
            scope,
            new FieldMapper(relationPlan.Scope, scope.RelationFrame.Fields),
            relationPlan,
            _analysis.RelationNames[node].ToString(),
            scope.RelationFrame.Fields.Select(f => _analysis.GetExpression(f)).ToArray()
        );
    }

    protected PlanNode PlanTableSubquery(TableSubquery node)
    {
        var relationPlan = PlanQuery(node.Query.Value);

        var scope = _analysis.Scopes[node];

        return new RelationPlanNode(
            scope,
            new FieldMapper(relationPlan.Scope, scope.RelationFrame.Fields),
            relationPlan,
            _analysis.RelationNames[node].ToString(),
            scope.RelationFrame.Fields.Select(f => _analysis.GetExpression(f)).ToArray()
        );
    }

    private PlanNode PlanInlineTable(InlineTable node)
    {
        var scope = _analysis.Scopes[node];
        var evaluator = new ExpressionEvaluator(scope);

        var values = node.Expressions
            .OfType<RowConstructor>()
            .Select(expr => expr.Expressions
                // TODO: add EvaluatorValueProvider
                .Select(e => evaluator.Process(e, null))
                .ToArray()
            )
            .ToArray();

        return new ValuesPlanNode(scope, values);
    }

    protected PlanNode PlanTable(Table node)
    {
        if (!_analysis.Tables.TryGetValue(node, out var tableInfo))
            throw new PrimarSqlException($"{node.Name} Relation name is null.");

        return new TableSourcePlanNode(tableInfo, _analysis.GetScope(node));
    }

    // TODO: Extract to FunctionMapper!
    private AggregateFunction ConvertToFunction(SelectItem selectItem)
    {
        if (selectItem is SingleColumn singleColumn)
        {
            if (singleColumn.Expression.Value is AggregateFunctionCall aggregateFunctionCall)
            {
                var functionName = aggregateFunctionCall.Name.ToString().ToLower();

                if (functionName == "count")
                    return new CountFunction();

                throw new NotSupportedFeatureException($"Not Supported function: {functionName}");
            }

            throw new NotSupportedFeatureException($"ConvertToFunction not support {singleColumn.Expression.Value}");
        }

        throw new NotSupportedFeatureException($"ConvertToFunction not support {selectItem.GetType().Name}");
    }
}

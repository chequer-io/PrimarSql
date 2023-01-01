using PrimarSql.Common;
using PrimarSql.Common.Metadata;
using PrimarSql.Common.Providers;
using PrimarSql.Common.Utilities;
using PrimarSql.Parser;
using PrimarSql.Parser.Nodes;
using PrimarSql.Parser.Visitors;

namespace PrimarSql.Engine.Analyzers;

internal class StatementAnalyzer
{
    private readonly IMetadataProvider _metadataProvider;

    private Analysis Analysis => _analysis.NotNull("result");

    public StatementAnalyzer(IMetadataProvider metadataProvider)
    {
        _metadataProvider = metadataProvider;
    }

    private Analysis? _analysis;

    public Analysis Analyze(Statement node)
    {
        _analysis = new Analysis();

        new Visitor(this).Process(node);

        return _analysis;
    }

    private class Visitor : NodeVisitorBase<Scope, Scope>
    {
        public Analysis Analysis => _analyzer.Analysis;

        public IMetadataProvider MetadataProvider => _analyzer._metadataProvider;

        private readonly StatementAnalyzer _analyzer;

        public Visitor(StatementAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        protected override Scope? VisitInsertInto(InsertInto node, Scope context)
        {
            // TODO: Create Property from Implicit Table Creation
            var objectName = MetadataProvider.ResolveObjectName(node.Target.ToObjectName());

            Analysis.InsertTargets[node] = objectName;

            // var tableInfo = MetadataProvider.GetTableMetadata(objectName);
            //
            // if (tableInfo is not { })
            //     throw new UnableResolveSourceException(objectName);

            VisitQuery(node.Query.Value, context);

            return null;
        }

        protected override Scope VisitQuery(Query node, Scope? context)
        {
            // node.With

            var scope = Visit(node.QueryBody.Value, context).NotNull("scope");

            // node.OrderBy

            if (node.Limit.Value is { } limit)
            {
                var limitExpr = limit.RowCount.Value;

                Analysis.Limits[limit] = limitExpr switch
                {
                    AllRows => -1,
                    Literal literal => (int)(literal.Value ?? -1),
                    _ => throw new NotSupportedFeatureException($"Limit with node '{limit.GetType().Name}'")
                };
            }

            if (node.Offset.Value is { } offset)
            {
                var offsetExpr = offset.RowCount.Value;

                Analysis.Offsets[offset] = offsetExpr switch
                {
                    Literal literal => (int)(literal.Value ?? -1),
                    _ => throw new NotSupportedFeatureException($"Limit with node '{offset.GetType().Name}'")
                };
            }

            return CreateScope(node, context, scope.RelationFrame);
        }

        protected override Scope VisitQuerySpecification(QuerySpecification node, Scope? context)
        {
            var relationScope = node.Relation.Value is { } relation
                ? Visit(relation, context) ?? throw new PrimarSqlException("Scope is null")
                : new Scope(RelationFrame.Empty);

            if (node.Select.Value.SelectItems.OfType<SingleColumn>().Any(s => s.Expression.Value is AggregateFunctionCall))
            {
                Analysis.IsAggregateRelation[node] = true;
            }

            AnalyzeSelect(node, relationScope);

            var scope = BuildSelectItemsScope(node.Select.Value.SelectItems, relationScope);

            Analysis.Scopes[node] = scope;
            Analysis.RelationNames[node] = QualifiedName.From("(anonymous)");

            return scope;
        }

        private Scope BuildSelectItemsScope(IEnumerable<SelectItem> selectItems, Scope parent)
        {
            var fields = new List<Field>();

            foreach (var selectItem in selectItems)
            {
                switch (selectItem)
                {
                    case AllColumns allColumns:
                        fields.AddRange(Analysis.AllColumnsFields[allColumns]);
                        break;

                    case SingleColumn singleColumn:
                        fields.Add(Analysis.SingleColumnFields[singleColumn]);
                        break;

                    default:
                        throw new NotSupportedFeatureException($"Get fields from {selectItem.GetType().Name} SelectItem.");
                }
            }

            var relationFrame = new RelationFrame(fields.ToArray());
            return new Scope(parent, relationFrame);
        }

        private void AnalyzeSelect(QuerySpecification node, Scope scope)
        {
            foreach (var selectItem in node.Select.Value.SelectItems)
            {
                switch (selectItem)
                {
                    case AllColumns allColumns:
                        AnalyzeAllColumns(allColumns, node, scope);
                        break;

                    case SingleColumn singleColumn:
                        AnalyzeSingleColumn(singleColumn, scope);
                        break;
                }
            }
        }

        private void AnalyzeAllColumns(AllColumns allColumns, QuerySpecification node, Scope scope)
        {
            // <expr>.*
            if (allColumns.Target.Value is { } target)
            {
                var name = target switch
                {
                    Identifier identifier => QualifiedName.From(identifier.Unescape()),
                    Dereference dereference => dereference.GetQualifiedName(),
                    _ => throw new PrimarSyntaxException("AllColumn (*) Target must be source name.")
                };

                Field[] resolvedField = scope.RelationFrame.ResolveSource(name).ToArray();

                if (resolvedField.Length is 0)
                    throw new UnableResolveSourceException(name.ToObjectName());

                Analysis.AllColumnsFields[allColumns] = CreateAllColumns(allColumns, resolvedField);
            }
            // *
            else
            {
                if (allColumns.Aliases is not { })
                    throw new PrimarSyntaxException("Column aliases must need target.");

                Field[] fields = scope.RelationFrame.Fields;

                if (fields.Length == 0)
                {
                    if (!node.Relation.HasValue)
                        throw new PrimarSyntaxException("AllColumn (*) not supported when from source is empty.");

                    throw new PrimarSyntaxException("AllColumn (*) not supported when Column is empty.");
                }

                Analysis.AllColumnsFields[allColumns] = CreateAllColumns(allColumns, fields);
            }
        }

        private Field[] CreateAllColumns(AllColumns allColumns, IEnumerable<Field> fields)
        {
            return fields.Select((f, i) =>
            {
                string? alias = f.Name;

                if (allColumns.Aliases.Length != 0)
                    alias = allColumns.Aliases[i].Value;

                var newField = Field.New(alias, f.Source, f.ColumnType, f.OriginName, f.OriginSource);

                // TODO: Refactor Symbol system to can Resolve Expression only Fields
                if (f.OriginName is null && f.OriginSource is null)
                {
                    Analysis.Expressions[newField] = Analysis.Expressions[f];
                }

                return newField;
            }).ToArray();
        }

        private void AnalyzeSingleColumn(SingleColumn column, Scope scope)
        {
            var expression = column.Expression.Value;
            var field = column.Alias.Value;

            ObjectName? originTable = null;
            string? originColumn = null;

            var name = expression switch
            {
                Identifier identifier => QualifiedName.From(identifier.Unescape()),
                Dereference dereference => dereference.GetQualifiedName(),
                _ => null
            };

            if (name is not null)
            {
                IEnumerable<Field> matchingFields = scope.RelationFrame.Resolve(name);

                if (matchingFields.FirstOrDefault() is { } first)
                {
                    originTable = first.OriginSource;
                    originColumn = first.OriginName;
                }
                else
                {
                    throw new UnableResolveColumnException(name.ToString());
                }
            }

            if (field is null && name is not null)
                field = name.Identifiers[^1];

            // TODO: ColumnType from expression
            var newField = Field.New(field?.Unescape(), null, ColumnType.Unknown, originColumn, originTable);

            Analysis.Expressions[newField] = expression;
            Analysis.SingleColumnFields[column] = newField;
        }

        protected override Scope VisitTable(Table node, Scope? context)
        {
            var objectName = MetadataProvider.ResolveObjectName(node.Name.ToObjectName());
            Analysis.TableNames[node] = objectName;
            var tableInfo = MetadataProvider.GetTableMetadata(objectName);

            if (tableInfo is not { })
                throw new UnableResolveSourceException(objectName);

            Analysis.Tables[node] = tableInfo;
            Analysis.RelationNames[node] = node.Name;

            IEnumerable<Field> fields = tableInfo.GetColumns().Select(c => Field.New(
                c.ColumnName,
                node.Name,
                c.Type,
                c.ColumnName,
                objectName
            ));

            return CreateScope(node, context, new RelationFrame(fields.ToArray()));
        }

        protected override Scope VisitTableSubquery(TableSubquery node, Scope? context)
        {
            var queryScope = Visit(node.Query.Value, context) ?? throw new PrimarSqlException("Scope is null");

            Analysis.RelationNames[node] = QualifiedName.From("(anonymous)");
            return CreateScope(node, context, queryScope.RelationFrame);
        }

        protected override Scope? VisitInlineTable(InlineTable node, Scope? context)
        {
            var fields = new List<Field>();

            foreach (var row in node.Expressions)
            {
                if (row is not RowConstructor rowConstructor)
                    throw new PrimarSyntaxException($"Inline table only can use RowConstructor not {row.GetType().Name}");

                if (fields.Count == 0)
                    fields.AddRange(rowConstructor.Expressions.Select((e, i) =>
                    {
                        // TODO: With Origin Table Name? (it is virtual table)
                        var field = Field.New(
                            $"implicit_col{i}",
                            QualifiedName.From("implicit_table"),
                            ColumnType.Unknown,
                            $"implicit_col{i}",
                            new ObjectName("implicit_table")
                        );

                        Analysis.Expressions[field] = e;

                        return field;
                    }));
            }

            var relation = new RelationFrame(fields.ToArray());

            return CreateScope(node, context, relation);
        }

        protected override Scope VisitAliasedRelation(AliasedRelation node, Scope? context)
        {
            Analysis.RelationNames[node] = QualifiedName.From(node.Alias.Value);

            var relationScope = node.Relation.Value is { } relation
                ? Visit(relation, context) ?? throw new PrimarSqlException("Scope is null")
                : new Scope(RelationFrame.Empty);

            var frame = relationScope.RelationFrame;
            var source = QualifiedName.From(node.Alias.Value);

            IEnumerable<Field> fields;

            if (node.ColumnAliases is { } aliases)
            {
                fields = frame.Fields.Select((f, i) =>
                {
                    string? alias = f.Name;

                    if (aliases.Length != 0)
                        alias = aliases[i].Value;

                    var newField = Field.New(alias, source, f.ColumnType, f.OriginName, f.OriginSource);

                    // TODO: Refactor Symbol system to can Resolve Expression only Fields
                    if (f.OriginName is null && f.OriginSource is null)
                    {
                        Analysis.Expressions[newField] = Analysis.Expressions[f];
                    }

                    return newField;
                });
            }
            else
            {
                fields = frame.Fields.Select(
                    f =>
                    {
                        var newField = Field.New(f.Name, source, f.ColumnType, f.OriginName, f.OriginSource);

                        // TODO: Refactor Symbol system to can Resolve Expression only Fields
                        if (f.OriginName is null && f.OriginSource is null)
                        {
                            Analysis.Expressions[newField] = Analysis.Expressions[f];
                        }

                        return newField;
                    }
                );
            }

            frame = new RelationFrame(fields.ToArray());
            return CreateScope(node, context, frame);
        }

        private Scope CreateScope(Node node, Scope? parent, RelationFrame frame)
        {
            var scope = new Scope(parent, frame);

            Analysis.Scopes[node] = scope;

            return scope;
        }
    }

    // private void AnalyzeQuery(Query query)
    // {
    //     if (query.QueryBody is { HasValue: false })
    //         throw new PrimarSqlException(ErrorCode.Syntax, "QueryBody is null.");
    //
    //     AnalyzeQueryBody(query.QueryBody.Value);
    // }
    //
    // public void AnalyzeRelation(Relation relation)
    // {
    //     switch (relation)
    //     {
    //         case QueryBody queryBody:
    //             AnalyzeQueryBody(queryBody);
    //             break;
    //
    //         case AliasedRelation aliasedRelation:
    //             // AnalyzeAliasedRelation(aliasedRelation);
    //             break;
    //
    //         case Join join:
    //             // AnalyzeJoin(join);
    //             break;
    //     }
    // }
    //
    // public void AnalyzeQueryBody(QueryBody queryBody)
    // {
    //     switch (queryBody)
    //     {
    //         case QuerySpecification querySpecification:
    //             AnalyzeQuerySpecification(querySpecification);
    //             break;
    //
    //         case Table table:
    //             AnalyzeTable(table);
    //             break;
    //     }
    // }
    //
    // public void AnalyzeQuerySpecification(QuerySpecification querySpecification)
    // {
    //     if (querySpecification.Select is { HasValue: false })
    //         throw new PrimarSqlException(ErrorCode.Syntax, "Select is null.");
    //
    //     var select = querySpecification.Select.Value;
    //
    //     foreach (var selectItem in select.SelectItems)
    //     {
    //     }
    //
    //     if (querySpecification.Relation.Value is { } relation)
    //     {
    //         AnalyzeRelation(relation);
    //     }
    // }
    //
    // public void AnalyzeTable(Table table)
    // {
    //     var name = QualifiedNameToObjectName(table.Name);
    //
    //     var tableMetadata = ResolveTable(name);
    //     ColumnInfo[] columns = ResolveColumns(name);
    // }
    //
    // private TableInfo ResolveTable(ObjectName name)
    // {
    //     name = _metadataProvider.ResolveObjectName(name);
    //
    //     if (!Analysis.Tables.TryGetValue(name, out var table))
    //     {
    //         table = _metadataProvider.GetTableMetadata(name);
    //
    //         if (table is not { })
    //             throw new UnableResolveSourceException(name);
    //
    //         Analysis.Tables[name] = table;
    //     }
    //
    //     return table;
    // }
    //
    // private ColumnInfo[] ResolveColumns(ObjectName name)
    // {
    //     name = _metadataProvider.ResolveObjectName(name);
    //
    //     if (!Analysis.Columns.TryGetValue(name, out ColumnInfo[]? columns))
    //     {
    //         columns = _metadataProvider.GetTableColumns(ResolveTable(name))?.ToArray();
    //
    //         if (columns is not { })
    //             throw new UnableResolveSourceException(name);
    //
    //         Analysis.Columns[name] = columns;
    //     }
    //
    //     return columns;
    // }
}

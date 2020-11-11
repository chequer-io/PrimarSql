using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    internal partial class PrimarSqlParser
    {
        internal partial class RootContext : IRootContext
        {
            public IEnumerable<INode> Children
            {
                get { yield return SqlStatements; }
            }

            public ISqlStatementsContext SqlStatements
                => sqlStatements();

            ITerminalNode IRootContext.MINUSMINUS => MINUSMINUS();
        }

        internal partial class SqlStatementsContext : ISqlStatementsContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    foreach (var sqlStatement in SqlStatements)
                        yield return sqlStatement;

                    foreach (var emptyStatement in EmptyStatements)
                        yield return emptyStatement;
                }
            }

            public IEnumerable<ISqlStatementContext> SqlStatements => sqlStatement();

            public IEnumerable<IEmptyStatementContext> EmptyStatements => emptyStatement();

            IEnumerable<ITerminalNode> ISqlStatementsContext.MINUSMINUS => MINUSMINUS();

            IEnumerable<ITerminalNode> ISqlStatementsContext.SEMI => SEMI();
        }

        internal partial class SqlStatementContext : ISqlStatementContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    if (DdlStatement != null)
                        yield return DdlStatement;

                    if (DmlStatement != null)
                        yield return DmlStatement;

                    if (ShowStatement != null)
                        yield return ShowStatement;
                }
            }

            public IDdlStatementContext DdlStatement => ddlStatement();

            public IDmlStatementContext DmlStatement => dmlStatement();

            public IShowStatementContext ShowStatement => showStatement();
        }

        internal partial class EmptyStatementContext : IEmptyStatementContext
        {
            public IEnumerable<INode> Children => Enumerable.Empty<INode>();
        }

        internal partial class DdlStatementContext : IDdlStatementContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    if (CreateTable != null)
                        yield return CreateTable;

                    if (AlterTable != null)
                        yield return AlterTable;

                    if (DropIndex != null)
                        yield return DropIndex;

                    if (DropTable != null)
                        yield return DropTable;

                    if (TruncateTable != null)
                        yield return TruncateTable;
                }
            }

            public ICreateTableContext CreateTable => createTable();

            public IAlterTableContext AlterTable => alterTable();

            public IDropIndexContext DropIndex => dropIndex();

            public IDropTableContext DropTable => dropTable();

            public ITruncateTableContext TruncateTable => truncateTable();
        }

        internal partial class DmlStatementContext : IDmlStatementContext
        {
        }

        internal partial class ShowStatementContext : IShowStatementContext
        {
        }

        #region DDL Statement
        internal partial class CreateTableContext : ICreateTableContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    yield return ifNotExists();
                    yield return TableName;
                    yield return CreateDefinitions;

                    foreach (var tableOption in TableOptions)
                    {
                        yield return tableOption;
                    }
                }
            }

            IIfNotExistsContext ICreateTableContext.IfNotExists => ifNotExists();

            public ITableNameContext TableName => tableName();

            public ICreateDefinitionsContext CreateDefinitions => createDefinitions();

            public IEnumerable<ITableOptionContext> TableOptions => tableOption();
        }

        internal partial class CreateDefinitionsContext : ICreateDefinitionsContext
        {
            public IEnumerable<INode> Children => CreateDefinitions;

            public IEnumerable<ICreateDefinitionContext> CreateDefinitions => createDefinition();
        }

        internal partial class CreateDefinitionContext : ICreateDefinitionContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    yield return Uid;
                    yield return ColumnDefinition;
                }
            }

            public IUidContext Uid => uid();

            public IColumnDefinitionContext ColumnDefinition => columnDefinition();
        }

        internal partial class ColumnDefinitionContext : IColumnDefinitionContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    yield return DataType;

                    foreach (var columnConstraint in ColumnConstraints)
                        yield return columnConstraint;
                }
            }

            public IDataTypeContext DataType => dataType();

            public IEnumerable<IColumnConstraintContext> ColumnConstraints => columnConstraint();
        }

        internal partial class DataTypeContext : IDataTypeContext
        {
            public IEnumerable<INode> Children => Enumerable.Empty<INode>();

            public IToken TypeName => typeName;
        }

        internal partial class AlterSpecificationContext : IAlterSpecificationContext
        {
            public IEnumerable<INode> Children => TableOption;

            public IEnumerable<ITableOptionContext> TableOption => tableOption();
        }

        internal partial class DropIndexContext : IDropIndexContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    yield return Uid;
                    yield return TableName;
                }
            }

            public IUidContext Uid => uid();

            public ITableNameContext TableName => tableName();
        }

        internal partial class DropTableContext : IDropTableContext
        {
            public IEnumerable<INode> Children
            {
                get { yield return TableName; }
            }

            public ITableNameContext TableName => tableName();
        }

        internal partial class TruncateTableContext : ITruncateTableContext
        {
            public IEnumerable<INode> Children
            {
                get { yield return TableName; }
            }

            public ITableNameContext TableName => tableName();
        }
        #endregion

        internal partial class IfNotExistsContext : IIfNotExistsContext
        {
            public IEnumerable<INode> Children => Enumerable.Empty<INode>();
        }

        internal partial class ColumnConstraintContext : IColumnConstraintContext
        {
            public IEnumerable<INode> Children => Enumerable.Empty<INode>();
        }

        internal partial class TableOptionContext : ITableOptionContext
        {
            public IEnumerable<INode> Children => Enumerable.Empty<INode>();
        }

        internal partial class TableOptionThroughputContext : ITableOptionThroughputContext
        {
            public int ReadCapacity => int.TryParse(readCapacity.GetText(), out int i) ? i : -1;

            public int WriteCapacity => int.TryParse(writeCapacity.GetText(), out int i) ? i : -1;
        }

        #region DB Objects
        internal partial class UidContext : IUidContext
        {
            public IEnumerable<INode> Children
            {
                get { yield return SimpleId; }
            }

            public ISimpleIdContext SimpleId => simpleId();

            public string Text => GetText();
        }

        internal partial class TableNameContext : ITableNameContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    yield return FullId;
                }
            }

            public IFullIdContext FullId => fullId();
        }

        internal partial class AlterTableContext : IAlterTableContext
        {
            public IEnumerable<INode> Children
            {
                get
                {
                    yield return TableName;

                    foreach (var alterSpecification in AlterSpecifications)
                        yield return alterSpecification;
                }
            }

            public ITableNameContext TableName => tableName();

            public IEnumerable<IAlterSpecificationContext> AlterSpecifications => alterSpecification();
        }

        internal partial class SimpleIdContext : ISimpleIdContext
        {
            public IEnumerable<INode> Children => Enumerable.Empty<INode>();

            ITerminalNode ISimpleIdContext.ID => ID();
        }

        internal partial class FullIdContext : IFullIdContext
        {
            public IEnumerable<INode> Children { get; }
            
            public IUidContext Uid => uid();

            public IDottedIdContext DottedId => dottedId();
        }

        internal partial class DottedIdContext : IDottedIdContext
        {
            public IEnumerable<INode> Children { get; }
        }
        
        #endregion
    }
}

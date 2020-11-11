using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    internal partial class PrimarSqlParser
    {
        internal partial class RootContext : IRootNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    yield return SqlStatements;
                    yield return MINUSMINUS();
                }
            }

            public ISqlStatementsNode SqlStatements
                => sqlStatements();

            ITerminalNode IRootNode.MINUSMINUS => MINUSMINUS();
        }

        internal partial class SqlStatementsContext : ISqlStatementsNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    foreach (var sqlStatement in SqlStatements)
                        yield return sqlStatement;

                    foreach (var emptyStatement in EmptyStatements)
                        yield return emptyStatement;

                    foreach (var m in MINUSMINUS())
                        yield return m;

                    foreach (var s in SEMI())
                        yield return s;
                }
            }

            public IEnumerable<ISqlStatementNode> SqlStatements => sqlStatement();

            public IEnumerable<IEmptyStatementNode> EmptyStatements => emptyStatement();

            IEnumerable<ITerminalNode> ISqlStatementsNode.MINUSMINUS => MINUSMINUS();

            IEnumerable<ITerminalNode> ISqlStatementsNode.SEMI => SEMI();
        }

        internal partial class SqlStatementContext : ISqlStatementNode
        {
            public IEnumerable<ITree> Children
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

            public IDdlStatementNode DdlStatement => ddlStatement();

            public IDmlStatementNode DmlStatement => null; // dmlStatement();

            public IShowStatementNode ShowStatement => null; // showStatement();
        }

        internal partial class EmptyStatementContext : IEmptyStatementNode
        {
            public IEnumerable<ITree> Children
            {
                get { yield return SEMI(); }
            }

            ITerminalNode IEmptyStatementNode.SEMI => SEMI();
        }

        internal partial class DdlStatementContext : IDdlStatementNode
        {
            public IEnumerable<ITree> Children
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

            public ICreateTableNode CreateTable => createTable();

            public IAlterTableNode AlterTable => alterTable();

            public IDropIndexNode DropIndex => dropIndex();

            public IDropTableNode DropTable => dropTable();

            public ITruncateTableNode TruncateTable => truncateTable();
        }
        //
        // internal partial class DmlStatementContext : IDmlStatementNode
        // {
        // }
        //
        // internal partial class ShowStatementContext : IShowStatementNode
        // {
        // }

        #region DDL Statement
        internal partial class CreateTableContext : ICreateTableNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (IfNotExists != null)
                        yield return IfNotExists;

                    if (TableName != null)
                        yield return TableName;

                    if (CreateDefinitions != null)
                        yield return CreateDefinitions;

                    foreach (var tableOption in TableOptions)
                        yield return tableOption;
                }
            }

            public IIfNotExistsNode IfNotExists => ifNotExists();

            public ITableNameNode TableName => tableName();

            public ICreateDefinitionsNode CreateDefinitions => createDefinitions();

            public IEnumerable<ITableOptionNode> TableOptions => tableOption();
        }

        internal partial class CreateDefinitionsContext : ICreateDefinitionsNode
        {
            public IEnumerable<ITree> Children => CreateDefinitions;

            public IEnumerable<ICreateDefinitionNode> CreateDefinitions => createDefinition();
        }

        internal partial class CreateDefinitionContext : ICreateDefinitionNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    yield return Uid;
                    yield return ColumnDefinition;
                }
            }

            public IUidNode Uid => uid();

            public IColumnDefinitionNode ColumnDefinition => columnDefinition();
        }

        internal partial class ColumnDefinitionContext : IColumnDefinitionNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (DataType != null)
                        yield return DataType;

                    foreach (var columnConstraint in ColumnConstraints)
                        yield return columnConstraint;
                }
            }

            public IDataTypeNode DataType => dataType();

            public IEnumerable<IColumnConstraintNode> ColumnConstraints => columnConstraint();
        }

        internal partial class DataTypeContext : IDataTypeNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (Varchar != null) yield return Varchar;
                    if (Text != null) yield return Text;
                    if (Mediumtext != null) yield return Mediumtext;
                    if (Longtext != null) yield return Longtext;
                    if (String != null) yield return String;
                    if (Int != null) yield return Int;
                    if (Integer != null) yield return Integer;
                    if (Bigint != null) yield return Bigint;
                    if (Bool != null) yield return Bool;
                    if (Boolean != null) yield return Boolean;
                    if (List != null) yield return List;
                    if (Binary != null) yield return Binary;
                    if (NumberList != null) yield return NumberList;
                    if (StringList != null) yield return StringList;
                }
            }

            public ITerminalNode Varchar => VARCHAR();

            public ITerminalNode Text => TEXT();

            public ITerminalNode Mediumtext => MEDIUMTEXT();

            public ITerminalNode Longtext => LONGTEXT();

            public ITerminalNode String => STRING();

            public ITerminalNode Int => INT();

            public ITerminalNode Integer => INTEGER();

            public ITerminalNode Bigint => BIGINT();

            public ITerminalNode Bool => BOOL();

            public ITerminalNode Boolean => BOOLEAN();

            public ITerminalNode List => LIST();

            public ITerminalNode Binary => BINARY();

            public ITerminalNode NumberList => NUMBER_LIST();

            public ITerminalNode StringList => STRING_LIST();
        }

        internal partial class AlterSpecificationContext : IAlterSpecificationNode
        {
            public IEnumerable<ITree> Children => TableOption;

            public IEnumerable<ITableOptionNode> TableOption => tableOption();
        }

        internal partial class DropIndexContext : IDropIndexNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (Uid != null)
                        yield return Uid;

                    if (TableName != null)
                        yield return TableName;
                }
            }

            public IUidNode Uid => uid();

            public ITableNameNode TableName => tableName();
        }

        internal partial class DropTableContext : IDropTableNode
        {
            public IEnumerable<ITree> Children
            {
                get { yield return TableName; }
            }

            public ITableNameNode TableName => tableName();
        }

        internal partial class TruncateTableContext : ITruncateTableNode
        {
            public IEnumerable<ITree> Children
            {
                get { yield return TableName; }
            }

            public ITableNameNode TableName => tableName();
        }
        #endregion

        internal partial class IfNotExistsContext : IIfNotExistsNode
        {
            public IEnumerable<ITree> Children => Enumerable.Empty<INode>();
        }

        internal partial class ColumnConstraintContext : IColumnConstraintNode
        {
            public IEnumerable<ITree> Children => Enumerable.Empty<INode>();
        }

        internal partial class TableOptionContext : ITableOptionNode
        {
            public virtual IEnumerable<ITree> Children => Enumerable.Empty<INode>();
        }

        internal partial class TableOptionThroughputContext : ITableOptionThroughputNode
        {
            public override IEnumerable<ITree> Children
            {
                get
                {
                    yield return ReadCapacity;
                    yield return WriteCapacity;
                }
            }

            public IDecimalLiteralNode ReadCapacity => readCapacity;

            public IDecimalLiteralNode WriteCapacity => writeCapacity;
        }

        internal partial class TableBillingModeContext : ITableBillingModeNode
        {
            public override IEnumerable<ITree> Children
            {
                get
                {
                    if (Provisioned != null)
                        yield return Provisioned;
                    
                    if (PayPerRequest != null)
                        yield return PayPerRequest;
                }
            }

            public ITerminalNode Provisioned => PROVISIONED();

            public ITerminalNode PayPerRequest => PAY_PER_REQUEST();

            public IToken BillingMode => billingMode;
        }
        
        internal partial class UidContext : IUidNode
        {
            public IEnumerable<ITree> Children
            {
                get { yield return SimpleId; }
            }

            public ISimpleIdNode SimpleId => simpleId();
        }

        internal partial class TableNameContext : ITableNameNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (Uid != null)
                        yield return Uid;
                }
            }

            public IUidNode Uid => uid();
        }

        internal partial class AlterTableContext : IAlterTableNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (TableName != null)
                        yield return TableName;

                    foreach (var alterSpecification in AlterSpecifications)
                        yield return alterSpecification;
                }
            }

            public ITableNameNode TableName => tableName();

            public IEnumerable<IAlterSpecificationNode> AlterSpecifications => alterSpecification();
        }

        internal partial class SimpleIdContext : ISimpleIdNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (Id != null)
                        yield return Id;

                    if (KeywordsCanBeId != null)
                        yield return KeywordsCanBeId;
                }
            }

            public ITerminalNode Id => ID();

            public IKeywordCanBeIdNode KeywordsCanBeId => keywordsCanBeId();
        }

        internal partial class DecimalLiteralContext : IDecimalLiteralNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (DecimalLiteral != null) yield return DecimalLiteral;
                    if (ZeroDecimal != null) yield return ZeroDecimal;
                    if (OneDecimal != null) yield return OneDecimal;
                    if (TwoDecimal != null) yield return TwoDecimal;
                }
            }

            public ITerminalNode DecimalLiteral => DECIMAL_LITERAL();

            public ITerminalNode ZeroDecimal => ZERO_DECIMAL();

            public ITerminalNode OneDecimal => ONE_DECIMAL();

            public ITerminalNode TwoDecimal => TWO_DECIMAL();
        }

        internal partial class KeywordsCanBeIdContext : IKeywordCanBeIdNode
        {
            public IEnumerable<ITree> Children
            {
                get
                {
                    if (Columns != null)
                        yield return Columns;

                    if (Fields != null)
                        yield return Fields;

                    if (Hash != null)
                        yield return Hash;

                    if (Indexes != null)
                        yield return Indexes;

                    if (List != null)
                        yield return List;

                    if (Serial != null)
                        yield return Serial;

                    if (String != null)
                        yield return String;

                    if (Truncate != null)
                        yield return Truncate;

                    if (Value != null)
                        yield return Value;

                    if (Text != null)
                        yield return Text;

                    if (Tables != null)
                        yield return Tables;

                    if (In != null)
                        yield return In;
                }
            }

            public ITerminalNode Columns => COLUMNS();

            public ITerminalNode Fields => FIELDS();

            public ITerminalNode Hash => HASH();

            public ITerminalNode Indexes => INDEXES();

            public ITerminalNode List => LIST();

            public ITerminalNode Serial => SERIAL();

            public ITerminalNode String => STRING();

            public ITerminalNode Truncate => TRUNCATE();

            public ITerminalNode Value => VALUE();

            public ITerminalNode Text => TEXT();

            public ITerminalNode Tables => TABLES();

            public ITerminalNode In => IN();
        }
    }
}

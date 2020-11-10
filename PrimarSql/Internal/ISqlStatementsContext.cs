using System.Collections.Generic;
using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface ISqlStatementsContext : INode
    {
        IEnumerable<ISqlStatementContext> SqlStatements { get; }
        
        IEnumerable<IEmptyStatementContext> EmptyStatements { get; }
        
        IEnumerable<ITerminalNode> MINUSMINUS { get; }
        
        IEnumerable<ITerminalNode> SEMI { get; }
    }
}

using System.Collections.Generic;
using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface ISqlStatementsNode : INode
    {
        IEnumerable<ISqlStatementNode> SqlStatements { get; }
        
        IEnumerable<IEmptyStatementNode> EmptyStatements { get; }
        
        IEnumerable<ITerminalNode> MINUSMINUS { get; }
        
        IEnumerable<ITerminalNode> SEMI { get; }
    }
}

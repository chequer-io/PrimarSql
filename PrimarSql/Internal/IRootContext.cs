using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface IRootContext : INode
    {
        public ISqlStatementsContext SqlStatements { get; }
        
        public ITerminalNode MINUSMINUS { get; }
        
        public int RuleIndex { get; }
    }
}

using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface IRootNode : INode
    {
        public ISqlStatementsNode SqlStatements { get; }
        
        public ITerminalNode MINUSMINUS { get; }
        
        public int RuleIndex { get; }
    }
}

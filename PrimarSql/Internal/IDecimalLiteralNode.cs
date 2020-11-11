using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface IDecimalLiteralNode : INode
    {
        ITerminalNode DecimalLiteral { get; }
        
        ITerminalNode ZeroDecimal { get; }
        
        ITerminalNode OneDecimal { get; }
        
        ITerminalNode TwoDecimal { get; }
    }
}

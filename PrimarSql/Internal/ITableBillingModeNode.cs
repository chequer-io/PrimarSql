using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface ITableBillingModeNode : INode
    {
        IToken BillingMode { get; }
        
        ITerminalNode Provisioned { get; }
        
        ITerminalNode PayPerRequest { get; }
    }
}

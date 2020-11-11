using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface IEmptyStatementNode : INode
    {
        ITerminalNode SEMI { get; }
    }
}

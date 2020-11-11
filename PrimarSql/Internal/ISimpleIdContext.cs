using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface ISimpleIdContext : INode
    {
        ITerminalNode ID { get; }
    }
}

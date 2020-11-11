using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface ISimpleIdNode : INode
    {
        ITerminalNode Id { get; }

        IKeywordCanBeIdNode KeywordsCanBeId { get; }
    }
}

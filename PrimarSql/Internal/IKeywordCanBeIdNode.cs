using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface IKeywordCanBeIdNode : INode
    {
        ITerminalNode Columns { get; }

        ITerminalNode Fields { get; }

        ITerminalNode Hash { get; }

        ITerminalNode Indexes { get; }

        ITerminalNode List { get; }

        ITerminalNode Serial { get; }

        ITerminalNode String { get; }

        ITerminalNode Truncate { get; }

        ITerminalNode Value { get; }

        ITerminalNode Text { get; }

        ITerminalNode Tables { get; }

        ITerminalNode In { get; }
    }
}

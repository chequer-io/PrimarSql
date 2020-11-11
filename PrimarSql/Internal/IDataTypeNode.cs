using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface IDataTypeNode : INode
    {
        ITerminalNode Varchar { get; }

        ITerminalNode Text { get; }

        ITerminalNode Mediumtext { get; }

        ITerminalNode Longtext { get; }

        ITerminalNode String { get; }

        ITerminalNode Int { get; }

        ITerminalNode Integer { get; }

        ITerminalNode Bigint { get; }

        ITerminalNode Bool { get; }

        ITerminalNode Boolean { get; }

        ITerminalNode List { get; }

        ITerminalNode Binary { get; }

        ITerminalNode NumberList { get; }

        ITerminalNode StringList { get; }
    }
}

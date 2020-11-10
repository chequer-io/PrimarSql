using Antlr4.Runtime;

namespace PrimarSql.Internal
{
    public interface IDataTypeContext : INode
    {
        IToken TypeName { get; }
    }
}

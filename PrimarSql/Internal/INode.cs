using System.Collections.Generic;
using Antlr4.Runtime.Tree;

namespace PrimarSql.Internal
{
    public interface INode : ITree
    {
        IEnumerable<ITree> Children { get; }
    }
}

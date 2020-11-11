using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface IAlterSpecificationNode : INode
    {
        IEnumerable<ITableOptionNode> TableOption { get; }
    }
}

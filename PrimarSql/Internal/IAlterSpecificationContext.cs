using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface IAlterSpecificationContext : INode
    {
        IEnumerable<ITableOptionContext> TableOption { get; }
    }
}

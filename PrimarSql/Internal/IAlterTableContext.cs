using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface IAlterTableContext : INode
    {
        IEnumerable<IAlterSpecificationContext> AlterSpecifications { get; }
    }
}

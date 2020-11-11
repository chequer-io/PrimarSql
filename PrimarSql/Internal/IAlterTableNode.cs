using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface IAlterTableNode : INode
    {
        IEnumerable<IAlterSpecificationNode> AlterSpecifications { get; }
    }
}

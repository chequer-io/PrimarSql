using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface IColumnDefinitionNode : INode
    {
        IDataTypeNode DataType { get; }
        
        IEnumerable<IColumnConstraintNode> ColumnConstraints { get; }
    }
}

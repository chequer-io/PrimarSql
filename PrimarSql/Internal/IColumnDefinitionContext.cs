using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface IColumnDefinitionContext : INode
    {
        IDataTypeContext DataType { get; }
        
        IEnumerable<IColumnConstraintContext> ColumnConstraints { get; }
    }
}

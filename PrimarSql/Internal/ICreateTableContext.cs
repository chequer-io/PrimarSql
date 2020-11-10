using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface ICreateTableContext : INode
    {
        IIfNotExistsContext IfNotExists { get; }
        
        ITableNameContext TableName { get; }
        
        ICreateDefinitionsContext CreateDefinitions { get; }
        
        IEnumerable<ITableOptionContext> TableOptions { get; }
    }
}

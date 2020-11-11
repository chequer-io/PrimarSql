using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface ICreateTableNode : INode
    {
        IIfNotExistsNode IfNotExists { get; }
        
        ITableNameNode TableName { get; }
        
        ICreateDefinitionsNode CreateDefinitions { get; }
        
        IEnumerable<ITableOptionNode> TableOptions { get; }
    }
}

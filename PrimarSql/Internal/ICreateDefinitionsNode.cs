using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface ICreateDefinitionsNode : INode
    {
        IEnumerable<ICreateDefinitionNode> CreateDefinitions { get; }
    }
}

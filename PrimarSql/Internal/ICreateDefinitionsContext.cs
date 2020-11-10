using System.Collections.Generic;

namespace PrimarSql.Internal
{
    public interface ICreateDefinitionsContext : INode
    {
        IEnumerable<ICreateDefinitionContext> CreateDefinitions { get; }
    }
}

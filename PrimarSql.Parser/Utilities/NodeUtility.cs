using System.Collections.Generic;
using System.Linq;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Parser.Utilities;

public static class NodeUtility
{
    public static IEnumerable<Node> YieldNodes(params INodeValue[] properties)
    {
        return from property in properties
               where property.HasValue
               select property.Get();
    }
}

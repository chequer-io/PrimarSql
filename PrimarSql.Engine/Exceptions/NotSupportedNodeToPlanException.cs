using PrimarSql.Common;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine;

public class NotSupportedNodeToPlanException : PrimarSqlException
{
    public NotSupportedNodeToPlanException(Node node)
        : base(ErrorCode.NotSupportedFeature, $"Not supported node '{node.GetType().Name}' to plan.")
    {
    }
}

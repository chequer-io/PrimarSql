using PrimarSql.Common;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine;

public class NotSupportedNodeToAnalyzeException : PrimarSqlException
{
    public NotSupportedNodeToAnalyzeException(Node node)
        : base(ErrorCode.NotSupportedFeature, $"Not supported node '{node.GetType().Name}' to analyze.")
    {
    }
}

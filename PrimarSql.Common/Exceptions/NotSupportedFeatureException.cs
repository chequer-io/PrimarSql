namespace PrimarSql.Common;

public class NotSupportedFeatureException : PrimarSqlException
{
    public NotSupportedFeatureException(string featureName)
        : base(ErrorCode.NotSupportedFeature, $"Feature '{featureName}' not supported.")
    {
    }
}

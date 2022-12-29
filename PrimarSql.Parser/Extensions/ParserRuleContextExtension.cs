using static PrimarSql.Parser.Internal.PrimarSqlParser;

namespace PrimarSql.Parser;

internal static class ParserRuleContextExtension
{
    public static bool IsDistinct(this SetQuantifierContext? context)
    {
        return context?.DISTINCT() != null;
    }
}

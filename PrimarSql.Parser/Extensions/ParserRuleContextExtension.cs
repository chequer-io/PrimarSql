using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace PrimarSql.Parser;

internal static class ParserRuleContextExtension
{
    public static bool HasToken(this ParserRuleContext? context, int type)
    {
        if (context is not { })
            return false;

        return context.children
            .OfType<ITerminalNode>()
            .Any(n => n.Symbol.Type == type);
    }
}

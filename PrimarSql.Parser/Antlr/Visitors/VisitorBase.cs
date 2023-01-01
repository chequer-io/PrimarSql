using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using PrimarSql.Parser.Internal;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Parser.Antlr.Visitors;

internal partial class PrimarSqlVisitor
{
    public override Node VisitSingleStatement(PrimarSqlParser.SingleStatementContext context)
    {
        return Visit<Statement>(context.statement());
    }

    private T? VisitIfNotNull<T>(ParserRuleContext? context) where T : Node
    {
        return context is null
            ? null
            : Visit<T>(context);
    }

    private T Visit<T>(ParserRuleContext context) where T : Node
    {
        if (Visit(context) is { } visitResult && visitResult.To<T>() is { } result)
            return result;

        throw new ParserInternalException("Visit(ParserRuleContext) returns null.");
    }

    private IEnumerable<T> Visit<T>(IEnumerable<ParserRuleContext>? contexts) where T : Node
    {
        return contexts?.Select(Visit<T>).ToArray() ?? Enumerable.Empty<T>();
    }
}

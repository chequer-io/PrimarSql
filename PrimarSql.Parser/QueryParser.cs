using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using PrimarSql.Common;
using PrimarSql.Parser.Antlr;
using PrimarSql.Parser.Antlr.Visitors;
using PrimarSql.Parser.Internal;
using PrimarSql.Parser.Nodes;
using static PrimarSql.Parser.Internal.PrimarSqlParser;

namespace PrimarSql.Parser;

public static class QueryParser
{
    internal static SingleStatementContext SingleStatement(string query)
    {
        var lexer = new PrimarSqlLexer(new CaseInsensitiveStream(new AntlrInputStream(query)));
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new PrimarSqlParser(tokenStream);

        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(new LexerErrorListener());

        parser.RemoveErrorListeners();
        parser.AddErrorListener(new ParserErrorHandler());

        SingleStatementContext result;

        try
        {
            parser.Interpreter.PredictionMode = PredictionMode.SLL;
            result = parser.singleStatement();
        }
        catch (ParseCanceledException)
        {
            tokenStream.Seek(0);
            parser.Reset();

            parser.Interpreter.PredictionMode = PredictionMode.LL;
            result = parser.singleStatement();
        }

        return result;
    }

    public static Statement Parse(string query)
    {
        var result = SingleStatement(query);
        var visitor = new PrimarSqlVisitor();
        var visitNode = visitor.Visit(result);

        if (visitNode is not Statement statement)
            throw new PrimarSqlException(
                $"Parse result is not statement. (Antlr: {result.GetType().Name}, Node: {visitNode.GetType().Name})"
            );

        return statement;
    }
}

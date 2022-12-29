using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using PrimarSql.Parser.Antlr;
using PrimarSql.Parser.Internal;
using PrimarSql.Parser.Tree;
using static PrimarSql.Parser.Internal.PrimarSqlParser;

namespace PrimarSql.Parser.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        string sql = "SELECT 1";

        var lexer = new PrimarSqlLexer(new CaseInsensitiveStream(new AntlrInputStream(sql)));
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new PrimarSqlParser(tokenStream);
        var visitor = new PrimarSqlVisitor();

        SingleStatementContext result;

        try
        {
            // first, try parsing with potentially faster SLL mode
            parser.Interpreter.PredictionMode = PredictionMode.SLL;
            result = parser.singleStatement();
        }
        catch (ParseCanceledException)
        {
            // if we fail, parse with LL mode
            tokenStream.Seek(0); // rewind input stream
            parser.Reset();

            parser.Interpreter.PredictionMode = PredictionMode.LL;
            result = parser.singleStatement();
        }

        var visitResult = visitor.Visit(((QueryPrimaryDefaultContext)(((QueryTermDefaultContext)((StatementDefaultContext)result.statement()).queryNoWith().queryTerm())).queryPrimary()).querySpecification()).To<QuerySpecification>();

        Assert.Pass();
    }
}

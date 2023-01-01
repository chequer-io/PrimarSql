using NUnit.Framework;
using PrimarSql.Common.Data;
using PrimarSql.Csv;
using PrimarSql.Engine;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.Planners;
using PrimarSql.Parser;

namespace PrimarSql.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        const string sql = "SELECT * FROM test";
        var statementNode = QueryParser.Parse(sql);

        var service = new CsvService("/Users/user/Documents/primarsql_database");
        var session = new Session(service);
        var analyzer = new StatementAnalyzer(session.MetadataProvider);
        var result = analyzer.Analyze(statementNode);

        var planner = new ExecutePlanner(result);
        var planNode = planner.Plan(statementNode);

        var provider = planNode.Execute(session);
        var reader = new DataPageReader(provider);

        Assert.Pass();
    }
}

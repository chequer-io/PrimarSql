using PrimarSql.Common;
using PrimarSql.Common.Data;
using PrimarSql.Common.Providers;
using PrimarSql.Engine.Analyzers;
using PrimarSql.Engine.Planners;
using PrimarSql.Parser;

namespace PrimarSql.Engine;

public class Session
{
    public IMetadataProvider MetadataProvider => _metadataProvider.Value;

    public ISourceProvider SourceProvider => _pageProvider.Value;

    public string Name => _service.VendorName;

    private readonly VendorService _service;
    private readonly Lazy<IMetadataProvider> _metadataProvider;
    private readonly Lazy<ISourceProvider> _pageProvider;

    public Session(VendorService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _metadataProvider = new Lazy<IMetadataProvider>(() => _service.GetMetadataProvider());
        _pageProvider = new Lazy<ISourceProvider>(() => _service.GetSourceProvider());
    }

    public ExecuteResult Execute(string sql)
    {
        var statementNode = QueryParser.Parse(sql);
        var analyzer = new StatementAnalyzer(MetadataProvider);
        var result = analyzer.Analyze(statementNode);

        var planner = new ExecutePlanner(result);
        var planNode = planner.Plan(statementNode);

        var provider = planNode.Execute(this);
        var reader = new DataPageReader(provider);

        return new ExecuteResult(reader);
    }
}

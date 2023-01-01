using PrimarSql.Common;
using PrimarSql.Common.Providers;

namespace PrimarSql.Csv;

public class CsvService : VendorService
{
    private readonly string _basePath;

    public override string VendorName => "csv";

    public CsvService(string basePath)
    {
        _basePath = basePath;
    }

    public override IMetadataProvider GetMetadataProvider()
    {
        return new CsvMetadataProvider(_basePath);
    }

    public override ISourceProvider GetSourceProvider()
    {
        return new CsvSourceProvider(_basePath);
    }
}

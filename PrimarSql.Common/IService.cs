using PrimarSql.Common.Providers;

namespace PrimarSql.Common;

public interface IService
{
    string VendorName { get; }

    IMetadataProvider GetMetadataProvider();

    ISourceProvider GetSourceProvider();
}

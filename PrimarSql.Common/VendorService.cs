using PrimarSql.Common.Providers;

namespace PrimarSql.Common;

public abstract class VendorService : IService
{
    public abstract string VendorName { get; }

    public abstract IMetadataProvider GetMetadataProvider();

    public virtual ISourceProvider GetSourceProvider()
    {
        throw new PrimarSqlException(ErrorCode.NotSupportedFeature, $"Vendor '{VendorName}' not supported PageProvider.");
    }
}

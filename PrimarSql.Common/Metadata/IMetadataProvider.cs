using PrimarSql.Common.Sources;

namespace PrimarSql.Common.Metadata;

public interface IMetadataProvider
{
    ISource ResolveMetadata(ObjectName name);

    TableSource GetTableMetadata(ObjectName name);

    // ViewSource GetViewMetadata();
}

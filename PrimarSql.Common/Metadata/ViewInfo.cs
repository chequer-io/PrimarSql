using PrimarSql.Common.Utilities;

namespace PrimarSql.Common.Metadata;

public class ViewInfo : IMetadata
{
    public ObjectName Name { get; }

    public long EstimatedCount { get; }

    public ViewInfo(ObjectName name, long estimatedCount)
    {
        Name = name;
        EstimatedCount = estimatedCount;
    }

    public class Builder
    {
        private ObjectName? _name;
        private long? _estimatedCount;

        public Builder SetName(ObjectName name)
        {
            _name = name;
            return this;
        }

        public Builder SetEstimatedCount(long count)
        {
            // -1 means don't know estimatedCount
            if (count < 0)
                count = -1;

            _estimatedCount = count;
            return this;
        }

        public ViewInfo Build()
        {
            return new ViewInfo(_name.NotNull("name"), _estimatedCount.NotNull("estimatedCount"));
        }
    }
}

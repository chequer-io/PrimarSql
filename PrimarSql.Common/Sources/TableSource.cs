using static PrimarSql.Common.Utilities.VerifyUtility;

namespace PrimarSql.Common.Sources;

public class TableSource : ISource
{
    public ObjectName Name { get; }

    public long EstimatedCount { get; }

    private TableSource(ObjectName name, long estimatedCount)
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

        public TableSource Create()
        {
            return new TableSource(
                VerifyNotNull("name", _name),
                VerifyNotNull("estimatedCount", _estimatedCount)
            );
        }
    }
}

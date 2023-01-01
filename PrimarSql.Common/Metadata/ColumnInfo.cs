using PrimarSql.Common.Utilities;

namespace PrimarSql.Common.Metadata;

public class ColumnInfo
{
    public string ColumnName { get; }

    public ColumnType Type { get; }

    // TODO:
    public ColumnInfo(string columnName, ColumnType type)
    {
        ColumnName = columnName;
        Type = type;
    }

    public class Builder
    {
        private string? _name;
        private ColumnType? _columnType;

        public Builder SetName(string name)
        {
            _name = name;
            return this;
        }

        public Builder SetColumnType(ColumnType columnType)
        {
            _columnType = columnType;
            return this;
        }

        public ColumnInfo Build()
        {
            return new ColumnInfo(_name.NotNull("name"), _columnType.NotNull("columnType"));
        }
    }
}

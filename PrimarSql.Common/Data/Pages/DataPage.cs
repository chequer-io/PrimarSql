using PrimarSql.Common.Utilities;

namespace PrimarSql.Common.Data;

public class DataPage : IDataPage
{
    public string Name { get; }

    public DataColumn[] Columns { get; }

    public int DataCount { get; }

    public int ColumnCount => Columns.Length;

    public DataPage(string name, DataColumn[] columns, int dataCount)
    {
        Name = name;
        Columns = columns;
        DataCount = dataCount;
    }

    public DataColumn GetColumn(int columnIndex)
    {
        return Columns[columnIndex];
    }

    public object?[] Get(int index)
    {
        return Columns.Select(c => c.Get(index)).ToArray();
    }

    public IDataPage Skip(int count)
    {
        if (count < 0)
            throw new PrimarSqlException("Cannot skip negative times.");

        if (count == 0)
            return this;

        var builder = Builder.From(this)
            .SetDataCount(Math.Max(DataCount - count, 0))
            .ClearColumns();

        foreach (var column in Columns)
        {
            builder.AddColumn(column.Skip(count));
        }

        return builder.Build();
    }

    public IDataPage Take(int count)
    {
        if (count <= -1)
            return this;

        var actualCount = Math.Min(count, DataCount);

        var builder = Builder.From(this)
            .SetDataCount(actualCount)
            .ClearColumns();

        foreach (var column in Columns)
            builder.AddColumn(column.Take(actualCount));

        return builder.Build();
    }

    public class Builder
    {
        public IReadOnlyList<DataColumn> Outputs => _outputs.AsReadOnly();

        public string? Name { get; private set; }

        public int? DataCount { get; private set; }

        private readonly List<DataColumn> _outputs = new();

        public static Builder From(DataPage page)
        {
            var builder = new Builder
            {
                Name = page.Name,
                DataCount = page.DataCount
            };

            builder._outputs.AddRange(page.Columns);

            return builder;
        }

        public Builder ClearColumns()
        {
            _outputs.Clear();
            return this;
        }

        public Builder AddColumn(DataColumn dataColumn)
        {
            _outputs.Add(dataColumn);
            return this;
        }

        public Builder AddColumns(IEnumerable<DataColumn> dataColumns)
        {
            _outputs.AddRange(dataColumns);
            return this;
        }

        public Builder SetName(string name)
        {
            Name = name;
            return this;
        }

        public Builder SetDataCount(int dataCount)
        {
            DataCount = dataCount;
            return this;
        }

        public DataPage Build()
        {
            return new DataPage(
                Name.NotNull(nameof(Name)),
                _outputs.ToArray(),
                DataCount.NotNull(nameof(DataCount))
            );
        }
    }
}

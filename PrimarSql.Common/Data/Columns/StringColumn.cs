using PrimarSql.Common.Utilities;

namespace PrimarSql.Common.Data;

public class StringColumn : DataColumn
{
    private readonly string[] _data;

    public override string Name { get; }

    public StringColumn(string name, string[] data)
    {
        _data = data;
        Name = name;
    }

    public override object Get(int index)
    {
        return _data[index];
    }

    public override DataColumn Skip(int count)
    {
        return new StringColumn(Name, _data.Skip(count).ToArray());
    }

    public override DataColumn Take(int count)
    {
        return new StringColumn(Name, _data.Take(count).ToArray());
    }

    // TODO: Builder to Interface
    public class Builder
    {
        private readonly List<string> _buffer = new();
        private string? _name;

        public int ItemCount => _buffer.Count;

        public Builder SetName(string name)
        {
            _name = name;
            return this;
        }

        public Builder Add(string item)
        {
            _buffer.Add(item);
            return this;
        }

        public Builder Clear()
        {
            _buffer.Clear();
            return this;
        }

        public StringColumn Build()
        {
            return new StringColumn(_name.NotNull("name"), _buffer.ToArray());
        }
    }
}

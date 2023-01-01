namespace PrimarSql.Common.Data;

public class ObjectColumn : DataColumn
{
    private readonly object?[] _data;

    public override string Name { get; }

    public ObjectColumn(string name, object?[] data)
    {
        _data = data;
        Name = name;
    }

    public override object? Get(int index)
    {
        return _data[index];
    }

    public override DataColumn Skip(int count)
    {
        return new ObjectColumn(Name, _data.Skip(count).ToArray());
    }

    public override DataColumn Take(int count)
    {
        return new ObjectColumn(Name, _data.Take(count).ToArray());
    }
}

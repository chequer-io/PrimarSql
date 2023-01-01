namespace PrimarSql.Common.Data;

public abstract class DataColumn
{
    public abstract string Name { get; }

    public abstract object? Get(int index);

    public abstract DataColumn Skip(int count);

    public abstract DataColumn Take(int count);
}

using System.Text;

namespace PrimarSql.Common;

public class ObjectName
{
    public ObjectName(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public ObjectName(string schema, string name)
    {
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public ObjectName(string database, string schema, string name)
    {
        Database = database ?? throw new ArgumentNullException(nameof(database));
        Schema = schema ?? throw new ArgumentNullException(nameof(schema));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string? Database { get; }

    public string? Schema { get; }

    public string Name { get; }

    public static bool operator ==(ObjectName? a, ObjectName? b)
    {
        bool aNull = ReferenceEquals(null, a);
        bool bNull = ReferenceEquals(null, b);

        if (aNull && bNull)
            return true;

        if (aNull && !bNull)
            return false;

        if (!aNull && bNull)
            return false;

        // ReSharper disable NullableWarningSuppressionIsUsed
        return a!.Equals(b!);
        // ReSharper restore NullableWarningSuppressionIsUsed
    }

    public static bool operator !=(ObjectName? a, ObjectName? b)
    {
        return !(a == b);
    }

    protected bool Equals(ObjectName other)
    {
        return Database == other.Database && Schema == other.Schema && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((ObjectName)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Database, Schema, Name);
    }

    public IEnumerable<string> GetStrings()
    {
        if (Database is not null)
            yield return Database;

        if (Schema is not null)
            yield return Schema;

        yield return Name;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (Database is not null)
        {
            sb.Append(Database);
            sb.Append('.');
        }

        if (Schema is not null)
        {
            sb.Append(Schema);
            sb.Append('.');
        }

        sb.Append(Name);

        return sb.ToString();
    }
}

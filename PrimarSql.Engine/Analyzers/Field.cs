using PrimarSql.Common;
using PrimarSql.Common.Metadata;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Analyzers;

public class Field
{
    public string? Name { get; }

    public QualifiedName? Source { get; }

    public string? OriginName { get; }

    public ObjectName? OriginSource { get; }

    public ColumnType ColumnType { get; }

    public bool IsAliased => OriginName is not null;

    private Field(string? name, QualifiedName? source, ColumnType columnType, string? originName, ObjectName? originSource)
    {
        OriginName = originName;
        OriginSource = originSource;
        ColumnType = columnType;
        Name = name;
        Source = source;
    }

    public bool CanResolve(QualifiedName name)
    {
        if (Name is null)
            return false;

        return MatchSource(name.Identifiers[..^1].ToArray()) && Name == name.Identifiers[^1].Unescape();
    }

    public bool MatchSource(Identifier[] name)
    {
        if (Source is null)
            return true;

        switch (name.Length)
        {
            case 0:
                return true;

            case > 3:
                return false;
        }

        var input = Deconsruct(name);
        var source = Deconsruct(Source.Identifiers);

        if (input.database is not null && input.database != source.database)
            return false;

        if (input.schema is not null && input.schema != source.schema)
            return false;

        return input.table == source.table;
    }

    private (string? database, string? schema, string table) Deconsruct(Identifier[] identifiers)
    {
        string[] data = identifiers.Select(i => i.Unescape()).ToArray();

        return identifiers.Length switch
        {
            1 => (null, null, data[0]),
            2 => (null, data[0], data[1]),
            3 => (data[0], data[1], data[2]),
            _ => throw new InvalidOperationException($"Deconstruct data length must be 1 to 3. not {identifiers.Length}.")
        };
    }

    public static Field New(string? name, QualifiedName? source, ColumnType columnType)
    {
        return new Field(name, source, columnType, null, null);
    }

    public static Field New(string? name, QualifiedName? source, ColumnType columnType, string? originName, ObjectName? originSource)
    {
        return new Field(name, source, columnType, originName, originSource);
    }
}

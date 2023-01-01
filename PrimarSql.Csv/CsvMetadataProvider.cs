using System.Globalization;
using CsvHelper;
using PrimarSql.Common;
using PrimarSql.Common.Metadata;
using PrimarSql.Common.Providers;
using PrimarSql.Csv.Exceptions;

namespace PrimarSql.Csv;

public class CsvMetadataProvider : VendorMetadataProvider
{
    private readonly string _basePath;
    private string _schema;

    public CsvMetadataProvider(string basePath)
    {
        _schema = "default";
        _basePath = basePath;
    }

    public override TableInfo? GetTableMetadata(ObjectName name)
    {
        if (GetPath(name) is not { } path)
            return null;

        if (!File.Exists(path))
            return null;

        return new TableInfo(this, name, -1);
    }

    public override IEnumerable<ColumnInfo>? GetTableColumns(ObjectName tableName)
    {
        if (GetPath(tableName) is not { } path)
            return null;

        using var reader = new CsvReader(new StreamReader(File.Open(path, FileMode.Open)), CultureInfo.InvariantCulture);

        if (!reader.Read() || !reader.ReadHeader() || reader.HeaderRecord is not { } headerRecord)
            return null;

        return headerRecord.Select(c => new ColumnInfo(c, ColumnType.String));
    }

    // TODO: To Utility
    private string? GetPath(ObjectName name)
    {
        if (name.Database is not null)
            throw new CsvException(CsvErrorCode.NotSupportedFeature, "vendor 'csv' does not supported ObjectName with database.");

        if (name.Schema is null)
            return null;

        try
        {
            return Path.Combine(_basePath, name.Schema, $"{name.Name}.csv");
        }
        catch (ArgumentException)
        {
            throw new InvalidTableNameException(name);
        }
    }

    public override string GetSchemaName()
    {
        return _schema;
    }

    public override void SetSchemaName(string schema)
    {
        _schema = schema;
    }

    public override ObjectName ResolveObjectName(ObjectName name)
    {
        if (name.Database is not null)
            throw new CsvException(CsvErrorCode.NotSupportedFeature, "vendor 'csv' does not supported ObjectName with database.");

        return name.Schema is not null
            ? name
            : new ObjectName(_schema, name.Name);
    }
}

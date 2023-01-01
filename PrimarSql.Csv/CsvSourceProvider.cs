using System.Dynamic;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using PrimarSql.Common;
using PrimarSql.Common.Data;
using PrimarSql.Common.Metadata;
using PrimarSql.Common.Providers;
using PrimarSql.Csv.Exceptions;

namespace PrimarSql.Csv;

public class CsvSourceProvider : VendorSourceProvider
{
    private readonly string _basePath;
    private const int PageItemCount = 10;

    public CsvSourceProvider(string basePath)
    {
        _basePath = basePath;
    }

    public override DataPageProvider GetTableDataPage(TableInfo table)
    {
        if (GetPath(table.Name) is not { } path)
            throw new CsvException(CsvErrorCode.Internal, "GetPath(ObjectName) returns null.");

        var reader = new CsvReader(new StreamReader(File.Open(path, FileMode.Open)), CultureInfo.InvariantCulture);
        return new CsvFileProvider(reader, table.Name.ToString());
    }

    public override int InsertToTable(ObjectName name, DataPageReader reader)
    {
        int affected = 0;

        try
        {
            if (GetPath(name) is not { } path)
                throw new CsvException(CsvErrorCode.Internal, "GetPath(ObjectName) returns null.");

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture);

            if (File.Exists(path))
                throw new CsvException(CsvErrorCode.Internal, $"Table {name} already exists.");

            using var writer = new CsvWriter(new StreamWriter(File.Open(path, FileMode.CreateNew)), configuration);

            // Write Column
            dynamic data = new ExpandoObject();
            var dataDict = (IDictionary<string, object>)data;

            for (int i = 0; i < reader.ColumnCount; i++)
                dataDict.Add(reader.Columns[i], reader.Columns[i]);

            writer.WriteRecord(data);
            writer.NextRecord();

            foreach (var record in reader)
            {
                if (record is null)
                    continue;

                data = new ExpandoObject();
                dataDict = (IDictionary<string, object>)data;

                for (int i = 0; i < reader.ColumnCount; i++)
                    dataDict.Add(reader.Columns[i], record[i]);

                writer.WriteRecord(data);
                writer.NextRecord();

                affected++;
            }
        }
        finally
        {
            reader.Dispose();
        }

        return affected;
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

    private class CsvFileProvider : DataPageProvider
    {
        public override int ColumnCount => _columns.Length;

        public override DataPage? Current => _current;

        public override string[] Columns => _columns;

        private readonly CsvReader _reader;
        private readonly string[] _columns;
        private readonly DataPage.Builder _pageBuilder;
        private readonly StringColumn.Builder[] _columnBuilders;

        private DataPage? _current;
        private bool _isClosed;

        public CsvFileProvider(CsvReader reader, string name)
        {
            _reader = reader;

            if (!_reader.Read() || !_reader.ReadHeader() || _reader.HeaderRecord is not { } columns)
            {
                throw new CsvException(CsvErrorCode.InvalidTable, $"Invalid Table: {name}");
            }

            _columns = columns;

            _pageBuilder = new DataPage.Builder()
                .SetName(name)
                .SetDataCount(PageItemCount);

            _columnBuilders = _columns
                .Select(v => new StringColumn.Builder().SetName(v))
                .ToArray();
        }

        public override bool Next()
        {
            if (_isClosed)
            {
                _current = null;
                return false;
            }

            int count = 0;

            for (int i = 0; i < PageItemCount; i++)
            {
                if (!_reader.Read())
                {
                    _isClosed = true;
                    break;
                }

                for (int j = 0; j < _columns.Length; j++)
                {
                    var builder = _columnBuilders[j];

                    if (_reader.GetField(j) is not { } data)
                        data = "NULL";

                    builder.Add(data);
                }

                count++;
            }

            _current = _pageBuilder
                .ClearColumns()
                .AddColumns(_columnBuilders.Select(c => c.Build()))
                .SetDataCount(count)
                .Build();

            return true;
        }

        public override void Dispose()
        {
            _current = null;
            _reader.Dispose();
        }
    }
}

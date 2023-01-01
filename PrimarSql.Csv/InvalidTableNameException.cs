using PrimarSql.Common;
using PrimarSql.Csv.Exceptions;

namespace PrimarSql.Csv;

public class InvalidTableNameException : CsvException
{
    public InvalidTableNameException(ObjectName name) : base(CsvErrorCode.InvalidTableName, $"Invalid table name: {name}")
    {
    }
}

using PrimarSql.Common;
using PrimarSql.Csv.Exceptions;

namespace PrimarSql.Csv;

public class CsvException : PrimarSqlException
{
    // TODO: Build with ErrorMessage
    private readonly CsvErrorCode _code;

    public CsvException(CsvErrorCode code, string errorMessage) : base(ErrorCode.Vendor, errorMessage)
    {
        _code = code;
    }
}

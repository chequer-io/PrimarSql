using System.Text;

namespace PrimarSql.Common;

public class PrimarSqlException : Exception
{
    public PrimarSqlException(string errorMessage) : this(ErrorCode.Internal, errorMessage)
    {
    }

    public PrimarSqlException(ErrorCode code, string errorMessage) : base(CreateMessage(code, errorMessage))
    {
        Code = code;
    }

    public ErrorCode Code { get; }

    private static string CreateMessage(ErrorCode error, string message)
    {
        var builder = new StringBuilder();

        // Error code
        builder.Append($"PS-{(int)error:0000}");

        if (!string.IsNullOrWhiteSpace(message))
            builder.Append(": ").Append(message);

        return builder.ToString();
    }
}

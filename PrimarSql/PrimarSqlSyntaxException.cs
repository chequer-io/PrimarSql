using System;

namespace PrimarSql
{
    public class PrimarSqlSyntaxException : Exception
    {
        public PrimarSqlSyntaxException(int line, int column, string message) : base($"[{line}:{column}] {message}")
        {
        }
    }
}

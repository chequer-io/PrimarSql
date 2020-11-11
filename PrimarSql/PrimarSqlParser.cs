using Antlr4.Runtime;
using PrimarSql.Internal;

namespace PrimarSql
{
    public sealed class PrimarSqlParser
    {
        public static IRootNode Parse(string sql)
        {
            var stream = new AntlrUpperInputStream(sql);
            var lexer = new PrimarSqlLexer(stream);
            var tokens = new CommonTokenStream(lexer);

            return (new Internal.PrimarSqlParser(tokens)).root();
        }
    }
}

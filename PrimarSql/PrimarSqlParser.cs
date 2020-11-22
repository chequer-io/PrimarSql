using Antlr4.Runtime;
using PrimarSql.Internal;

namespace PrimarSql
{
    public static class PrimarSqlParser
    {
        public static Internal.PrimarSqlParser.RootContext Parse(string sql)
        {
            var stream = new AntlrUpperInputStream(sql);
            var lexer = new PrimarSqlLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new Internal.PrimarSqlParser(tokens);
            
            parser.AddErrorListener(new AntlrErrorHandler());
            
            return parser.root();
        }
    }
}

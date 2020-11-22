using System.IO;
using Antlr4.Runtime;

namespace PrimarSql
{
    internal sealed class AntlrErrorHandler : IAntlrErrorListener<IToken>
    {
        void IAntlrErrorListener<IToken>.SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int column, string msg, RecognitionException e)
        {
            throw new PrimarSqlSyntaxException(line, column, msg);
        }
    }
}

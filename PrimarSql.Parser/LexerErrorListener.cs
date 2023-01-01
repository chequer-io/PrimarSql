using System.IO;
using Antlr4.Runtime;

namespace PrimarSql.Parser;

internal sealed class LexerErrorListener : IAntlrErrorListener<int>
{
    public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        throw new ParserInternalException(msg);
    }
}

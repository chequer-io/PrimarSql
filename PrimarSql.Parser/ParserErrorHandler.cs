using System.IO;
using Antlr4.Runtime;

namespace PrimarSql.Parser;

public class ParserErrorHandler : BaseErrorListener
{
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        throw new ParserInternalException(msg);
    }
}

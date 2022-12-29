using System.Linq;
using PrimarSql.Parser.Tree;
using static PrimarSql.Parser.Internal.PrimarSqlParser;

namespace PrimarSql.Parser;

internal partial class PrimarSqlVisitor
{
    #region Identifier
    public override Identifier VisitUnquotedIdentifier(UnquotedIdentifierContext context)
    {
        return new Identifier(context.GetPosition(), context.GetText(), false);
    }

    public override Node VisitQuotedIdentifier(QuotedIdentifierContext context)
    {
        return base.VisitQuotedIdentifier(context);
    }

    public override Node VisitBackQuotedIdentifier(BackQuotedIdentifierContext context)
    {
        return base.VisitBackQuotedIdentifier(context);
    }

    public override Node VisitDigitIdentifier(DigitIdentifierContext context)
    {
        return base.VisitDigitIdentifier(context);
    }
    #endregion

    private QualifiedName GetQualifiedName(QualifiedNameContext context)
    {
        return new QualifiedName(Visit<Identifier>(context.identifier()).ToArray());
    }
}

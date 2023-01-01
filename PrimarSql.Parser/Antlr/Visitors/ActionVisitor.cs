using System.Linq;
using PrimarSql.Parser.Nodes;
using static PrimarSql.Parser.Internal.PrimarSqlParser;

namespace PrimarSql.Parser.Antlr.Visitors;

internal partial class PrimarSqlVisitor
{
    public override Node VisitInsertInto(InsertIntoContext context)
    {
        var insert = new InsertInto(GetQualifiedName(context.qualifiedName()), Visit<Query>(context.query()));

        if (context.columnAliases() is { } columnAliases)
            insert.ColumnAliases = Visit<Identifier>(columnAliases.identifier()).ToArray();

        return insert.With(context);
    }
}

using PrimarSql.Common;
using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Analyzers;

public class RelationFrame
{
    public static readonly RelationFrame Empty = new(Array.Empty<Field>());

    public Field[] Fields { get; }

    public RelationFrame(Field[] fields)
    {
        Fields = fields;
    }

    public IEnumerable<Field> Resolve(QualifiedName name)
    {
        return Fields.Where(f => f.CanResolve(name));
    }

    public IEnumerable<Field> ResolveSource(QualifiedName name)
    {
        return Fields.Where(f => f.MatchSource(name.Identifiers));
    }

    public RelationFrame ToAliasedFrame(string aliasedName, string[]? columnAliases)
    {
        if (columnAliases is { })
        {
            if (columnAliases.Length != Fields.Length)
            {
                throw new PrimarSqlException(
                    ErrorCode.Syntax,
                    $"Column alias list has {columnAliases.Length} items." +
                    $"but '{aliasedName}' has {Fields.Length} columns available."
                );
            }
        }

        IEnumerable<Field> fields = Fields.Select((f, i) =>
        {
            string? name = columnAliases is null ? f.Name : columnAliases[i];
            return Field.New(name, QualifiedName.From(aliasedName), f.ColumnType, f.OriginName, f.OriginSource);
        });

        return new RelationFrame(fields.ToArray());
    }
}

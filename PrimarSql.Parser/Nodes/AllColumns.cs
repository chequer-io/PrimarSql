using System.Collections.Generic;

namespace PrimarSql.Parser.Nodes;

public class AllColumns : SelectItem
{
    public NullableValue<Expression> Target { get; } = new();

    public Identifier[] Aliases { get; }

    public AllColumns(Identifier[] aliases)
    {
        Aliases = aliases;
    }

    public override IEnumerable<Node> GetChildren()
    {
        foreach (var alias in Aliases)
            yield return alias;

        if (Target.Value is { })
            yield return Target.Value;
    }
}

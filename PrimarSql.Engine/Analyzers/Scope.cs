using PrimarSql.Parser.Nodes;

namespace PrimarSql.Engine.Analyzers;

internal class Scope
{
    public Scope? Parent { get; }

    public RelationFrame RelationFrame { get; set; }

    public Scope(RelationFrame relationFrame) : this(null, relationFrame)
    {
    }

    public Scope(Scope? parent, RelationFrame relationFrame)
    {
        Parent = parent;
        RelationFrame = relationFrame;
    }

    public Field? Find(QualifiedName source, string name)
    {
        var scope = this;

        while (true)
        {
            if (scope.Parent is null)
                break;

            var frame = scope.RelationFrame;

            try
            {
                return Array.Find(frame.Fields, f => f.Name == name && f.Source == source);
            }
            catch
            {
                // ignored
            }

            scope = scope.Parent;
        }

        return null;
    }
}

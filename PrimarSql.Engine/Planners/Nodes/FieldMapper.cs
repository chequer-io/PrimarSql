using PrimarSql.Engine.Analyzers;

namespace PrimarSql.Engine.Planners.Nodes;

internal class FieldMapper
{
    public Scope Scope { get; }

    private readonly Field[] _fields;
    private readonly Dictionary<Field, int> _mappings = new();

    public FieldMapper(Scope scope)
    {
        Scope = scope;
        _fields = Scope.RelationFrame.Fields;
    }

    public FieldMapper(Scope scope, IEnumerable<Field> fields)
    {
        Scope = scope;
        _fields = Scope.RelationFrame.Fields;

        // Scan
        foreach (var field in fields)
            Find(field);
    }

    // -1 means not found.
    public int Find(Field field)
    {
        if (_mappings.TryGetValue(field, out var index))
            return index;

        string? name = field.OriginName;
        var relation = field.OriginSource;

        if (relation is null)
        {
            _mappings[field] = -1;
        }
        else
        {
            try
            {
                _mappings[field] = Array.FindIndex(_fields, f => f.OriginName == name && f.OriginSource == relation);
            }
            catch
            {
                _mappings[field] = -1;
            }
        }

        return _mappings[field];
    }
}

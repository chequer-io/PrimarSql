namespace PrimarSql.Parser.Nodes;

public class NullableValue<T> : INodeValue where T : Node
{
    public T? Value { get; set; }

    public bool HasValue => Value is not null;

    #region INodeValue
    Node? INodeValue.Get()
    {
        return Value;
    }
    #endregion

    public override string ToString()
    {
        if (Value is { } value)
            return $"{value} ({value.GetType().Name})";

        return "Empty (Null)";
    }
}

using System;

namespace PrimarSql.Parser.Nodes;

public class NodeValue<T> : INodeValue where T : Node
{
    public T Value { get; set; }

    public bool HasValue => true;

    public NodeValue(T value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    Node INodeValue.Get()
    {
        return Value;
    }
}

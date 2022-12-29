using System;
using System.Collections.Generic;

namespace PrimarSql.Parser.Tree;

public abstract class Node
{
    public NodePosition Position { get; }

    protected Node(NodePosition position)
    {
        Position = position;
    }

    public abstract IEnumerable<T> GetChildrens<T>() where T : Node;

    public virtual bool DeepEquals(Node other)
    {
        throw new NotSupportedException($"{GetType().Name} node not supported DeepEquals(Node).");
    }
}

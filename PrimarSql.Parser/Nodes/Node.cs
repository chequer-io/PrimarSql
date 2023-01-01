using System;
using System.Collections.Generic;
using PrimarSql.Parser.Visitors;

namespace PrimarSql.Parser.Nodes;

public abstract class Node
{
    public NodePosition Position { get; set; }

    public abstract IEnumerable<Node> GetChildren();

    public virtual TValue? Accept<TValue, TContext>(NodeVisitorBase<TValue, TContext> visitor, TContext? context)
        where TValue : class where TContext : class
    {
        return visitor.Visit(this, context);
    }

    public virtual bool DeepEquals(Node other)
    {
        throw new NotSupportedException($"{GetType().Name} node not supported DeepEquals(Node).");
    }
}

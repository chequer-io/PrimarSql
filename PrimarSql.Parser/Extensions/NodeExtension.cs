using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PrimarSql.Parser.Tree;

namespace PrimarSql.Parser;

internal static class NodeExtension
{
    public static T? To<T>(this Node? node) where T : Node
    {
        if (node is null)
            return null;

        if (node is not T tNode)
        {
            throw new ParserInternalException(
                $"Expected node type {typeof(T).Name} but node was {node?.GetType().Name ?? "Unknown"}"
            );
        }

        return tNode;
    }

    public static NodePosition GetPosition(this ParserRuleContext context)
    {
        return GetPosition(context.Start);
    }

    public static NodePosition GetPosition(this ITerminalNode node)
    {
        return GetPosition(node.Symbol);
    }

    public static NodePosition GetPosition(this IToken start)
    {
        return new NodePosition(start.Line, start.Column, start.StartIndex);
    }
}

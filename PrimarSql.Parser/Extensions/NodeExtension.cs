using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PrimarSql.Parser.Nodes;

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
                $"Expected node type {typeof(T).Name} but node was {node.GetType().Name ?? "Unknown"}"
            );
        }

        return tNode;
    }

    public static T With<T>(this T node, ParserRuleContext context) where T : Node
    {
        node.Position = GetPosition(context);
        return node;
    }

    public static T With<T>(this T node, ITerminalNode terminalNode) where T : Node
    {
        node.Position = GetPosition(terminalNode);
        return node;
    }

    public static T With<T>(this T node, IToken start, IToken end) where T : Node
    {
        node.Position = GetPosition(start, end);
        return node;
    }

    public static NodePosition GetPosition(this ParserRuleContext context)
    {
        return GetPosition(context.Start, context.Stop);
    }

    public static NodePosition GetPosition(this ITerminalNode node)
    {
        return GetPosition(node.Symbol, node.Symbol);
    }

    public static NodePosition GetPosition(this IToken start, IToken end)
    {
        return new NodePosition(start.Line, start.Column, start.StartIndex, end.StopIndex + 1);
    }
}

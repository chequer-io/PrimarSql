namespace PrimarSql.Parser.Nodes;

public interface INodeValue
{
    bool HasValue { get; }

    Node? Get();
}

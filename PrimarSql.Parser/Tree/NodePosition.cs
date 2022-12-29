namespace PrimarSql.Parser.Tree;

public struct NodePosition
{
    public int Line { get; }

    public int Column { get; }

    public int Index { get; }

    public NodePosition(int line, int column, int index)
    {
        Line = line;
        Column = column;
        Index = index;
    }
}

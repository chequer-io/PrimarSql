namespace PrimarSql.Parser.Nodes;

public struct NodePosition
{
    public int Line { get; }

    public int Column { get; }

    public int Index { get; }

    public int EndIndex { get; }

    public NodePosition(int line, int column, int index, int endIndex)
    {
        Line = line;
        Column = column;
        Index = index;
        EndIndex = endIndex;
    }
}

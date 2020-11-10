namespace PrimarSql.Internal
{
    public interface ITableOptionThroughputContext : INode
    {
        int ReadCapacity { get; }

        int WriteCapacity { get; }
    }
}

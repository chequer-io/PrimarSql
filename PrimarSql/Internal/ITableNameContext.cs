namespace PrimarSql.Internal
{
    public interface ITableNameContext : INode
    {
        IFullIdContext FullId { get; }
    }
}
